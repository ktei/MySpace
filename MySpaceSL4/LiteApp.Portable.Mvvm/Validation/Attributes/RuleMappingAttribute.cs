using System;

namespace LiteApp.Portable.Mvvm.Validation
{
    public class RuleMappingAttribute : Attribute
    {
        public RuleMappingAttribute(Type ruleType)
        {
            RuleType = ruleType;
        }

        public Type RuleType { get; private set; }
    }
}
