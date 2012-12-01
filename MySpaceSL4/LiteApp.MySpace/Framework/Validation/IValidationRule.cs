
namespace LiteApp.MySpace.Framework.Validation
{
    public interface IValidationRule
    {
        string Validate(ValidationAttribute attr, object value);
    }
}
