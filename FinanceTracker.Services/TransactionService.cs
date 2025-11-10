using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.TransactionDtos;
using FinanceTracker.Services.Interfaces.Repositories;
using FinanceTracker.Services.Interfaces.Services;

namespace FinanceTracker.Services
{
    public class TransactionService : GenericService<Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _cateRepo;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, ICategoryRepository cateRepo) : base(transactionRepository)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _cateRepo = cateRepo;
        }
        public Task<IEnumerable<Transaction>> GetByUserIdAndDateRangeAsync(string userId, DateTime from, DateTime to)
        {
            return _transactionRepository.GetByUserIdAndDateRangeAsync(userId, from, to);
        }
        public Task<IEnumerable<Transaction>> GetByFilterAsync(string? userId, DateTime? from, DateTime? to, string? type)
        {
            return _transactionRepository.GetByFilterAsync(userId, from, to, type);
        }

        public async Task<List<TransactionViewDto>> GetAllWithCategoryNameAsync()
        {
            IEnumerable<Transaction> trans = await _transactionRepository.GetAllAsync();
            trans = trans.ToList();

            List<TransactionViewDto> transactionDtos = _mapper.Map<List<TransactionViewDto>>(trans);

            foreach (TransactionViewDto tran in transactionDtos)
            {
                Category c = await _cateRepo.GetByIdAsync(tran.CategoryId);
                tran.CategoryName = c?.Name ?? "Unknown";
            }

            return transactionDtos;
        }

        public async Task<List<TransactionViewDto>> GetAllWithCategoryNameByUserIdAsync(string userId)
        {
            IEnumerable<Transaction> trans = await _transactionRepository.GetByUserIdAsync(userId);
            trans = trans.ToList();

            List<TransactionViewDto> transactionDtos = _mapper.Map<List<TransactionViewDto>>(trans);

            foreach (TransactionViewDto tran in transactionDtos)
            {
                Category c = await _cateRepo.GetByIdAsync(tran.CategoryId);
                tran.CategoryName = c?.Name ?? "Unknown";
            }

            return transactionDtos;
        }

        public async Task<List<TransactionViewDto>> GetByRangeWithCategoryNameAsync(string userId, DateTime from, DateTime to)
        {
            IEnumerable<Transaction> trans = await _transactionRepository.GetByUserIdAndDateRangeAsync(userId, from, to);
            trans = trans.ToList();

            List<TransactionViewDto> transactionDtos = _mapper.Map<List<TransactionViewDto>>(trans);

            foreach (TransactionViewDto tran in transactionDtos)
            {
                Category c = await _cateRepo.GetByIdAsync(tran.CategoryId);
                tran.CategoryName = c?.Name ?? "Unknown";
            }

            return transactionDtos;
        }

        public async Task<List<TransactionViewDto>> GetByFilterWithCategoryNameAsync(string? userId, DateTime? from, DateTime? to, string? type)
        {
            IEnumerable<Transaction> trans = await _transactionRepository.GetByFilterAsync(userId, from, to, type);
            trans = trans.ToList();

            List<TransactionViewDto> transactionDtos = _mapper.Map<List<TransactionViewDto>>(trans);

            foreach (TransactionViewDto tran in transactionDtos)
            {
                Category c = await _cateRepo.GetByIdAsync(tran.CategoryId);
                tran.CategoryName = c?.Name ?? "Unknown";
            }

            return transactionDtos;
        }

        public async Task<TransactionViewDto?> GetByIdWithCategoryNameAsync(string id)
        {
            Transaction? trans = await _transactionRepository.GetByIdAsync(id);
            if (trans == null)
            {
                return null;
            }

            TransactionViewDto transactionDto = _mapper.Map<TransactionViewDto>(trans);

            Category c = await _cateRepo.GetByIdAsync(transactionDto.CategoryId);
            transactionDto.CategoryName = c?.Name ?? "Unknown";

            return transactionDto;
        }
    }
}
