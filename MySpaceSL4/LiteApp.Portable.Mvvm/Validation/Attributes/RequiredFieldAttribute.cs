
namespace LiteApp.Portable.Mvvm.Validation
{
    [RuleMapping(typeof(RequiredFieldRule))]
    public class RequiredFieldAttribute : ValidationAttribute
    {
        public RequiredFieldAttribute()
        {
            AllowWhiteSpace = true;
        }

        public override string DefaultErrorMessage
        {
            get
            {
                if (!AllowWhiteSpace)
                    return "This field is required and cannot only contain spaces.";
                return "This field is required";
                
            }
        }

        public bool AllowWhiteSpace { get; set; }
    }
}
