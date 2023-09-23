using Agrin2.Helper.UIHelper.Grid;
using Agrin2.Helper.UIHelper.Gird;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using static Agrin2.Helper.UIHelper.Gird.GridExtension;

namespace Agrin2.Helper.UIHelper
{
    public class Agrin2Framework<TModel>
    {
        private IHtmlHelper<TModel> _html;
        public Agrin2Framework(IHtmlHelper<TModel> html)
        {
            _html = html;
        }
        public HtmlString Grid<TValue>(string gridName,string gridTitle=null, bool showSearchPanel=true, bool showAddButton=true, string url = null, string pagesCountUrl=null, string areaName = null,bool showEdit = true, bool showDelete = true,bool showPaging=true,bool initiateGrid=true,string parentObjectName = null, Agrin2GridPagingType pagingType = Agrin2GridPagingType.ServerSide, Dictionary<string, bool> showIfParameters = null, bool isCollapse=false)
        {
            return GridExtension.Grid<TModel, TValue>(_html, gridName, gridTitle, showSearchPanel, showAddButton, url, pagesCountUrl, areaName,showEdit, showDelete, showPaging, initiateGrid, parentObjectName, pagingType, showIfParameters, isCollapse);
        }
        public HtmlString Grid<TValue, TSearch>(string gridName, string gridTitle = null, bool showSearchPanel = true, bool showAddButton = true, string url = null, string pagesCountUrl = null, string areaName = null,bool showEdit = true, bool showDelete = true, bool showPaging = true, bool initiateGrid = true,string parentObjectName = null, Agrin2GridPagingType pagingType = Agrin2GridPagingType.ServerSide, Dictionary<string, bool> showIfParameters = null, bool isCollapse = false)
        {
            return GridExtension.Grid<TModel, TValue>(_html, gridName, gridTitle, showSearchPanel, showAddButton, url, pagesCountUrl, areaName,showEdit, showDelete, showPaging, initiateGrid, parentObjectName, pagingType, showIfParameters, isCollapse);
        }
        public HtmlString JGrid2<TValue>(string gridName, string objectName=null,bool showPaging=true,bool showFooter=false,bool isReadOnlyGrid=false, Dictionary<string, bool> showIfParameters = null)
        {
            return Agrin2Component.JGrid2<TModel, TValue>(_html,gridName: gridName,objectName: objectName, showPaging:showPaging, showFooter: showFooter, isReadOnlyGrid: isReadOnlyGrid, showIfParameters: showIfParameters);
        }
    }
}