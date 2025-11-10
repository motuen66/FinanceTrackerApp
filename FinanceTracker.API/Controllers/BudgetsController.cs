using AutoMapper;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.BudgetDtos;
using FinanceTracker.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : AuthenticatedControllerBase
    {
        private readonly IBudgetService _budgetService;
        private readonly IMapper _mapper;

        public BudgetsController(IBudgetService budgetService, IMapper mapper)
        {
            _budgetService = budgetService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetViewDto>>> GetAll()
        {
            var userId = GetAuthenticatedUserId();
            List<BudgetViewDto> budgets = await _budgetService.GetAllIncludeCategoryByUserIdAsync(userId);
            return Ok(budgets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetViewDto>> GetById(string id)
        {
            var budget = await _budgetService.GetByIdAsync(id);
            if (budget == null)
            {
                return NotFound();
            }

            // Verify user owns this budget
            var userId = GetAuthenticatedUserId();
            if (budget.UserId != userId)
            {
                return Forbid();
            }

            return Ok(_mapper.Map<BudgetViewDto>(budget));
        }

        [HttpPost]
        public async Task<ActionResult<BudgetViewDto>> Create([FromBody] BudgetMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetAuthenticatedUserId();
            var budget = _mapper.Map<Budget>(dto);
            budget.UserId = userId; // Ensure budget belongs to authenticated user
            await _budgetService.AddAsync(budget);

            var view = _mapper.Map<BudgetViewDto>(budget);
            return CreatedAtAction(nameof(GetById), new { id = view.Id }, view);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BudgetViewDto>> Update(string id, [FromBody] BudgetMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _budgetService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            // Verify user owns this budget
            var userId = GetAuthenticatedUserId();
            if (existing.UserId != userId)
            {
                return Forbid();
            }

            _mapper.Map(dto, existing);
            await _budgetService.UpdateAsync(existing);

            return Ok(_mapper.Map<BudgetViewDto>(existing));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _budgetService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            // Verify user owns this budget
            var userId = GetAuthenticatedUserId();
            if (existing.UserId != userId)
            {
                return Forbid();
            }

            await _budgetService.DeleteAsync(id);
            return NoContent();
        }
    }
}
