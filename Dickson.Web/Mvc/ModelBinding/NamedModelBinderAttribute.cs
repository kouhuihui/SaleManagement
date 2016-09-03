using System;
using System.Web.Mvc;

namespace Dickson.Web.Mvc.ModelBinding
{
    public class NamedModelBinderAttribute : CustomModelBinderAttribute
    {
        Type m_ModelBinder;
        string m_ParameterName;

        public NamedModelBinderAttribute(Type modelBinder, string parameterName)
        {
            if (modelBinder == null)
                throw new ArgumentNullException("modelBinder");
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException("parameterName");

            m_ModelBinder = modelBinder;
            m_ParameterName = parameterName;
        }

        public override IModelBinder GetBinder()
        {
            return Activator.CreateInstance(m_ModelBinder, (object)m_ParameterName) as IModelBinder;
        }
    }
}
