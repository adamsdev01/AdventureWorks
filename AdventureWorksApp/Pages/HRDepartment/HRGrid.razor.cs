using AdventureWorksApp.Data.Models;
using AdventureWorksApp.Data.Services.AdventureWorksServices;
using AdventureWorksApp.Helpers;
using Microsoft.AspNetCore.Components;
using Telerik.DataSource;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorksApp.Pages.HRDepartment
{
    public partial class HRGrid : ComponentBase
    {
        [Parameter]
        public string BusinessEntityId { get; set; }

        [Inject]
        public HRService _hrService { get; set; }

        [Inject]
        public NavigationManager _navigationManager { get; set; }

        public List<Employee> GridData { get; set; }

        public List<Employee> SourceData { get; set; }

        public Employee selectedEmployee { get; set; }

        protected override void OnInitialized()
        {
            LoadData();
        }

        public void LoadData()
        {
            SourceData = new List<Employee>();
            GridData = new List<Employee>(SourceData);
        }

        private async Task GridViewHandler(GridCommandEventArgs args)
        {
            int.TryParse(BusinessEntityId, out var id);

            id = (args.Item as Employee).BusinessEntityId;

            selectedEmployee = _hrService.GetEmployeeById(id);

            if (selectedEmployee.BusinessEntityId > id)
            {
                _navigationManager.NavigateTo("/ViewEmployeeDetails/" + id);
            }
        }

        private async Task GridDeleteHandler(GridCommandEventArgs args)
        {
            int.TryParse(BusinessEntityId, out var id);

            id = (args.Item as Employee).BusinessEntityId;

            selectedEmployee = _hrService.GetEmployeeById(id);

            if (selectedEmployee.CurrentFlag == true)
            {
                _navigationManager.NavigateTo("/DeleteEmployeeModal/" + id);
            }
        }

        #region Pager - to show 1000s of rows of data without a slow down
        protected async Task ReadItems(GridReadEventArgs args)
        {
            DataSourceResult result = await _hrService.ReadEmployeeByQuerying(args.Request);

            var data = new DataEnvelope<Employee>
            {
                Data = result.Data.OfType<Employee>().ToList(), // current page data
                Total = result.Total // count of total items
            };

            args.Data = data.Data;
            args.Total = data.Total;
        }
        #endregion

        // a helper method to extract the foreign key data that we want to render
        // this needs to be fast and synchronous because each render needs it many times
        public string GetPersonFirstNameFromId(int businessEntityId)
        {
            Person person = _hrService.GetPersonByEmployeeId(businessEntityId);
            var matchingPosFullName = person.FirstName + " " + person.LastName;

            return matchingPosFullName;
        }
    }
}
