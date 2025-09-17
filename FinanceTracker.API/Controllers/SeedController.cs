using FinanceTracker.Domain;
using FinanceTracker.Infrastructure.Database;
using FinanceTracker.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace FinanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly MongoDbContext _context;
        private readonly ISavingGoalService _savingGoalService;
        private readonly IBudgetService _budgetService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly ITransactionService _transactionService;

        public SeedController(
            MongoDbContext context,
            ISavingGoalService savingGoalService,
            IBudgetService budgetService,
            IUserService userService,
            ICategoryService categoryService,
            ITransactionService transactionService)
        {
            _context = context;
            _savingGoalService = savingGoalService;
            _budgetService = budgetService;
            _userService = userService;
            _categoryService = categoryService;
            _transactionService = transactionService;
        }

        [HttpPost("seed-data")]
        public async Task<IActionResult> SeedData()
        {
            // Tạo người dùng mẫu
            var user = new User
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                Name = "tuDK",
                Email = "user@example.com",
                PasswordHash = "hashed_password_here",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _userService.AddAsync(user);

            // Tạo danh mục mẫu
            var incomeCategory = new Category
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                Name = "Extra Money",
                Type = "Income",
                UserId = user.Id
            };

            var expenseCategory1 = new Category
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                Name = "Food",
                Type = "Expense",
                UserId = user.Id
            };

            var expenseCategory2 = new Category
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                Name = "Transportation",
                Type = "Expense",
                UserId = user.Id
            };

            _categoryService.AddAsync(expenseCategory1);
            _categoryService.AddAsync(expenseCategory2);
            _categoryService.AddAsync(incomeCategory);

            // Tạo giao dịch mẫu
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                    Note = "May Salary",
                    Amount = 10000000,
                    Date = DateTime.Now.AddDays(-15),
                    UserId = user.Id,
                    Type = "Income",
                    CategoryId = incomeCategory.Id
                },
                new Transaction
                {
                    Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                    Note = "Lunch",
                    Amount = 50000,
                    Date = DateTime.Now.AddDays(-2),
                    UserId = user.Id,
                    Type = "Expense",
                    CategoryId = expenseCategory1.Id
                },
                new Transaction
                {
                    Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                    Note = "Fuels",
                    Amount = 100000,
                    Date = DateTime.Now.AddDays(-5),
                    UserId = user.Id,
                    Type = "Expense",
                    CategoryId = expenseCategory2.Id
                }
            };
            foreach(var transaction in transactions)
            {
                _transactionService.AddAsync(transaction);
            }

            // Tạo mục tiêu tiết kiệm mẫu
            var savingGoal = new SavingGoal
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                UserId = user.Id,
                Title = "Buy a new phone",
                GoalAmount = 2000000,
                CurrentAmount = 5000000,
                GoalDate = DateTime.Now.AddMonths(6),
                IsCompleted = false
            };
            await _savingGoalService.AddAsync(savingGoal);

            // Tạo ngân sách mẫu    
            var budget = new Budget
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                UserId = user.Id,
                CategoryId = expenseCategory1.Id,
                LimitAmount = 2000000,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year
            };
            await _budgetService.AddAsync(budget);

            return Ok("Data has been seeded successfully!");
        }
    }
}