
namespace LiteApp.MySpace.Web.Security
{
    public interface ICryptography
    {
        string Encrypt(string plainText);
        string Decrypt(string encrypted);
    }
}