using AdventureWorksApp.Data.Models;
using AdventureWorksApp.Data.Services.AdventureWorksServices;
using AdventureWorksApp.Helpers;
using Microsoft.AspNetCore.Components;
using Telerik.DataSource;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorksApp.Pages.ContactInfo
{
    public partial class ContactsGrid : ComponentBase
    {
        [Parameter]
        public string BusinessEntityId { get; set; }

        [Inject]
        public PersonsService _personService { get; set; }

        [Inject]
        public NavigationManager _navigationManager { get; set; }

        public List<Person> GridData { get; set; }

        public List<Person> SourceData { get; set; }

        public Person selectedPerson { get; set; }

        protected override void OnInitialized()
        {
            LoadData();
        }

        public void LoadData()
        {
            SourceData = new List<Person>();
            GridData = new List<Person>(SourceData);
        }

        private async Task GridViewHandler(GridCommandEventArgs args)
        {
            int.TryParse(BusinessEntityId, out var id);

            id = (args.Item as Person).BusinessEntityId;

            selectedPerson = _personService.GetPersonById(id);

            if (selectedPerson.BusinessEntityId > id)
            {
                _navigationManager.NavigateTo("/ViewPersonDetails/" + id);
            }
        }

        private async Task GridDeleteHandler(GridCommandEventArgs args)
        {
            int.TryParse(BusinessEntityId, out var id);

            id = (args.Item as Person).BusinessEntityId;

            selectedPerson = _personService.GetPersonById(id);

            if (selectedPerson.ActiveFlag == "Y")
            {
                _navigationManager.NavigateTo("/DeletePersonModal/" + id);
            }
        }

        #region Pager - to show 1000s of rows of data without a slow down
        protected async Task ReadItems(GridReadEventArgs args)
        {
            DataSourceResult result = await _personService.ReadPersonsByQuerying(args.Request);

            var data = new DataEnvelope<Person>
            {
                Data = result.Data.OfType<Person>().ToList(), // current page data
                Total = result.Total // count of total items
            };

            args.Data = data.Data;
            args.Total = data.Total;
        }
        #endregion

        // a helper method to extract the foreign key data that we want to render
        // this needs to be fast and synchronous because each render needs it many times
        public string GetPersonFullName(int businessEntityId)
        {
            Person person = _personService.GetPersonById(businessEntityId);
            var matchingPosFullName = person.FirstName + " " + person.MiddleName + "." + person.LastName;

            return matchingPosFullName;
        }
    }
}
