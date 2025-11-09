using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.BudgetDtos;
using FinanceTracker.Services.Interfaces.Repositories;
using FinanceTracker.Services.Interfaces.Services;

namespace FinanceTracker.Services
{
    public class BudgetService : GenericService<Budget>, IBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly ICategoryRepository _cateRepo;
        private readonly IMapper _mapper;

        public BudgetService(IBudgetRepository budgetRepository, ICategoryRepository cateRepo, IMapper mapper) : base(budgetRepository)
        {
            _budgetRepository = budgetRepository;
            _cateRepo = cateRepo;
            _mapper = mapper;
        }

        public async Task<List<BudgetViewDto>> GetAllIncludeCategoryAsync()
        {
            IEnumerable<Budget> budgets = await _budgetRepository.GetAllAsync();
            List<Budget> budgetList = budgets.ToList();

            List<BudgetViewDto> budgetDtos = _mapper.Map<List<BudgetViewDto>>(budgetList);

            foreach (BudgetViewDto b in budgetDtos) 
            {
                Category c = await _cateRepo.GetByIdAsync(b.CategoryId);
                b.CategoryName = c.Name;
            }

            return budgetDtos;
        }
    }
}
