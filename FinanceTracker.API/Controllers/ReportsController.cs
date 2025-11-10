using AutoMapper;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.ReportDtos;
using FinanceTracker.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : AuthenticatedControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;

        public ReportsController(IReportService reportService, IMapper mapper)
        {
            _reportService = reportService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportViewDto>>> GetAll()
        {
            var userId = GetAuthenticatedUserId();
            var reports = await _reportService.GetByUserIdAsync(userId);
            return Ok(_mapper.Map<IEnumerable<ReportViewDto>>(reports));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportViewDto>> GetById(string id)
        {
            var report = await _reportService.GetByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            // Verify user owns this report
            var userId = GetAuthenticatedUserId();
            if (report.UserId != userId)
            {
                return Forbid();
            }

            return Ok(_mapper.Map<ReportViewDto>(report));
        }

        [HttpPost]
        public async Task<ActionResult<ReportViewDto>> Create([FromBody] ReportMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetAuthenticatedUserId();
            var report = _mapper.Map<Report>(dto);
            report.UserId = userId; // Ensure report belongs to authenticated user
            await _reportService.AddAsync(report);

            var view = _mapper.Map<ReportViewDto>(report);
            return CreatedAtAction(nameof(GetById), new { id = view.Id }, view);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReportViewDto>> Update(string id, [FromBody] ReportMutationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _reportService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            // Verify user owns this report
            var userId = GetAuthenticatedUserId();
            if (existing.UserId != userId)
            {
                return Forbid();
            }

            _mapper.Map(dto, existing);
            await _reportService.UpdateAsync(existing);

            return Ok(_mapper.Map<ReportViewDto>(existing));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _reportService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            // Verify user owns this report
            var userId = GetAuthenticatedUserId();
            if (existing.UserId != userId)
            {
                return Forbid();
            }

            await _reportService.DeleteAsync(id);
            return NoContent();
        }
    }
}
