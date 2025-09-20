using AutoMapper;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.BudgetDtos;
using FinanceTracker.Services.DTOs.CategoryDtos;
using FinanceTracker.Services.DTOs.SavingGoalDtos;
using FinanceTracker.Services.DTOs.TransactionDtos;
using FinanceTracker.Services.DTOs.UserDtos;
using FinanceTracker.Services.DTOs.ReportDtos;
namespace FinanceTracker.Services.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SavingGoal, SavingGoalViewDto>().ReverseMap();
            CreateMap<SavingGoal, SavingGoalMutationDto>().ReverseMap();

            CreateMap<User, UserViewDto>().ReverseMap();
            CreateMap<User, UserMutationDto>().ReverseMap();

            CreateMap<Transaction, TransactionViewDto>().ReverseMap();
            CreateMap<Transaction, TransactionMutationDto>().ReverseMap();

            CreateMap<Category, CategoryViewDto>().ReverseMap();
            CreateMap<Category, CategoryMutationDto>().ReverseMap();

            CreateMap<Budget, BudgetViewDto>().ReverseMap();
            CreateMap<Budget, BudgetMutationDto>().ReverseMap();

            CreateMap<Category, CategoryViewDto>().ReverseMap();
            CreateMap<Category, CategoryMutationDto>().ReverseMap();

            CreateMap<Report, ReportViewDto>().ReverseMap();
            CreateMap<Report, ReportMutationDto>().ReverseMap();
        }
    }
}
