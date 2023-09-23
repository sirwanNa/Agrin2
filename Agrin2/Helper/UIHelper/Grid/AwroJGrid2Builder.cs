using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agrin2.Helper.UIHelper.Grid
{
    public class Agrin2JGrid2Builder<TSource, TDestination> : IActionResult
    {
        private List<TSource> _data;
        private List<TDestination> _destData;
        public Agrin2JGrid2Builder(List<TSource> data)
        {
            _data = data;
            _destData = new List<TDestination>();
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (_destData != null)
            {
                Mapper.Map(_data, _destData);
                int rowIndex = 1;
                foreach (var item in _destData)
                {
                    if (item.GetType().GetProperty("RowIndex") != null)
                        item.GetType().GetProperty("RowIndex").SetValue(item, rowIndex++);
                    if (item.GetType().GetProperty("IsDeleted") != null)
                        item.GetType().GetProperty("IsDeleted").SetValue(item, false);
                }
                var objectResult = new ObjectResult(_destData)
                {
                    StatusCode = StatusCodes.Status200OK
                };
                await objectResult.ExecuteResultAsync(context);
            }
        }
    }
    public class Agrin2JGrid2Builder<TDestination> : IActionResult
    {

        private List<TDestination> _destData;
        public Agrin2JGrid2Builder(List<TDestination> data)
        {
            _destData = data;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (_destData != null)
            {               
                int rowIndex = 1;
                foreach (var item in _destData)
                {
                    if (item.GetType().GetProperty("RowIndex") != null)
                        item.GetType().GetProperty("RowIndex").SetValue(item, rowIndex++);
                    if (item.GetType().GetProperty("IsDeleted") != null)
                        item.GetType().GetProperty("IsDeleted").SetValue(item, false);
                }
                var objectResult = new ObjectResult(_destData)
                {
                    StatusCode = StatusCodes.Status200OK
                };
                await objectResult.ExecuteResultAsync(context);
            }
        }
    }
}
