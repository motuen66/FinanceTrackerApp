using AutoMapper;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.TransactionDtos;
using FinanceTracker.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionViewDto>>> GetAll()
        {
            var transactions = await _transactionService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TransactionViewDto>>(transactions));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionViewDto>> GetById(string id)
        {
            var transaction = await _transactionService.GetByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TransactionViewDto>(transaction));
        }

        [HttpPost]
        public async Task<ActionResult<TransactionViewDto>> Create([FromBody] TransactionMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = _mapper.Map<Transaction>(dto);
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

            await _transactionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
