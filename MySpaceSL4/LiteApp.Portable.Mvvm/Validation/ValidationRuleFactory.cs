using System;

namespace LiteApp.Portable.Mvvm.Validation
{
    public static class ValidationRuleFactory
    {
        static ValidationRuleFactory()
        {
            RuleLocator = new StandardRuleLocator();
        }

        public static IRuleLocator RuleLocator { get; set; }

        public static IValidationRule GetRule(ValidationAttribute attr)
        {
            if (RuleLocator == null)
                throw new NotSupportedException("Rule locator is needed. Set RuleLocator first.");
            return RuleLocator.FindRule(attr);
        }
    }
}
