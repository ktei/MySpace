using System;
using System.ComponentModel.Composition;

namespace LiteApp.MySpace.Framework.Validation
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ValidationRuleMetadataAttribute : ExportAttribute
    {
        public ValidationRuleMetadataAttribute(Type targetType)
        {
            this.TargetType = targetType;
        }

        public Type TargetType { get; private set; }
    }

    public interface IValidationRuleMetadata
    {
        Type TargetType { get; }
    }
}
