
namespace LiteApp.MySpace.Security
{
    public class ActivationResult
    {
        public ActivationResult(string userName, bool success = true, string error = null)
        {
            UserName = userName;
            Success = success;
            Error = error;
        }

        public string UserName { get; private set; }

        public bool Success { get; private set; }

        public string Error { get; private set; }
    }
}
