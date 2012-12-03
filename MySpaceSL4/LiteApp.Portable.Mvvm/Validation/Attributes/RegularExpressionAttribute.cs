
namespace LiteApp.Portable.Mvvm.Validation
{
    [RuleMapping(typeof(RegularExpressionRule))]
    public class RegularExpressionAttribute : ValidationAttribute
    {
        public RegularExpressionAttribute(string pattern)
        {
            Pattern = pattern;
        }

        public override string DefaultErrorMessage
        {
            get { return "Pattern is wrong."; }
        }

        public string Pattern { get; private set; }
    }
}
