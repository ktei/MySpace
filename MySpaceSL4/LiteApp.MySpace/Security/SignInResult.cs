
namespace LiteApp.MySpace.Security
{
    public class SignInResult
    {
        public SignInResult(Identity user, bool success = true, string error = null)
        {
            User = user;
            Success = success;
            Error = error;
        }

        public bool Success { get; private set; }

        public Identity User { get; private set; }

        public string Error { get; private set; }
    }
}
