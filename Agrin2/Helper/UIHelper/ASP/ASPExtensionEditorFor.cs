using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Linq.Expressions;

namespace ASP
{
    public static class EditorExtensions
    {
      
        public static IHtmlContent EditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
           // Methods.SetDefaultDateTime(html, expression);
            return HtmlHelperEditorExtensions.EditorFor(html, expression);
        }

       
    }
}