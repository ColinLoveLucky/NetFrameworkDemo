using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCDemo.Models
{
    public class DataAnnotationsValidationRunner
    {
        public static IEnumerable<ErrorInfo> GetErrors(object instance)
        {
            var metadataAttrib = instance.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), true).OfType<MetadataTypeAttribute>().FirstOrDefault();
            var buddyClassOrModelClass = metadataAttrib != null ? metadataAttrib.MetadataClassType : instance.GetType();
            var buddyClassProperties = TypeDescriptor.GetProperties(buddyClassOrModelClass).Cast<PropertyDescriptor>();
            var modelClassProperties = TypeDescriptor.GetProperties(instance.GetType()).Cast<PropertyDescriptor>();

            return from buddyProp in buddyClassProperties
                   join modelProp in modelClassProperties on buddyProp.Name equals modelProp.Name
                   from attribute in buddyProp.Attributes.OfType<ValidationAttribute>()
                   where !attribute.IsValid(modelProp.GetValue(instance))
                   select new ErrorInfo(buddyProp.Name, attribute.FormatErrorMessage(string.Empty), instance);
        }
    }
}