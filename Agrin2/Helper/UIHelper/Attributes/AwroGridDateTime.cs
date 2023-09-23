using System;

namespace Agrin2.Helper.UIHelper.Attributes
{
    public class Agrin2GridDateTime:Attribute
    {
        public string Format { get; set; }
        public bool ShowTime { get; set; }
    }
}
