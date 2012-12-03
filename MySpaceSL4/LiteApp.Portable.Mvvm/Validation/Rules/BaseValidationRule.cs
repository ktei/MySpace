using System.Resources;

namespace LiteApp.Portable.Mvvm.Validation
{
    public abstract class BaseValidationRule : IValidationRule
    {
        public string Validate(ValidationAttribute attr, object value)
        {
            bool valid = DoValidate(attr, value);

            if (valid)
                return string.Empty;

            if (attr.ErrorMessageResourceName != null && attr.ErrorMessageResourceType != null)
            {
                ResourceManager rm = new ResourceManager(attr.ErrorMessageResourceType);
                return rm.GetString(attr.ErrorMessageResourceName);
            }

            return attr.DefaultErrorMessage;
        }

        protected virtual bool DoValidate(ValidationAttribute attr, object value)
        {
            return true;
        }
    }
}
