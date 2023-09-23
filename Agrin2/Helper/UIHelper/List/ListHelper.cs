using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Agrin2.Helper.UIHelper.List
{
    public static class ListHelper
    {
        public static IEnumerable<SelectListItem> ConvertForDropdown<T>(this IEnumerable<T> items, bool showLabel = true, string value = "Id", string caption = "Title", string labelText = "-- Select --")
        {
            var result = items.Select(x => new SelectListItem { Text = x.GetType().GetProperty(caption).GetValue(x) != null ? x.GetType().GetProperty(caption).GetValue(x).ToString() : "", Value = x.GetType().GetProperty(value).GetValue(x) != null ?  x.GetType().GetProperty(value).GetValue(x).ToString() : "" });
            var tempList = result != null && result.Count() > 0 ? result.ToList() : new List<SelectListItem>();
            if (showLabel)
                tempList.Insert(0, new SelectListItem() { Value = "", Text = labelText });
            result = tempList.AsEnumerable();
            return result;
        }
        public static IEnumerable<SelectListItem> GetEmptyDropDown(bool withSelecteItems = false)
        {
            var emptyList = new List<SelectListItem>();
            if (withSelecteItems)
            {
                emptyList.Add(new SelectListItem() { Text = "-- Select --", Value = "" });
            }
            return emptyList.AsEnumerable();
        }
        public static IEnumerable<SelectListItem> GetEnumItems<TValue>(bool withSelecteItems = false)
        {
            var tmodelType = typeof(TValue);
            var values = Enum.GetValues(tmodelType);
            var result = new List<SelectListItem>();
            if (withSelecteItems)
            {
                result.Add(new SelectListItem() { Text = "-- Select --", Value = "" });
            }
            if (values != null)
            {
                foreach (var item in values)
                {
                    var display = (tmodelType.GetField(item.ToString()).GetCustomAttribute(typeof(DisplayAttribute), false)) as DisplayAttribute;
                    if (display != null)
                        if (display.ResourceType != null)
                            result.Add(new SelectListItem() { Value = item.ToString(), Text = lookupResource(display.ResourceType, display.Name) });
                        else
                            result.Add(new SelectListItem() { Value = item.ToString(), Text = display.Name });
                    else
                        result.Add(new SelectListItem() { Value = item.ToString(), Text = item.ToString() });
                }
            }
            return result;
        }
        public static IEnumerable<SelectListItem> GetEnumIntItems<TValue>(bool withSelectedItems = false)
        {
            var tmodelType = typeof(TValue);
            var values = Enum.GetValues(tmodelType);
            var result = new List<SelectListItem>();
            if (withSelectedItems)
            {
                result.Add(new SelectListItem() { Text = "-- Select --", Value = "" });
            }
            if (values != null)
            {
                foreach (var item in values)
                {
                    var value = (int)Enum.Parse(typeof(TValue), item.ToString());
                    var display = (tmodelType.GetField(item.ToString()).GetCustomAttribute(typeof(DisplayAttribute), false)) as DisplayAttribute;
                    if (display != null)
                        if(display.ResourceType!=null)
                          result.Add(new SelectListItem() { Value = value.ToString(), Text = lookupResource(display.ResourceType,display.Name) });
                       else
                         result.Add(new SelectListItem() { Value = value.ToString(), Text = display.Name });
                    else
                        result.Add(new SelectListItem() { Value = value.ToString(), Text = item.ToString() });
                }
            }
            return result;
        }

        private static string lookupResource(Type resourceManagerProvider, string resourceKey)
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
        public static IEnumerable<SelectListItem> GetEnumIntItems<TValue>(Array values)
        {
            var tmodelType = typeof(TValue);          
            var result = new List<SelectListItem>();
            if (values != null)
            {
                foreach (var item in values)
                {
                    var value = (int)Enum.Parse(typeof(TValue), item.ToString());
                    var display = (tmodelType.GetField(item.ToString()).GetCustomAttribute(typeof(DisplayAttribute), false)) as DisplayAttribute;
                    if (display != null)
                        if (display.ResourceType != null)
                            result.Add(new SelectListItem() { Value = value.ToString(), Text = lookupResource(display.ResourceType, display.Name) });
                        else
                            result.Add(new SelectListItem() { Value = value.ToString(), Text = display.Name });
                    else
                        result.Add(new SelectListItem() { Value = value.ToString(), Text = item.ToString() });
                }
            }
            return result;
        }
        public static IEnumerable<SelectListItem> GetEnumItems(Type tmodelType)
        {
            var values = Enum.GetValues(tmodelType);
            var result = new List<SelectListItem>();
            if (values != null)
            {
                foreach (var item in values)
                {
                    var display = (tmodelType.GetField(item.ToString()).GetCustomAttribute(typeof(DisplayAttribute), false)) as DisplayAttribute;
                    if (display != null)
                        result.Add(new SelectListItem() { Value =  item.ToString(), Text = display.Name });
                    else
                        result.Add(new SelectListItem() { Value = item.ToString(), Text = item.ToString() });
                }
            }
            return result;
        }
        public static IEnumerable<Enum> GetUniqueFlags(this Enum flags)
        {
            ulong flag = 1;
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
            {
                ulong bits = Convert.ToUInt64(value);
                while (flag < bits)
                {
                    flag <<= 1;
                }

                if (flag == bits && flags.HasFlag(value))
                {
                    yield return value;
                }
            }
        }
        public static string ReturnEnumFlagsDisplay(this Enum flags,object resource)
        {
            //var obj= Activator.CreateInstance(resourceType);
            var resourceType = resource.GetType();
            var flagsList = flags.GetUniqueFlags();
            var result = "";
            foreach(var item in flagsList)
            {
                result+= (string)resourceType.GetMethod("GetString",new Type[] { typeof(string)}).Invoke(resource, new string[] { item.ToString() })+",";
            }
            if (!string.IsNullOrEmpty(result))
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }
        public static TValue ToEnum<TValue>(this string value)
        {
            return (TValue)Enum.Parse(typeof(TValue), value, true);
        }  
        public static string GetDisplay(this Enum value)
        {
            return value.ToString();
        }

    }
}
