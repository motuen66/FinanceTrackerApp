using System;
using AutoMapper;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.TransactionDtos;
using FinanceTracker.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : AuthenticatedControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        // GET api/transactions/range?from=2025-10-01&to=2025-10-31 (dates only, format: yyyy-MM-dd)
        [HttpGet("range")]
        public async Task<ActionResult<IEnumerable<TransactionViewDto>>> GetByRange([FromQuery] DateOnly from, [FromQuery] DateOnly to)
        {
            if (from > to)
            {
                return BadRequest(new { message = "from must be <= to" });
            }

            var userId = GetAuthenticatedUserId();

            // Normalize to date-only: include entire 'to' day by making the upper bound exclusive at next day
            var fromDate = from.ToDateTime(TimeOnly.MinValue);
            var toExclusive = to.AddDays(1).ToDateTime(TimeOnly.MinValue);

            var results = await _transactionService.GetByRangeWithCategoryNameAsync(userId, fromDate, toExclusive);
            return Ok(results);
        }

        // GET api/transactions/bytype?type=Income&from=2025-10-01&to=2025-10-31
        [HttpGet("bytype")]
        public async Task<ActionResult<IEnumerable<TransactionViewDto>>> GetByType([FromQuery] string type, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return BadRequest(new { message = "type is required (Income or Expense)" });
            }

            var userId = GetAuthenticatedUserId();

            DateTime? fromDt = null;
            DateTime? toDt = null;
            if (from.HasValue)
            {
                fromDt = from.Value.ToDateTime(TimeOnly.MinValue);
            }
            if (to.HasValue)
            {
                toDt = to.Value.AddDays(1).ToDateTime(TimeOnly.MinValue); // exclusive
            }

            var results = await _transactionService.GetByFilterWithCategoryNameAsync(userId, fromDt, toDt, type);
            return Ok(results);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionViewDto>>> GetAll()
        {
            var userId = GetAuthenticatedUserId();
            List<TransactionViewDto> transactions = await _transactionService.GetAllWithCategoryNameByUserIdAsync(userId);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionViewDto>> GetById(string id)
        {
            var transaction = await _transactionService.GetByIdWithCategoryNameAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            // Verify user owns this transaction
            var userId = GetAuthenticatedUserId();
            // Note: transaction here is TransactionViewDto from GetByIdWithCategoryNameAsync
            // We need to get the domain entity to check userId
            var domainTransaction = await _transactionService.GetByIdAsync(id);
            if (domainTransaction == null || domainTransaction.UserId != userId)
            {
                return Forbid();
            }

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionViewDto>> Create([FromBody] TransactionMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetAuthenticatedUserId();
            var transaction = _mapper.Map<Transaction>(dto);
            transaction.UserId = userId; // Ensure transaction belongs to authenticated user
            await _transactionService.AddAsync(transaction);

            var view = _mapper.Map<TransactionViewDto>(transaction);
            return CreatedAtAction(nameof(GetById), new { id = view.Id }, view);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TransactionViewDto>> Update(string id, [FromBody] TransactionMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _transactionService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            // Verify user owns this transaction
            var userId = GetAuthenticatedUserId();
            if (existing.UserId != userId)
            {
                return Forbid();
            }

            _mapper.Map(dto, existing);
            await _transactionService.UpdateAsync(existing);

            return Ok(_mapper.Map<TransactionViewDto>(existing));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _transactionService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            // Verify user owns this transaction
            var userId = GetAuthenticatedUserId();
            if (existing.UserId != userId)
            {
                return Forbid();
            }

            await _transactionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
