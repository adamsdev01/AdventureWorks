using AdventureWorksApp.Data.Models;
using AdventureWorksApp.Data.Services.AdventureWorksServices;
using AdventureWorksApp.Helpers;
using Microsoft.AspNetCore.Components;
using Telerik.DataSource;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorksApp.Pages.ProductDepartment
{
    public partial class ProductsGrid : ComponentBase
    {
        [Parameter]
        public string ProductId { get; set; }

        [Inject]
        public ProductService _productService { get; set; }

        [Inject]
        public NavigationManager _navigationManager { get; set; }

        public List<Product> GridData { get; set; }

        public List<Product> SourceData { get; set; }

        public Product selectedProduct { get; set; }

        protected override void OnInitialized()
        {
            LoadData();
        }

        public void LoadData()
        {
            SourceData = new List<Product>();
            GridData = new List<Product>(SourceData);
        }

        private async Task GridViewHandler(GridCommandEventArgs args)
        {
            int.TryParse(ProductId, out var id);

            id = (args.Item as Person).BusinessEntityId;

            selectedProduct = _productService.GetProductById(id);

            if (selectedProduct.ProductId > id)
            {
                _navigationManager.NavigateTo("/ViewProductDetails/" + id);
            }
        }

        private async Task GridDeleteHandler(GridCommandEventArgs args)
        {
            int.TryParse(ProductId, out var id);

            id = (args.Item as Person).BusinessEntityId;

            selectedProduct = _productService.GetProductById(id);

            if (selectedProduct.ActiveFlag == "Y")
            {
                _navigationManager.NavigateTo("/DeleteProductModal/" + id);
            }
        }

        #region Pager - to show 1000s of rows of data without a slow down
        protected async Task ReadItems(GridReadEventArgs args)
        {
            DataSourceResult result = await _productService.ReadProductsByQuerying(args.Request);

            var data = new DataEnvelope<Product>
            {
                Data = result.Data.OfType<Product>().ToList(), // current page data
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
