using AdventureWorksApp.Data.Models;
using AdventureWorksApp.Data.Services.AdventureWorksServices;
using AdventureWorksApp.Helpers;
using Microsoft.AspNetCore.Components;
using Telerik.DataSource;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorksApp.Pages.SalesDepartment
{
    public partial class SalesGrid : ComponentBase
    {
        [Parameter]
        public string SalesOrderId { get; set; }

        [Inject]
        public SalesService _salesService { get; set; }

        [Inject]
        public NavigationManager _navigationManager { get; set; }

        public List<SalesOrderHeader> GridData { get; set; }

        public List<SalesOrderHeader> SourceData { get; set; }

        public SalesOrderHeader selectedSalesOrderHeader { get; set; }

        protected override void OnInitialized()
        {
            LoadData();
            base.OnInitialized();
        }

        public void LoadData()
        {
            SourceData = new List<SalesOrderHeader>();
            GridData = new List<SalesOrderHeader>(SourceData);
        }

        private async Task GridViewHandler(GridCommandEventArgs args)
        {
            int.TryParse(SalesOrderId, out var id);

            id = (args.Item as SalesOrderHeader).SalesOrderId;

            selectedSalesOrderHeader = _salesService.GetSalesOrderHeaderById(id);

            _navigationManager.NavigateTo("/SalesDepartment/OrderDetails/" + id);
        }

        private async Task GridDeleteHandler(GridCommandEventArgs args)
        {
            int.TryParse(SalesOrderId, out var id);

            id = (args.Item as SalesOrderHeader).SalesOrderId;

            selectedSalesOrderHeader = _salesService.GetSalesOrderHeaderById(id);

            if (selectedSalesOrderHeader.ActiveFlag == "Y")
            {
                _navigationManager.NavigateTo("/DeleteSalesOrderModal/" + id);
            }
        }

        #region Pager - to show 1000s of rows of data without a slow down
        protected async Task ReadItems(GridReadEventArgs args)
        {
            DataSourceResult result = await _salesService.ReadSalesOrderHeaderByQuerying(args.Request);

            var data = new DataEnvelope<SalesOrderHeader>
            {
                Data = result.Data.OfType<SalesOrderHeader>().ToList(), // current page data
                Total = result.Total // count of total items
            };

            args.Data = data.Data;
            args.Total = data.Total;
        }
        #endregion
    }
}
