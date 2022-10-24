using AdventureWorksApp.Data.Services.ReportsService;
using AdventureWorksApp.Data.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace AdventureWorksApp.Pages.ReportCharts
{
    public partial class ProductsByTypeChart : ComponentBase
    {
        [Inject]
        public ReportsDashboardService _reportsDashboardService { get; set; }

        [Parameter]
        public int ProductsCount { get; set; }

        private List<ItemByTypeChartModel> ChartData = new List<ItemByTypeChartModel>();

        protected override void OnParametersSet()
        {
            ChartData.Add(new ItemByTypeChartModel
            {
                CategoryName = "Product Name 1",
                IssueCount = _reportsDashboardService.GetProductsCount()
            });
        }

    }
}
