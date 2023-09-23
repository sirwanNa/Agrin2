using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace Agrin2.Helper.UIHelper.Form
{
    public static class ModelStateExtension
    {
        public static string GetErrors(this ModelStateDictionary modelState, string seperator = ",")
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return string.Join(seperator, errorList);
        }
    }
}
