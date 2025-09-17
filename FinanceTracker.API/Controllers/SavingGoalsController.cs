using FinanceTracker.Domain;
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

        public SavingGoalsController(ISavingGoalService savedGoalRepository)
        {
            _savingGoalService = savedGoalRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var goals = await _savingGoalService.GetAllAsync();
            return Ok(goals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var goal = await _savingGoalService.GetByIdAsync(id);
            if(goal == null)
            {
                return NotFound();
            }
            return Ok(goal);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SavingGoal savingGoal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _savingGoalService.AddAsync(savingGoal); 
            return Ok(savingGoal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] SavingGoal savingGoal){
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingGoal = await _savingGoalService.GetByIdAsync(id);
            if(existingGoal == null)
            {
                return NotFound();
            }
            savingGoal.Id = id;

            await _savingGoalService.UpdateAsync(savingGoal);
            return Ok(savingGoal);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){
            var existingGoal = await _savingGoalService.GetByIdAsync(id);
            if(existingGoal == null)
            {
                return NotFound();
            }
            await _savingGoalService.DeleteAsync(id);
            return Ok();
        }
    }
}
