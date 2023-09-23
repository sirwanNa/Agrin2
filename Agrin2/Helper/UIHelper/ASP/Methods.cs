using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Agrin2.Helper.UIHelper.ASP 
{
    public static class Methods
    {
        public static void SetCommonAttributes<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ref IDictionary<string, object> htmlAttributes)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }

            if (!htmlAttributes.ContainsKey("class"))
            {
                htmlAttributes.Add("class", "form-control");
            }
            if (!htmlAttributes.ContainsKey("autocomplete"))
            {
                htmlAttributes.Add("autocomplete", "off");
            }              
        }          

    }
}