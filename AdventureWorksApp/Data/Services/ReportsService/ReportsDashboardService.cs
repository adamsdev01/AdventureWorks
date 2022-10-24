using AdventureWorksApp.Data.Models;
using System.Linq;

namespace AdventureWorksApp.Data.Services.ReportsService
{
    public class ReportsDashboardService
    {
        private readonly AdventureWorksContext _context;

        public ReportsDashboardService(AdventureWorksContext context)
        {
            _context = context;
        }

        public int GetProductsCount()
        {
            var count = _context.Product
                .Where(r => r.ActiveFlag == "Y")
                .Count();

            return count;
        }
    }
}
