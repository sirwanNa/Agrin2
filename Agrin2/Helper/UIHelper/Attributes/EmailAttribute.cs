using System.ComponentModel.DataAnnotations;

namespace Agrin2.Helper.UIHelper.Attributes
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
        {
            ErrorMessage = "Please provide a valid email address";
        }
    }
}
