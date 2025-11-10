using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;

namespace FinanceTracker.Services.Interfaces.Services
{
    public interface IReportService : IGenericService<Report>
    {
        Task<IEnumerable<Report>> GetByUserIdAsync(string userId);
    }
}
