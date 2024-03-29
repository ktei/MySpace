﻿using System;
using System.Text.RegularExpressions;

namespace LiteApp.Portable.Mvvm.Validation
{
    public class RegularExpressionRule : BaseValidationRule
    {
        protected override bool DoValidate(ValidationAttribute attr, object value)
        {
            RegularExpressionAttribute attribute = attr as RegularExpressionAttribute;
            if (attribute == null)
                throw new ArgumentNullException("attr must be type of RegularExpressionAttribute");
            Regex regex = new Regex(attribute.Pattern);
            var input = value as string;
            bool valid = true;
            if (!string.IsNullOrEmpty(input))
            {
                valid = regex.IsMatch(input);
            }
            return valid;
        }
    }
}
