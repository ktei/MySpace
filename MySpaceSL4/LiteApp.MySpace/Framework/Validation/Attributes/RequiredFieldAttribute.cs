
namespace LiteApp.MySpace.Framework.Validation
{
    public class RequiredFieldAttribute : ValidationAttribute
    {
        public override string DefaultErrorMessage
        {
            get 
            {
                if (DisallowWhitespace)
                    return "This field is required and cannot only contain spaces.";
                return "This field is required";
            }
        }

        public bool DisallowWhitespace { get; set; }
    }
}
