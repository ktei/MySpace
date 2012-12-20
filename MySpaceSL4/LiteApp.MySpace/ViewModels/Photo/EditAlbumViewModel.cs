using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.Assets.Strings;
using LiteApp.Portable.Mvvm.Validation;
using LiteApp.MySpace.Services.Photo;

namespace LiteApp.MySpace.ViewModels
{
    public class EditAlbumViewModel : EditableViewModel
    {
        string _name;
        string _description;
        bool _isBusy;
        AlbumViewModel _target;

        public EditAlbumViewModel(AlbumViewModel target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            DisplayName = AppStrings.EditAlbumWindowTitle;
            RefreshBindingScope = new RefreshBindingScope();
            _name = target.Name;
            _description = target.Description;
        }

        public RefreshBindingScope RefreshBindingScope { get; set; }

        [RequiredField]
        [LengthConstraint(100)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    IsDirty = true;
                    NotifyOfPropertyChange(() => Name);
                }
            }
        }

        [LengthConstraint(150)]
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    IsDirty = true;
                    NotifyOfPropertyChange(() => Description);
                }
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    NotifyOfPropertyChange(() => IsBusy);
                }
            }
        }

        public void Save()
        {
            RefreshBindingScope.Scope();
            if (this.Validator.HasErrors)
                return;

            IsBusy = true;
            try
            {
                PhotoServiceClient svc = new PhotoServiceClient();
                svc.UpdateAlbumCompleted += (sender, e) =>
                    {
                        IsBusy = false;
                        if (e.Error != null)
                        {
                            e.Error.Handle();
                        }
                        else
                        {
                            _target.Name = Name;
                            _target.Description = Description;
                        }
                    };
            }
            catch
            {
                IsBusy = false;
            }
        }
    }
}
