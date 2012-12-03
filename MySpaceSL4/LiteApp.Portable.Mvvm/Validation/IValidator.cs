
namespace LiteApp.Portable.Mvvm.Validation
{
    public interface IValidator
    {
        void AddError(string propertyName, string message);

        void RemoveError(string propertyName);

        string ValidateProperty(string propertyName);

        bool HasErrors { get; }

        object Target { get; set; }
    }
}
