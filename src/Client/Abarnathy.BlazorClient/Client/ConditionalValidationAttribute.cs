using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Abarnathy.BlazorClient.Client
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class ConditionalValidationAttribute : Attribute, IPropertyValidationFilter
    {
        public string OtherProperty { get; set; }
        public object OtherValue { get; set; }

        public ConditionalValidationAttribute(string otherProperty, object otherValue)
        {
            OtherProperty = otherProperty;
            OtherValue = otherValue;
        }
        
        public bool ShouldValidateEntry(ValidationEntry entry, ValidationEntry parentEntry)
        {
            if (string.IsNullOrWhiteSpace(OtherProperty))
            {
                return true;
            }

            var prop =
                parentEntry
                    .Metadata
                    .Properties[OtherProperty]?
                    .PropertyGetter?
                    .Invoke(parentEntry.Model);

            return prop == OtherValue;
        }
    }
}