using Microsoft.AspNetCore.Mvc.Rendering;

namespace Agrin2.Helper.UIHelper
{
    public static class HTMLEditor
    {
        public static Agrin2Framework<TModel> Agrin2<TModel>(this IHtmlHelper<TModel> html)
        {
            return new Agrin2Framework<TModel>(html);
        }
    }

}
