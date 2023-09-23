using Agrin2.Helper.UIHelper.ASP;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ASP
{
    public static class PasswordExtensions 
    {    
        public static IHtmlContent PasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return PasswordFor(htmlHelper, expression, (string)null);
        }
        public static IHtmlContent PasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return PasswordFor(htmlHelper, expression,dictionary);
        }

        public static IHtmlContent PasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            Methods.SetCommonAttributes(htmlHelper, expression, ref htmlAttributes);
            return Microsoft.AspNetCore.Mvc.Rendering.HtmlHelperInputExtensions.PasswordFor(htmlHelper, expression);
        }
        
      
    }
}