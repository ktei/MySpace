using System;
using System.ComponentModel.Composition;

namespace LiteApp.MySpace.Framework.Validation
{
    [Export(typeof(IValidationRule))]
    [ValidationRuleMetadata(typeof(RequiredFieldAttribute))]
    public class RequiredFieldRule : BaseValidationRule
    {
        protected override bool DoValidate(ValidationAttribute attr, object value)
        {
            RequiredFieldAttribute attribute = attr as RequiredFieldAttribute;
            if (attribute == null)
                throw new ArgumentNullException("attr must be type of RequiredFieldAttribute");
            string input = value as string;
            bool valid = true;
            if (attribute.DisallowWhitespace)
                valid = !string.IsNullOrWhiteSpace(input);
            else
                valid = !string.IsNullOrEmpty(input);
            return valid;
        }
        
    }
}
