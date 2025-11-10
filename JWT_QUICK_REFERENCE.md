# Quick Reference: JWT Authorization Pattern

## For Developers Adding New Controllers

When creating a new controller that requires user-scoped data access, follow this pattern:

### 1. Controller Setup
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [Authorize]  // Require JWT token for all endpoints
    [Route("api/[controller]")]
    [ApiController]
    public class YourController : AuthenticatedControllerBase  // Inherit from this
    {
        private readonly IYourService _service;
        private readonly IMapper _mapper;

        public YourController(IYourService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
```

### 2. GetAll Pattern
```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<YourViewDto>>> GetAll()
{
    // Get userId from JWT token
    var userId = GetAuthenticatedUserId();
    
    // Get only this user's data
    var items = await _service.GetByUserIdAsync(userId);
    
    return Ok(_mapper.Map<IEnumerable<YourViewDto>>(items));
}
```

### 3. GetById Pattern
```csharp
[HttpGet("{id}")]
public async Task<ActionResult<YourViewDto>> GetById(string id)
{
    var item = await _service.GetByIdAsync(id);
    if (item == null)
    {
        return NotFound();
    }

    // Verify ownership
    var userId = GetAuthenticatedUserId();
    if (item.UserId != userId)
    {
        return Forbid();  // 403 if not owned by user
    }

    return Ok(_mapper.Map<YourViewDto>(item));
}
```

### 4. Create Pattern
```csharp
[HttpPost]
public async Task<ActionResult<YourViewDto>> Create([FromBody] YourMutationDto dto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    // Get userId from token
    var userId = GetAuthenticatedUserId();
    
    var item = _mapper.Map<YourEntity>(dto);
    item.UserId = userId;  // Force ownership to authenticated user
    
    await _service.AddAsync(item);

    var view = _mapper.Map<YourViewDto>(item);
    return CreatedAtAction(nameof(GetById), new { id = view.Id }, view);
}
```

### 5. Update Pattern
```csharp
[HttpPut("{id}")]
public async Task<ActionResult<YourViewDto>> Update(string id, [FromBody] YourMutationDto dto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var existing = await _service.GetByIdAsync(id);
    if (existing == null)
    {
        return NotFound();
    }

    // Verify ownership
    var userId = GetAuthenticatedUserId();
    if (existing.UserId != userId)
    {
        return Forbid();
    }

    _mapper.Map(dto, existing);
    await _service.UpdateAsync(existing);

    return Ok(_mapper.Map<YourViewDto>(existing));
}
```

### 6. Delete Pattern
```csharp
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(string id)
{
    var existing = await _service.GetByIdAsync(id);
    if (existing == null)
    {
        return NotFound();
    }

    // Verify ownership
    var userId = GetAuthenticatedUserId();
    if (existing.UserId != userId)
    {
        return Forbid();
    }

    await _service.DeleteAsync(id);
    return NoContent();
}
```

## Repository Layer

### Interface
```csharp
public interface IYourRepository : IGenericRepository<YourEntity>
{
    Task<IEnumerable<YourEntity>> GetByUserIdAsync(string userId);
    // Add other custom queries here
}
```

### Implementation
```csharp
using MongoDB.Bson;
using MongoDB.Driver;

public class YourRepository : GenericRepository<YourEntity>, IYourRepository
{
    public YourRepository(MongoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<YourEntity>> GetByUserIdAsync(string userId)
    {
        var filter = Builders<YourEntity>.Filter.Eq("userId", new ObjectId(userId));
        return await Collection.Find(filter).ToListAsync();
    }
}
```

## Service Layer

### Interface
```csharp
public interface IYourService : IGenericService<YourEntity>
{
    Task<IEnumerable<YourEntity>> GetByUserIdAsync(string userId);
}
```

### Implementation
```csharp
public class YourService : GenericService<YourEntity>, IYourService
{
    private readonly IYourRepository _repository;

    public YourService(IYourRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<YourEntity>> GetByUserIdAsync(string userId)
    {
        return _repository.GetByUserIdAsync(userId);
    }
}
```

## Common Patterns

### Allow Anonymous Access (e.g., Registration)
```csharp
[AllowAnonymous]
[HttpPost("register")]
public async Task<ActionResult> Register([FromBody] RegisterDto dto)
{
    // No authentication required
    // ...
}
```

### Self-Service Pattern (Users managing their own data)
```csharp
[HttpGet("me")]
public async Task<ActionResult<UserDto>> GetCurrentUser()
{
    var userId = GetAuthenticatedUserId();
    var user = await _userService.GetByIdAsync(userId);
    return Ok(_mapper.Map<UserDto>(user));
}
```

### Conditional Ownership Check
```csharp
// Only check ownership if ID is provided and not "me"
if (id != "me")
{
    var userId = GetAuthenticatedUserId();
    if (id != userId)
    {
        return Forbid();
    }
}
```

## Testing Checklist

- [ ] Login and obtain JWT token
- [ ] Test GetAll returns only authenticated user's data
- [ ] Test GetById with owned resource (should succeed)
- [ ] Test GetById with another user's resource (should return 403)
- [ ] Test Create automatically assigns userId
- [ ] Test Update with owned resource (should succeed)
- [ ] Test Update with another user's resource (should return 403)
- [ ] Test Delete with owned resource (should succeed)
- [ ] Test Delete with another user's resource (should return 403)
- [ ] Test endpoints without token (should return 401)
- [ ] Test endpoints with invalid token (should return 401)

## HTTP Status Codes

- **200 OK**: Successful GET/PUT
- **201 Created**: Successful POST
- **204 No Content**: Successful DELETE
- **400 Bad Request**: Invalid input
- **401 Unauthorized**: Missing or invalid token
- **403 Forbidden**: Valid token but user doesn't own the resource
- **404 Not Found**: Resource doesn't exist

## Common Mistakes to Avoid

❌ **DON'T** use `GetAllAsync()` in protected controllers
```csharp
var items = await _service.GetAllAsync(); // Returns ALL users' data!
```

✅ **DO** use `GetByUserIdAsync()`
```csharp
var userId = GetAuthenticatedUserId();
var items = await _service.GetByUserIdAsync(userId); // Only this user's data
```

❌ **DON'T** forget to verify ownership in GetById/Update/Delete
```csharp
var item = await _service.GetByIdAsync(id);
return Ok(item); // Might return another user's data!
```

✅ **DO** always check ownership
```csharp
var item = await _service.GetByIdAsync(id);
if (item.UserId != GetAuthenticatedUserId())
    return Forbid();
return Ok(item);
```

❌ **DON'T** accept userId from client in POST/PUT
```csharp
var item = _mapper.Map<Item>(dto); // dto.UserId might be malicious
await _service.AddAsync(item);
```

✅ **DO** force userId from token
```csharp
var item = _mapper.Map<Item>(dto);
item.UserId = GetAuthenticatedUserId(); // Always use token userId
await _service.AddAsync(item);
```
