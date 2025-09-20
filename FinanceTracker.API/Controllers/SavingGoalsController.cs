using AutoMapper;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.SavingGoalDtos;
using FinanceTracker.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingGoalsController : ControllerBase
    {
        private readonly ISavingGoalService _savingGoalService;
        private readonly IMapper _mapper;

        public SavingGoalsController(ISavingGoalService savedGoalRepository, IMapper mapper)
        {
            _savingGoalService = savedGoalRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var goals = await _savingGoalService.GetAllAsync();
            var goalsDto = _mapper.Map<List<SavingGoalViewDto>>(goals);
            return Ok(goalsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var goal = await _savingGoalService.GetByIdAsync(id);
            if(goal == null)
            {
                return NotFound();
            }
            var goalDto = _mapper.Map<SavingGoalViewDto>(goal);
            return Ok(goalDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SavingGoalMutationDto savingGoaldto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var savingGoal = _mapper.Map<SavingGoal>(savingGoaldto);

            await _savingGoalService.AddAsync(savingGoal); 

            var view = _mapper.Map<SavingGoalViewDto>(savingGoal);
            return CreatedAtAction(nameof(GetById), new { id = view.Id}, view);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] SavingGoalMutationDto savingGoalDto){
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingGoal = await _savingGoalService.GetByIdAsync(id);
            if(existingGoal == null)
            {
                return NotFound();
            }
            _mapper.Map(savingGoalDto, existingGoal); 
            await _savingGoalService.UpdateAsync(existingGoal);
            return Ok(_mapper.Map<SavingGoalViewDto>(existingGoal));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePartial(string id, [FromBody] SavingGoalMutationDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _savingGoalService.GetByIdAsync(id);
            if(existing == null)
            {
                return NotFound($"The goal is not found!");
            }

            await _savingGoalService.UpdateSavingGoalAsync(id, updateDto);
            return Ok ();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){
            var existingGoal = await _savingGoalService.GetByIdAsync(id);
            if(existingGoal == null)
            {
                return NotFound();
            }
            await _savingGoalService.DeleteAsync(id);
            return NoContent();
        }
    }
}
