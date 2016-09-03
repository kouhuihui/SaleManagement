using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dickson.Core.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举值的名称。
        /// </summary>
        /// <param name="value">枚举值。</param>
        public static string GetDisplayName(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }

        public static IList<KeyValuePair<int, string>> EnumToList(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("传入的参数必须是枚举类型！", "enumType");

            Dictionary<int, string> dictonary = new Dictionary<int, string>();
            Array enumValues = Enum.GetValues(enumType);
            foreach (Enum enumValue in enumValues)
            {
                int key = Convert.ToInt32(enumValue);
                string value = enumValue.GetDisplayName();
                dictonary.Add(key, value);
            }
            return dictonary.OrderByDescending(o=>o.Key).ToList();
        }
    }
}
