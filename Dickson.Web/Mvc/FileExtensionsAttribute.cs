using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dickson.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FileExtensionsAttribute : DataTypeAttribute, IClientValidatable
    {
        string m_Extensions;

        public FileExtensionsAttribute()
            : base("upload")
        {
            ErrorMessage = "仅支持以下文件格式：{0}。";
        }

        public string Extensions
        {
            get { return string.IsNullOrWhiteSpace(m_Extensions) ? "png,jpg,jpeg" : m_Extensions; }
            set { m_Extensions = value; }
        }

        private string ExtensionsFormatted
        {
            get { return ExtensionsParsed.Aggregate((left, right) => left + ", " + right); }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "These strings are normalized to lowercase because they are presented to the user in lowercase format")]
        private string ExtensionsNormalized
        {
            get { return Extensions.Replace(" ", String.Empty).Replace(".", String.Empty).ToLowerInvariant(); }
        }

        private IEnumerable<string> ExtensionsParsed
        {
            get { return ExtensionsNormalized.Split(',').Select(e => "." + e); }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "extension",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            rule.ValidationParameters["extension"] = ExtensionsNormalized;
            yield return rule;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            HttpPostedFileBase valueAsFileBase = value as HttpPostedFileBase;
            if (valueAsFileBase != null)
            {
                return ValidateExtension(valueAsFileBase.FileName);
            }

            string valueAsString = value as string;
            if (valueAsString != null)
            {
                return ValidateExtension(valueAsString);
            }

            return false;
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "These strings are normalized to lowercase because they are presented to the user in lowercase format")]
        private bool ValidateExtension(string fileName)
        {
            try
            {
                return ExtensionsParsed.Contains(Path.GetExtension(fileName).ToLowerInvariant());
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
