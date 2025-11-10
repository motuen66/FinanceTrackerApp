using AutoMapper;
using FinanceTracker.Domain;
using FinanceTracker.Services;
using FinanceTracker.Services.DTOs.CategoryDtos;
using FinanceTracker.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : AuthenticatedControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IBudgetService _budgetService;

        public CategoriesController(ICategoryService categoryService, IMapper mapper, IBudgetService budgetService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _budgetService = budgetService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryViewDto>>> GetAll()
        {
            var userId = GetAuthenticatedUserId();
            var categories = await _categoryService.GetByUserIdAsync(userId);
            return Ok(_mapper.Map<IEnumerable<CategoryViewDto>>(categories));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryViewDto>> GetById(string id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Verify user owns this category
            var userId = GetAuthenticatedUserId();
            if (category.UserId != userId)
            {
                return Forbid();
            }

            return Ok(_mapper.Map<CategoryViewDto>(category));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryViewDto>> Create([FromBody] CategoryMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetAuthenticatedUserId();
            var category = _mapper.Map<Category>(dto);
            category.UserId = userId; // Ensure category belongs to authenticated user
            await _categoryService.AddAsync(category);

            CategoryViewDto view = _mapper.Map<CategoryViewDto>(category);
            await _budgetService.AddAsync(new Budget
            {
                CategoryId = view.Id,
                LimitAmount = 0,
                Month = DateTime.UtcNow.Month,
                Year = DateTime.UtcNow.Year,
                UserId = userId
            });

            return CreatedAtAction(nameof(GetById), new { id = view.Id }, view);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryViewDto>> Update(string id, [FromBody] CategoryMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _categoryService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            // Verify user owns this category
            var userId = GetAuthenticatedUserId();
            if (existing.UserId != userId)
            {
                return Forbid();
            }

            _mapper.Map(dto, existing);
            await _categoryService.UpdateAsync(existing);

            return Ok(_mapper.Map<CategoryViewDto>(existing));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _categoryService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            // Verify user owns this category
            var userId = GetAuthenticatedUserId();
            if (existing.UserId != userId)
            {
                return Forbid();
            }

            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
