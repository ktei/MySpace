using System.ComponentModel;
using Caliburn.Micro;
using LiteApp.Portable.Mvvm.Validation;

namespace LiteApp.MySpace.ViewModels
{
    public abstract class EditableViewModel : Screen, IDataErrorInfo
    {
        public EditableViewModel()
        {
            Validator = new BaseValidator();
        }

        bool _isDirty;
        IValidator _validator;

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    NotifyOfPropertyChange(() => IsDirty);
                }
            }
        }

        public IValidator Validator
        {
            get { return _validator; }
            set
            {
                _validator = value;
                _validator.Target = this;
            }
        }

        public virtual string Error
        {
            get { return null; }
        }

        public virtual string this[string propertyName]
        {
            get
            {
                if (!IsDirty)
                    return null;
                if (Validator == null)
                    return null;
                Validator.RemoveError(propertyName);
                var error = Validator.ValidateProperty(propertyName);
                if (!string.IsNullOrEmpty(error))
                    Validator.AddError(propertyName, error);
                return error;
            }
        }
    }
}
