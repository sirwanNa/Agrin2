using Agrin2.Helper.UIHelper.ASP;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ASP
{
    public static class SelectExtensions 
    {
   
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            return DropDownListFor(htmlHelper, expression, selectList, null, null);
        }     
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return DropDownListFor(htmlHelper, expression, selectList, null, dictionary);
        }
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
        }     
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel)
        {
            return DropDownListFor(htmlHelper, expression, selectList, optionLabel, null);
        }
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return DropDownListFor(htmlHelper, expression, selectList, optionLabel, dictionary);
        }
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            Methods.SetCommonAttributes(htmlHelper, expression,ref htmlAttributes);
            return HtmlHelperSelectExtensions.DropDownListFor(htmlHelper, expression, selectList);
        }  
      
    }
}