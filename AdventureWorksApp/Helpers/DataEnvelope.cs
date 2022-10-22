using System.Collections.Generic;

namespace AdventureWorksApp.Helpers
{
    public class DataEnvelope<T>
    {
        public int Total { get; set; }
        public List<T> Data { get; set; }
    }
}
