
namespace LiteApp.MySpace.Framework.Validation
{
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
