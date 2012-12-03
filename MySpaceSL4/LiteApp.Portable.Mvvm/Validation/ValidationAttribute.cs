using System;
using System.Resources;

namespace LiteApp.Portable.Mvvm.Validation
{
    public abstract class ValidationAttribute : Attribute
    {
        public abstract string DefaultErrorMessage { get; }
        public string ErrorMessageResourceName { get; set; }
        public Type ErrorMessageResourceType { get; set; }

        protected virtual string GetErrorMessageFromResource()
        {
            if (ErrorMessageResourceName == null || ErrorMessageResourceType == null)
                return null;
            ResourceManager rm = new ResourceManager(ErrorMessageResourceType);
            return rm.GetString(ErrorMessageResourceName);
        }
    }
}
