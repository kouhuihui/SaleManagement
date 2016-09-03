using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Dickson.Web.Mvc.ModelBinding
{
    public class CommaSeparatedModelBinder : DefaultModelBinder
    {
        static readonly MethodInfo _ToArrayMethod = typeof(Enumerable).GetMethod("ToArray");

        public CommaSeparatedModelBinder()
        { }

        public string ParameterName { get; set; }
        public CommaSeparatedModelBinder(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException("parameterName");

            ParameterName = parameterName;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return BindCore(bindingContext.ModelType, ParameterName ?? bindingContext.ModelName, bindingContext)
                    ?? base.BindModel(controllerContext, bindingContext);
        }

        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            return BindCore(propertyDescriptor.PropertyType, ParameterName ?? propertyDescriptor.Name, bindingContext)
                    ?? base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }

        object BindCore(Type type, string name, ModelBindingContext bindingContext)
        {
            if (type.GetInterface(typeof(IEnumerable).Name) != null)
            {
                var actualValue = bindingContext.ValueProvider.GetValue(name);
                if (actualValue != null)
                {
                    var valueType = type.GetElementType() ?? type.GetGenericArguments().FirstOrDefault();
                    if (valueType != null && valueType.GetInterface(typeof(IConvertible).Name) != null)
                    {
                        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(valueType));
                        foreach (var splitValue in actualValue.AttemptedValue.Split(new[] { ',' }))
                        {
                            if (!String.IsNullOrWhiteSpace(splitValue))
                                list.Add(Convert.ChangeType(splitValue, valueType));
                        }

                        if (type.IsArray)
                            return _ToArrayMethod.MakeGenericMethod(valueType).Invoke(this, new[] { list });
                        else
                            return list;
                    }
                }
            }

            return null;
        }
    }
}
