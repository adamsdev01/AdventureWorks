using AdventureWorksApp.Data.Services.ReportsService;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace AdventureWorksApp.Pages.Reports
{
    public partial class Dashboard : ComponentBase
    {
        [Inject]
        public ReportsDashboardService _reportsDashboardService { get; set; }

        public int Products { get; set; }

        private async Task LoadDashboardData()
        {
            Products = _reportsDashboardService.GetProductsCount();
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDashboardData();
        }


    }
}
