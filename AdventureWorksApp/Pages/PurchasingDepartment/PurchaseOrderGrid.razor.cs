using AdventureWorksApp.Data.Models;
using AdventureWorksApp.Data.Services.AdventureWorksServices;
using AdventureWorksApp.Helpers;
using Microsoft.AspNetCore.Components;
using Telerik.DataSource;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorksApp.Pages.PurchasingDepartment
{
    public partial class PurchaseOrderGrid : ComponentBase
    {

        [Parameter]
        public string PurchaseOrderId { get; set; }

        [Inject]
        public PurchaseService _purchaseService { get; set; }

        [Inject]
        public NavigationManager _navigationManager { get; set; }

        public List<PurchaseOrderHeader> GridData { get; set; }

        public List<PurchaseOrderHeader> SourceData { get; set; }

        public PurchaseOrderHeader selectedPurchaseOrderHeader { get; set; }

        protected override void OnInitialized()
        {
            LoadData();
        }

        public void LoadData()
        {
            SourceData = new List<PurchaseOrderHeader>();
            GridData = new List<PurchaseOrderHeader>(SourceData);
        }

        private async Task GridViewHandler(GridCommandEventArgs args)
        {
            int.TryParse(PurchaseOrderId, out var id);

            id = (args.Item as PurchaseOrderHeader).PurchaseOrderId;

            selectedPurchaseOrderHeader = _purchaseService.GetPurchaseOrderHeaderById(id);

            if (selectedPurchaseOrderHeader.PurchaseOrderId > id)
            {
                _navigationManager.NavigateTo("/ViewPurchaseOrderHeaderDetails/" + id);
            }
        }

        private async Task GridDeleteHandler(GridCommandEventArgs args)
        {
            int.TryParse(PurchaseOrderId, out var id);

            id = (args.Item as PurchaseOrderHeader).PurchaseOrderId;

            selectedPurchaseOrderHeader = _purchaseService.GetPurchaseOrderHeaderById(id);

            if (selectedPurchaseOrderHeader.ActiveFlag == "Y")
            {
                _navigationManager.NavigateTo("/DeletePurchaseOrderHeaderModal/" + id);
            }
        }

        #region Pager - to show 1000s of rows of data without a slow down
        protected async Task ReadItems(GridReadEventArgs args)
        {
            DataSourceResult result = await _purchaseService.ReadPurchaseOrderHeadersByQuerying(args.Request);

            var data = new DataEnvelope<PurchaseOrderHeader>
            {
                Data = result.Data.OfType<PurchaseOrderHeader>().ToList(), // current page data
                Total = result.Total // count of total items
            };

            args.Data = data.Data;
            args.Total = data.Total;
        }
        #endregion

        // a helper method to extract the foreign key data that we want to render
        // this needs to be fast and synchronous because each render needs it many times
        //public string GetPersonFullName(int businessEntityId)
        //{
        //    Person person = _personService.GetPersonById(businessEntityId);
        //    var matchingPosFullName = person.FirstName + " " + person.MiddleName + "." + person.LastName;

        //    return matchingPosFullName;
        //}
    }
}
