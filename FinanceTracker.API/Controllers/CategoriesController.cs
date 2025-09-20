using AutoMapper;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.CategoryDtos;
using FinanceTracker.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryViewDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
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

            return Ok(_mapper.Map<CategoryViewDto>(category));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryViewDto>> Create([FromBody] CategoryMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(dto);
            await _categoryService.AddAsync(category);

            var view = _mapper.Map<CategoryViewDto>(category);
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

            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
