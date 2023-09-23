using Agrin2.Helper.UIHelper.ASP;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ASP
{
    public static class InputExtensionsOverride
    {
        public static IHtmlContent TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return TextBoxFor(htmlHelper, expression, (string)null);
        }
        public static IHtmlContent TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            return TextBoxFor(htmlHelper, expression, null, htmlAttributes);
        }
        public static IHtmlContent TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return TextBoxFor(htmlHelper, expression, null, dictionary);
        }
        public static IHtmlContent TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
        {
            return TextBoxFor(htmlHelper, expression, format, null);
        }
        public static IHtmlContent TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
        {
            Methods.SetCommonAttributes(htmlHelper, expression, ref htmlAttributes);
            return HtmlHelperInputExtensions.TextBoxFor(htmlHelper, expression, htmlAttributes);
        }       
      
    }

}