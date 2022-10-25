using AdventureWorksApp.Data.Models;
using AdventureWorksApp.Data.Services.AdventureWorksServices;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureWorksApp.Pages.SalesDepartment
{
    public partial class OrderDetails : ComponentBase
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        public SalesService salesService { get; set; }

        public SalesOrderHeader selectedSalesOrderHeader { get; set; }

        public List<SalesOrderHeader> salesOrderHeaders;

        public SalesOrderHeader SalesOrderHeader { get; set; } = new SalesOrderHeader();

        protected override async void OnInitialized()
        {
            int.TryParse(Id, out var id);
            selectedSalesOrderHeader = salesService.GetSalesOrderHeaderById(id);
            SalesOrderHeader = salesService.GetSalesOrderHeaderById(id);
        }
    }
}
