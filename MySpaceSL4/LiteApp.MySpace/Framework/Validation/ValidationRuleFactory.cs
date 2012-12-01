using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace LiteApp.MySpace.Framework.Validation
{
    public class ValidationRuleFactory
    {
        static ValidationRuleFactory _instance;

        public ValidationRuleFactory()
        {
            CompositionInitializer.SatisfyImports(this);
        }

        [ImportMany]
        public IEnumerable<Lazy<IValidationRule, IValidationRuleMetadata>> Rules { get; set; }

        public static IValidationRule GetRule(ValidationAttribute attr)
        {
            if (_instance == null)
                _instance = new ValidationRuleFactory();
            return _instance.FindRule(attr.GetType());
        }

        public IValidationRule FindRule(Type type)
        {
            return Rules.Where(x => x.Metadata.TargetType == type).Select(x => x.Value).FirstOrDefault();
        }
    }
}
