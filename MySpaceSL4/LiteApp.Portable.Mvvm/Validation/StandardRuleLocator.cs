using System;

namespace LiteApp.Portable.Mvvm.Validation
{
    public class StandardRuleLocator : IRuleLocator
    {
        public IValidationRule FindRule(ValidationAttribute attr)
        {
            foreach (var attribute in attr.GetType().GetCustomAttributes(true))
            {
                var ruleMapping = attribute as RuleMappingAttribute;
                if (ruleMapping != null)
                {
                    return (IValidationRule)Activator.CreateInstance(ruleMapping.RuleType);
                }
            }
            return null;
        }
    }
}
