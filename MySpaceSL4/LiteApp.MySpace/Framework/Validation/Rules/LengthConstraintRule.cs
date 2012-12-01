using System;
using System.ComponentModel.Composition;

namespace LiteApp.MySpace.Framework.Validation
{
    [Export(typeof(IValidationRule))]
    [ValidationRuleMetadata(typeof(LengthConstraintAttribute))]
    public class LengthConstraintRule : BaseValidationRule
    {
        protected override bool DoValidate(ValidationAttribute attr, object value)
        {
            LengthConstraintAttribute attribute = attr as LengthConstraintAttribute;
            if (attribute == null)
                throw new ArgumentNullException("attr must be type of LengthConstraintAttribute");

            string input = value as string;

            bool valid = true;
            if (string.IsNullOrEmpty(input))
                return valid;

            valid = input.Length >= attribute.MinimumLength && input.Length <= attribute.MaximumLength;
            return valid;
        }
    }
}
