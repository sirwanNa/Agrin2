using System.Collections.Generic;

namespace Agrin2.Helper.UIHelper.Grid
{
    public class Agrin2GridPaging
    {
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public Dictionary<string,object> Parameters { get; set; }
        public string FilterParameters { get; set; } 
    }
}
