using AutoMapper;
using Agrin2.Helper.UIHelper.Attributes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Agrin2.Helper.UIHelper.Gird
{
    public class Agrin2GridBuilder<TSource, TDestination> : IActionResult
    {
        private List<TSource> _data;
        private List<TDestination> _destData;
        public Agrin2GridBuilder(List<TSource> data)
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
                }
                var objectResult = new ObjectResult(_destData)
                {
                    StatusCode = StatusCodes.Status200OK
                };
                await objectResult.ExecuteResultAsync(context);
            }
        }
    }
    public class TempUIHint<TDestination>
    {
        public Expression TempMethod<TValue>(MemberExpression member, ParameterExpression parameter)
        {
            return Expression.Lambda<Func<TDestination, TValue>>(member, parameter);
        }
    }
    public class Agrin2GridBuilder<TDestination> : IActionResult
    {

        private List<TDestination> _destData;
        private int _elementsCountPerPage;
        private int _pageNumber;
        public Agrin2GridBuilder(List<TDestination> data)
        {
            _destData = data;
        }
        public Agrin2GridBuilder(List<TDestination> data,int elementsCountPerPage, int pageNumber) 
        {
            _destData = data;
            _elementsCountPerPage = elementsCountPerPage;
            _pageNumber = pageNumber;
        }

        public async Task<List<object>> ConvertDataAsync(HttpContext context) 
        {
            var service = context.RequestServices.GetService(typeof(IActionContextAccessor)) as IActionContextAccessor; 
            var result = new List<object>();
            if (_destData != null&& service!=null)
            {
                int rowIndex = setRowIndex();               
                foreach (var item in _destData)
                {
                    var expandoObject = createColumns(service.ActionContext, item, rowIndex);
                    var obj = Agrin2TypeBuilder.CreateNewObject(await expandoObject);
                    result.Add(obj);
                    rowIndex++;
                }        
            }
            return result;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {          
            if (_destData != null)
            {
                int rowIndex = setRowIndex();
                var result = new List<object>();
                foreach (var item in _destData)
                {
                    var expandoObject = createColumns(context, item, rowIndex);
                    var obj = Agrin2TypeBuilder.CreateNewObject(await expandoObject);
                    result.Add(obj);
                    rowIndex++;
                }
                var objectResult = new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status200OK
                };
                await objectResult.ExecuteResultAsync(context);
            }

        }

        private int setRowIndex()
        {
            var rowIndex = 1;
            if (_elementsCountPerPage > 0 && _pageNumber > 0)
            {
                rowIndex = (_pageNumber - 1) * _elementsCountPerPage + 1;
            }

            return rowIndex;
        }

        private async Task<ExpandoObject> createColumns(ActionContext context,TDestination obj,int rowIndex)
        {
            var properties = typeof(TDestination).GetProperties();
            properties = setOrders(properties);
            dynamic result = new ExpandoObject();           
            foreach (var item in properties)
            {
                //if (item.Name != "Id")
                //{
                    if (item.Name == "RowIndex")                    
                        AddProperty(result, item.Name, rowIndex, item.PropertyType);                    
                    else if (item.PropertyType.IsEnum)                   
                        formatEnum(obj, result, item);                   
                    else if (item.GetCustomAttribute<UIHintAttribute>() != null)
                    {                       
                        //Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.
                        await AddUIHint(context,obj, result,item);
                    }
                    else if (item.PropertyType == typeof(DateTime)|| item.PropertyType == typeof(DateTime?))                    
                        formateDateTime(obj, result, item);
                    else if (item.GetCustomAttribute<Agrin2GridBooleanToString>() != null)
                        formateBoolean(obj, result, item);
                    else if (item.GetCustomAttribute<Agrin2GridCurrency>() != null)
                        formateCurrency(obj, result, item);
                    else if (item.PropertyType == typeof(string))
                        formateString(obj, result, item);
                    else                    
                        AddProperty(result, item.Name, item.GetValue(obj),item.PropertyType);
                    
                //}
            }
            return result;
        }
        private  PropertyInfo[] setOrders(PropertyInfo[] properties)
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

        private void formatEnum(TDestination obj, dynamic result, PropertyInfo item)
        {
            //  var values = item.PropertyType.GetEnumValues();

            // var display = item.GetValue(obj) != null && item.PropertyType.GetField(item.GetValue(obj).ToString()) != null ? (item.PropertyType.GetField(item.GetValue(obj).ToString()).GetCustomAttribute(typeof(DisplayAttribute), false)) as DisplayAttribute : null;

            var display = readDisplayValue(obj, item);

            if (!string.IsNullOrEmpty(display))
            {
                AddProperty(result, item.Name, display, typeof(string));
            }
            else if (item.GetValue(obj) != null)
            {
                AddProperty(result, item.Name, item.GetValue(obj).ToString(), typeof(string));
            }
            else
            {
                AddProperty(result, item.Name, item.Name, typeof(string));
            }
        }

        private string readDisplayValue(TDestination obj, PropertyInfo item)
        {
            var display = string.Empty;
            var flagsAttribute= item.PropertyType.GetCustomAttributes<FlagsAttribute>();   
            if (flagsAttribute.Any())
            {
                var valuesList = item.GetValue(obj).ToString().ToString();
                if (!string.IsNullOrEmpty(valuesList))
                {
                    var strArr = valuesList.Split(',');
                    if (strArr != null)
                    {
                        var displayArr = new List<string>();
                        foreach(var subStr in strArr)
                        {
                            var descriptionAttributes = (item.PropertyType.GetField(subStr.Trim()).GetCustomAttribute(typeof(DisplayAttribute), false)) as DisplayAttribute;
                            if (descriptionAttributes != null && !string.IsNullOrEmpty(descriptionAttributes.Name))
                                displayArr.Add(descriptionAttributes.Name);
                            else
                                displayArr.Add(subStr);
                        }
                        display = string.Join(",", displayArr);
                    }
                }

            }
            else
            {
                if (item != null && item.GetValue(obj) != null && item.PropertyType.GetField(item.GetValue(obj).ToString()) != null)
                {
                    var descriptionAttributes = (item.PropertyType.GetField(item.GetValue(obj).ToString()).GetCustomAttribute(typeof(DisplayAttribute), false)) as DisplayAttribute;
                    if (descriptionAttributes != null && descriptionAttributes.ResourceType != null)
                        display = lookupResource(descriptionAttributes.ResourceType, descriptionAttributes.Name);
                    else if (descriptionAttributes != null)
                        display = ((item.PropertyType.GetField(item.GetValue(obj).ToString()).GetCustomAttribute(typeof(DisplayAttribute), false)) as DisplayAttribute).Name;
                }
            }
          
            return display;
        }
   
        private  string lookupResource(Type resourceManagerProvider, string resourceKey)
        {
            foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
                {
                    System.Resources.ResourceManager resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
                    return resourceManager.GetString(resourceKey);
                }
            }

            return resourceKey; // Fallback with the key name
        }
        private void formateDateTime(TDestination obj, dynamic result, PropertyInfo item)
        {
            var value = string.Empty;
            if (item.GetValue(obj) != null)
            {              
                if (item.GetCustomAttribute<Agrin2GridDateTime>() != null)
                {
                    var date = ((DateTime)item.GetValue(obj));
                    var formatDate = !string.IsNullOrEmpty(item.GetCustomAttribute<Agrin2GridDateTime>().Format) ? item.GetCustomAttribute<Agrin2GridDateTime>().Format : "yyyy/MM/dd";
                    value = ((DateTime)item.GetValue(obj)).ToString(formatDate);
                    if (item.GetCustomAttribute<Agrin2GridDateTime>().ShowTime)
                    {
                        var time = date.ToString("HH:mm");
                        //var time = date.TimeOfDay.Hours + ":" + date.TimeOfDay.Minutes;
                        value += "&nbsp&nbsp" + time;
                    }                   
                }
                else
                    value = ((DateTime)item.GetValue(obj)).ToString("yyyy/MM/dd");
            }
            AddProperty(result, item.Name, value, typeof(string));
        }
        private void formateBoolean(TDestination obj, dynamic result, PropertyInfo item)
        {
            var value = string.Empty;
            if (item.GetValue(obj) != null)
            {
                var originValue = (bool)item.GetValue(obj);
                if (originValue)
                    value = Resources.Agrin2GridResource.CheckBox_True;
                else
                    value = Resources.Agrin2GridResource.CheckBox_False;
            }
            AddProperty(result, item.Name, value, typeof(string));
        }
        private void formateCurrency(TDestination obj, dynamic result, PropertyInfo item)
        {
            var value = string.Empty;
            if (item.GetValue(obj) != null)
                value = ((int)item.GetValue(obj)).ToString("C", CultureInfo.CurrentCulture);
           // value=string.Format("{0:#.000} $", Convert.ToDecimal(value));
            AddProperty(result, item.Name, value, typeof(string));
        }
        private void formateString(TDestination obj, dynamic result, PropertyInfo item) 
        {
            var value = string.Empty;
            if (item.GetValue(obj) != null)
                value = item.GetValue(obj).ToString();
            AddProperty(result, item.Name, value, typeof(string));
        }

        protected async Task AddUIHint(ActionContext context,TDestination obj,dynamic result, PropertyInfo item)
        {
            var razorViewEngine = (IRazorViewEngine)context.HttpContext.RequestServices.GetService(typeof(IRazorViewEngine));
            var tempDataProvider = (ITempDataProvider)context.HttpContext.RequestServices.GetService(typeof(ITempDataProvider));
            var serviceProvider = (IServiceProvider)context.HttpContext.RequestServices.GetService(typeof(IServiceProvider));
            var viewRender = new ViewRenderService(razorViewEngine, tempDataProvider, serviceProvider);
            string areaName = getAreaName(context);
            var viewPath = "DisplayTemplates/" + areaName + item.GetCustomAttribute<UIHintAttribute>().UIHint;
            var value = item.GetValue(obj);
            var renderedValue = await viewRender.RenderToStringAsync(viewPath, value);
            AddProperty(result, item.Name, renderedValue, typeof(string));
        }

        private static string getAreaName(ActionContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var areaName = descriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>() != null ?
                descriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>().RouteValue + "/" : "";
            return areaName;
        }

        protected void AddProperty(ExpandoObject expando, string propertyName, object propertyValue,Type propertyType)
        {
            // ExpandoObject supports IDictionary so we can extend it like this        
            var expandoDict = expando as IDictionary<string, object>;
            var valueWithType = propertyValue!=null? new ExpandoValue(){ Object= propertyValue,Type= propertyType }
            : new ExpandoValue() { Object = string.Empty, Type = typeof(string) };
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = valueWithType;
            else
                expandoDict.Add(propertyName, valueWithType);
        }   

    }
    public static class Agrin2TypeBuilder 
    {
        public static object CreateNewObject(ExpandoObject source)
        {
            var myType = CompileResultType(source);
            var obj = Activator.CreateInstance(myType);
            setValue(source, obj);
            return obj;
        }

        private static void setValue(ExpandoObject source, object obj)
        {
            foreach (var item in source)
            {
                if(!string.IsNullOrEmpty(item.Key) &&(item.Value as ExpandoValue) != null)
                {                                  
                    obj.GetType().GetProperty(item.Key).SetValue(obj, (item.Value as ExpandoValue).Object);
                }
                
            }
        }

        public static Type CompileResultType(ExpandoObject source)
        {
            TypeBuilder tb = GetTypeBuilder();
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            // NOTE: assuming your list contains Field objects with fields FieldName(string) and FieldType(Type)
            foreach (var field in source)
                CreateProperty(tb, field.Key, (field.Value as ExpandoValue).Type);

            Type objectType = tb.CreateTypeInfo();
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder()
        {
            var typeSignature = "MyDynamicType";
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
    public class ExpandoValue
    {
        public object Object { get; set; }
        public Type Type { get; set; }
    }
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }

    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public ViewRenderService(IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderToStringAsync(string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = _razorViewEngine.FindView(actionContext, viewName, true);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }
    }

}