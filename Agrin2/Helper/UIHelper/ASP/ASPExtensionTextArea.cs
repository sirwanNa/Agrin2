using Agrin2.Helper.UIHelper.ASP;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ASP
{
    public static class TextAreaExtensions
    {
        public static IHtmlContent TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return TextAreaFor(htmlHelper, expression, (string)null);
        }
        public static IHtmlContent TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return TextAreaFor(htmlHelper, expression, dictionary);

        }
        public static IHtmlContent TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            Methods.SetCommonAttributes(htmlHelper, expression, ref htmlAttributes);
            return HtmlHelperInputExtensions.TextAreaFor(htmlHelper, expression, htmlAttributes);
        }
    }
}