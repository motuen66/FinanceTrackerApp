 using AutoMapper;
using FinanceTracker.API.Handlers;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.UserDtos;
using FinanceTracker.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<UserViewDto>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewDto>> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserViewDto>(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserViewDto>> Create([FromBody] UserMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(dto);
            // Normalize email and hash password before saving
            user.Email = user.Email.Trim().ToLowerInvariant();
            user.PasswordHash = PasswordHashHandler.HashPassword(dto.PasswordHash);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _userService.AddAsync(user);

            var view = _mapper.Map<UserViewDto>(user);
            return CreatedAtAction(nameof(GetById), new { id = view.Id }, view);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserViewDto>> Update(string id, [FromBody] UserMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _userService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            _mapper.Map(dto, existing);
            // Normalize email and hash password before updating
            existing.Email = existing.Email.Trim().ToLowerInvariant();
            existing.PasswordHash = PasswordHashHandler.HashPassword(dto.PasswordHash);
            existing.UpdatedAt = DateTime.UtcNow;

            await _userService.UpdateAsync(existing);

            return Ok(_mapper.Map<UserViewDto>(existing));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _userService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
