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
using LiteApp.MySpace.Framework.Validation;
using LiteApp.MySpace.Services.Photo;

namespace LiteApp.MySpace.ViewModels
{
    public class CreateAlbumViewModel : EditableViewModel
    {
        string _name;
        string _description;
        bool _isBusy;

        public CreateAlbumViewModel()
        {
            DisplayName = "Create Album";
            RefreshBindingScope = new RefreshBindingScope();
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

        [LengthConstraint(2000)]
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

        public void Create()
        {
            RefreshBindingScope.Scope();
            if (this.Validator.HasErrors)
                return;
            IsBusy = true;
            try
            {
                PhotoServiceClient svc = new PhotoServiceClient();
                Album album = new Album();
                album.Name = Name;
                album.Description = Description;
                svc.SaveAlbumCompleted += (sender, e) =>
                    {
                        if (e.Error != null)
                        {
                            //TODO: show and log error
                        }
                        IsBusy = false;
                    };
                svc.SaveAlbumAsync(album);
            }
            catch
            {
                IsBusy = false;
            }
        }
    }
}
