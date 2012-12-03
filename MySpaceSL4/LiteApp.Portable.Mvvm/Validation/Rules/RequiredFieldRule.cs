using System;

namespace LiteApp.Portable.Mvvm.Validation
{
    public class RequiredFieldRule : BaseValidationRule
    {
        protected override bool DoValidate(ValidationAttribute attr, object value)
        {
            RequiredFieldAttribute attribute = attr as RequiredFieldAttribute;
            if (attribute == null)
                throw new ArgumentNullException("attr must be type of RequiredFieldAttribute");
            string input = value as string;
            bool valid = true;
            if (!attribute.AllowWhiteSpace)
                valid = !string.IsNullOrEmpty(input) && input.Trim().Length > 0;
            else
                valid = !string.IsNullOrEmpty(input);
            return valid;
        }
    }
}
