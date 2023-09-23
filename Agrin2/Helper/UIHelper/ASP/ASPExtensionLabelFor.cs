using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Linq.Expressions;

namespace ASP
{
    public static class DisplayNameExtensions
    {
        public static IHtmlContent DisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return HtmlHelperLabelExtensions.LabelFor(html, expression);
        }
    }
}