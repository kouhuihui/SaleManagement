using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace Dickson.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FileContentLengthAttribute : DataTypeAttribute, IClientValidatable
    {
        public FileContentLengthAttribute()
            : base("upload")
        {
            ErrorMessage = "上传的文件不能大于{1}MB";
            MaxSize = 10 * 1024 * 1024;
        }

        public int MaxSize { get; set; }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, MaxSize / 1024 / 1024);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "fileupload",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            rule.ValidationParameters["maxsize"] = MaxSize;
            yield return rule;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            HttpPostedFileBase valueAsFileBase = value as HttpPostedFileBase;
            if (valueAsFileBase != null)
            {
                return ValidateExtension(valueAsFileBase.ContentLength);
            }

            return false;
        }

        bool ValidateExtension(int contentLength)
        {
            return contentLength > 0 && contentLength < MaxSize;
        }
    }
}
