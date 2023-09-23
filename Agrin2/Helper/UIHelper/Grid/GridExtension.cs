using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Agrin2.Helper.UIHelper.Attributes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Agrin2.Helper.UIHelper.Gird
{
    public static class GridExtension
    {
        public static HtmlString Grid<TModel, TValue>(this IHtmlHelper<TModel> html, string gridName,string gridTitle, bool showSearchPanel, bool showAddButton, string url, string pagesCountUrl, string areaName, bool showEdit, bool ShowDelete, bool showPaging, bool initiateGrid, string parentObjectName, Agrin2GridPagingType pagingType, Dictionary<string, bool> showIfParameters,bool isCollapse)
        {
            var properties = typeof(TValue).GetProperties();
            properties = setOrders(properties);
            url = updateUrl(html, url, areaName);
            var countUrl = string.IsNullOrEmpty(pagesCountUrl) ? getPagesCountUrl(html, areaName) : pagesCountUrl;
            var deleteUrl = getDeleteUrl(html, areaName);
            string result = renderGrid(gridName, gridTitle, showSearchPanel, showAddButton, showEdit, ShowDelete, showPaging, initiateGrid, parentObjectName, null, properties, url, countUrl, deleteUrl, isCollapse);
            return new HtmlString(result);
        }

        private static PropertyInfo[] setOrders(PropertyInfo[] properties)
        {
            if (properties != null)
            {
                var haveOrdersList = properties.Where(c => c.GetCustomAttribute<Agrin2GridOrder>() != null)
                     .OrderBy(c => c.GetCustomAttribute<Agrin2GridOrder>().Value).ToList();
                var doNotHaveOrdersList = properties.Where(c => c.GetCustomAttribute<Agrin2GridOrder>() == null);
                var tempResult = new List<PropertyInfo>();
                tempResult = tempResult.Concat(haveOrdersList).ToList();
                tempResult = tempResult.Concat(doNotHaveOrdersList).ToList();
                properties = tempResult.ToArray();
            }

            return properties;
        }

        private static string updateUrl(IHtmlHelper html, string url, string areaName)
        {
            if (string.IsNullOrEmpty(url))
            {
                var controllerName = html.ViewContext.RouteData.Values["controller"];
                url = "/" + controllerName + "/List";
            }
            url = (!string.IsNullOrEmpty(areaName) && !url.Contains(areaName) ? "/" + areaName : "") + url;
            return url;
        }
        private static string getPagesCountUrl(IHtmlHelper html, string areaName)
        {
            var controllerName = html.ViewContext.RouteData.Values["controller"];
            var url = (!string.IsNullOrEmpty(areaName) ? "/" + areaName : "") + "/" + controllerName + "/GetPagesCount?pageCount=10";
            return url;
        }
        private static string getDeleteUrl(IHtmlHelper html, string areaName)
        {
            var controllerName = html.ViewContext.RouteData.Values["controller"];
            var url = (!string.IsNullOrEmpty(areaName) ? "/" + areaName : "") + "/" + controllerName + "/Delete";
            return url;
        }

        public static HtmlString Grid<TModel, TValue, TSearch>(this IHtmlHelper<TModel> html, string gridName, string gridTitle, bool showSearchPanel, bool showAddButton, string url, string pagesCountUrl, string areaName, bool showEdit, bool ShowDelete, bool showPaging, bool initiateGrid, string gridObjectName, Agrin2GridPagingType pagingType, Dictionary<string, bool> showIfParameters, bool isCollapse)
        {
            var properties = typeof(TValue).GetProperties().ToArray();
            url = updateUrl(html, url, areaName);
            var countUrl = string.IsNullOrEmpty(pagesCountUrl) ? getPagesCountUrl(html, areaName) : pagesCountUrl;
            var deleteUrl = getDeleteUrl(html, areaName);
            string result = renderGrid(gridName, gridTitle, showSearchPanel, showAddButton, showEdit, ShowDelete, showPaging, initiateGrid, gridObjectName, typeof(TSearch), properties, url, countUrl, deleteUrl, isCollapse);
            return new HtmlString(result);
        }

        private static string renderGrid(string gridName, string gridTitle,bool showSearchPanel, bool showAddButton, bool showEdit, bool ShowDelete, bool showPaging, bool initiateGrid, string parentObjectName, Type searchType, PropertyInfo[] properties, string url, string getPagesCountUrl, string deleteUrl, bool isCollapse)
        {

            var result = "<div class='content d-flex flex-column flex-column-fluid' id="+ gridName+">";
            result += "<div class='d-flex flex-column-fluid'>";
            result += "<div class='container'>";
            result += "<div class='row'>";
            result += "<div class='col-xxl-8 order-2 order-xxl-1'>";
            result += "<div class='card card-custom card-stretch gutter-b'>";
            if (showSearchPanel)
            {
                result += renderSearchPanel(gridTitle, showAddButton);
            }

            result += "<div class='card-body pt-3 pb-0'>";

            result += "<div  class='table-responsive'>";
            result += "<table class='table table-head-custom table-vertical-center'>";
            result = renderHead(result, showEdit || ShowDelete, searchType, properties);
            result += "<tbody>";
            result += "</tbody>";
            result += "</table>";
            if (showPaging)
                result += renderPaging(gridName);
            result += "</div>";

            result += "</div>";


            result += "</div>";
            result += "</div>";
            result += "</div>";
            result += "</div>";
            result += "</div>";
            result += "</div>";
            result = renderScript(gridName, showEdit, ShowDelete, initiateGrid, parentObjectName, properties, url, getPagesCountUrl, deleteUrl, result, isCollapse);           
            return result;
        }
        private static string renderSearchPanel(string gridTitle,bool showAddButton)
        {
            var html = "<div class='card-header border-0 pt-5'>";
            html += "<h3 class='card-title align-items-start flex-column'>";
            html += "<span class='card-label font-weight-bolder text-dark'>"+ gridTitle+"</span>";
            html += "</h3>";
            html += "<div class='card-toolbar'>";
            html += "<div class='dataTables_filter mr-4'>";
            html += "<label class='mb-0'>Search:<input type='search' name='btnSearch' class='form-control form-control-sm' placeholder='' aria-controls='kt_datatable'></label>";
            html += "</div>";

            if (showAddButton)
            {
                html += "<a href='#' class='btn btn-primary font-weight-bolder' name='btnNewRecord'>";
                html += "New Record";
                html += "</a>";
            }
         

            html += "</div>";
            html += "</div>";
            return html;
        }
        private static string renderScript(string gridName, bool showEdit, bool ShowDelete, bool initiateGrid, string parentObjectName, PropertyInfo[] properties, string url, string getPagesCountUrl, string deleteUrl, string result, bool isCollapse)
        {
            result += "<script>";
            result += "\n" + "var elementsNumberPerPage = 10;";
            result += "\n" + "var " + gridName + ";";
            result += "\n" + "$(function(){";
            result += "\n" + gridName + "=new Agrin2.Components.Grid('" + gridName + "', '" + url + "', null,"+(isCollapse?"true":"false")+");";

            result += renderHeadDataSource(gridName, showEdit, ShowDelete, parentObjectName, properties);

            if (initiateGrid)
                result += "\n" + gridName + ".GetPagesCount('" + getPagesCountUrl + "');";
            result += "\n" + gridName + ".DeleteUrl='" + deleteUrl + "';";
            if (initiateGrid)
                result += "\n" + gridName + ".Initial();";
            result += "\n" + gridName + ".BindPaging();";
          
            result += "\n" + "});";
            result += "\n" + "</script>";
            return result;
        }

        private static string renderHead(string result, bool showOperation, Type searchType, PropertyInfo[] properties)
        {
            result += "<thead><tr class='text-left'>";
            if (properties != null)
            {
                result += "<th>" + Resources.Agrin2GridResource.RowIndex + "</th>";
                foreach (var item in properties)
                {
                    if (item.Name != "Id" && item.Name != "RowIndex" && item.Name != "EditCommand" && item.GetCustomAttribute<DoNotShow>() == null)
                    {
                        var width = item.GetCustomAttribute<Agrin2GridWidth>() != null ? item.GetCustomAttribute<Agrin2GridWidth>().Value : "";
                        var display = item.GetCustomAttribute<DisplayAttribute>() != null ? item.GetCustomAttribute<DisplayAttribute>().GetName() : item.Name;
                        var printClass = (item.GetCustomAttribute<Agrin2GridNoPrint>() != null) ? "class='d-print-none text-center'" : "class='text-center'";
                        if (item.PropertyType == typeof(bool) && item.GetCustomAttribute<UIHintAttribute>() == null && item.GetCustomAttribute<Agrin2GridBooleanToString>() == null)
                            result += "<th class='d-print-none text-center'>" + "<input type='checkbox' data-type='select-all' data-for='" + item.Name.Substring(0, 1).ToLower() + item.Name.Substring(1, item.Name.Length - 1) + "'/>" + "</th>";
                        else if (!string.IsNullOrEmpty(width))                        
                            
                            result += "<th "+ printClass+" style='width:" + width + "'>" + display + "</th>";                                              
                        else
                            result += "<th "+ printClass+">" + display + "</th>";
                    }
                }
                if (showOperation)
                    result += "<th class='d-print-none'></th>";
            }
            result += "</tr><thead>";
            return result;
        }
        private static string renderHeadDataSource(string gridName, bool showEdit, bool showDelete, string parentObjectName, PropertyInfo[] properties)
        {
            var result = "";
            if (properties != null)
            {
                result += "\n" + gridName + ".HeaderSource.push({Name:'ShowEdit',Value:'" + showEdit + "',ParentObjectName:'" + parentObjectName + "'})";
                result += "\n" + gridName + ".HeaderSource.push({Name:'ShowDelete',Value:'" + showDelete + "'})";
                foreach (var item in properties)
                {
                    var width = item.GetCustomAttribute<Agrin2GridWidth>() != null ? item.GetCustomAttribute<Agrin2GridWidth>().Value : "";
                    // var propertyName = item.GetCustomAttribute<Agrin2GridKey>() != null ? "Id" : item.Name;
                    var doNotShow = item.GetCustomAttribute<DoNotShow>() != null;
                    var isKey = item.GetCustomAttribute<Agrin2GridKey>() != null;
                    var showPrint = (item.GetCustomAttribute<Agrin2GridNoPrint>() == null)?"True":"False";
                    var propertyType = item.GetCustomAttribute<Agrin2GridBooleanToString>() == null ?
                        (item.GetCustomAttribute<UIHintAttribute>() != null ? "Custom" : item.PropertyType.Name) : "String";
                    result += "\n" + gridName + ".HeaderSource.push({Name:'" + item.Name.Substring(0, 1).ToLower() + (item.Name.Length >= 2 ? item.Name.Substring(1, item.Name.Length - 1) : "") + "',Width:'" + width +
                        "',Type:'" + propertyType + "',DoNotShow:'" + doNotShow + "',IsKey:'" + isKey + "',ShowPrint:'"+ showPrint + "'})";


                }
            }
            return result;

        }
        //public static string renderPaging(string gridName)
        //{
        //    var result = "<nav>";
        //    result += "<ul class='pagination border'>";
        //    result += "<li class='page-item'><a class='page-link border-right goToFirst' title='" + Resources.Agrin2GridResource.First + "' href='#'><i class='fa fa fa-angle-double-left'></i></a></li>";
        //    result += "<li class='page-item'><a class='page-link border-left border-right goToPrevious' title='" + Resources.Agrin2GridResource.Previous + "' href='#'><i class='fa fa-angle-left'></i></a></li>";
        //    result += "<li class='page-item'><input class='pageNumber border-left border-right page-link' type='text'/></li>";
        //    result += "<li class='page-item'><a class='page-link border-left border-right goToNext' title='" + Resources.Agrin2GridResource.Next + "' href='#'><i class='fa fa-angle-right'></i></a></li>";
        //    result += "<li class='page-item'><a class='page-link border-left border-right goToLast' title='" + Resources.Agrin2GridResource.Last + "' href='#'><i class='fa fa fa-angle-double-right'></i></a></li>";
        //    result += "<li class='page-item'><a class='page-link border-left' href='#' onclick='Agrin2.Print.OutterArea(\"#" + gridName + " table\")'><i class='fa fa-print'></i></a></li>";
        //    result += "</ul>";
        //    result += "</nav>";
        //    return result;
        //}

        public static string renderPaging(string gridName)
        {
            var result = "<div class='d-flex justify-content-between align-items-center flex-wrap pt-4 pb-3'>";
            result += "<div class='d-flex flex-wrap py-2 mr-3'>";
            result += "<a class='btn btn-icon btn-sm btn-light mr-2 my-1 goToFirst' title='" + Resources.Agrin2GridResource.First + "' href='#'><i class='ki ki-bold-double-arrow-back icon-xs'></i></a>";
            result += "<a class='btn btn-icon btn-sm btn-light mr-2 my-1 goToPrevious' title='" + Resources.Agrin2GridResource.Previous + "' href='#'><i class='ki ki-bold-arrow-back icon-xs'></i></a>";
            result += "<input class='pageing-numbers' type='text'/>";
            result += "<a class='btn btn-icon btn-sm btn-light mr-2 my-1 goToNext' title='" + Resources.Agrin2GridResource.Next + "' href='#'><i class='ki ki-bold-arrow-next icon-xs'></i></a>";
            result += "<a class='btn btn-icon btn-sm btn-light mr-2 my-1 goToLast' title='" + Resources.Agrin2GridResource.Last + "' href='#'><i class='ki ki-bold-double-arrow-next icon-xs'></i></a>";
            result += "<a class='page-link border-left' href='#' onclick='Agrin2.Print.OutterArea(\"#" + gridName + " table\")'><i class='fa fa-print'></i></a>";
            result += "</div>";
            result += "</div>";
            return result;
        }

        public enum Agrin2GridPagingType
        {
            ClientSide,
            ServerSide
        }
    }
}