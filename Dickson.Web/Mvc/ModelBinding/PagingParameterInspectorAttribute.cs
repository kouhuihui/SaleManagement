using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dickson.Web.Mvc.ModelBinding
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PagingParameterInspectorAttribute : FilterAttribute,IActionFilter
    {
        public PagingParameterInspectorAttribute()
        {
            StartParameterName = "start";
            TakeParameterName = "take";
            DefaultTake = 10;
            MaxTake = 99;
            MaxStart = short.MaxValue;
        }

        public string StartParameterName { get; set; }

        public string TakeParameterName { get; set; }

        public static readonly int DefaultStart = 0;

        public int DefaultTake { get; set; }

        public int MaxTake { get; set; }

        public int MaxStart { get; set; }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int start;
            if (!TryGetIntValue(filterContext.ActionParameters, StartParameterName, out start) || start < 0 || start >= MaxStart)
            {
                start = DefaultStart;
            }
            int take;
            if (!TryGetIntValue(filterContext.ActionParameters, TakeParameterName, out take) || take <= 0 || take >= MaxTake)
            {
                take = DefaultTake;
            }
            filterContext.ActionParameters[StartParameterName] = start;
            filterContext.ActionParameters[TakeParameterName] = take;
        }

        bool TryGetIntValue(IDictionary<string, object> dict, string key, out int value)
        {
            object objValue;
            if (!dict.TryGetValue(key, out objValue))
            {
                value = 0;
                return false;
            }

            value = Convert.ToInt32(objValue);
            return true;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}
