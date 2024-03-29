﻿using System;
using LiteApp.MySpace.Assets.Strings;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.Services.Photo;
using LiteApp.Portable.Mvvm.Validation;

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
            _target = target;
            _name = target.Name;
            _description = target.Description;
        }

        public RefreshBindingScope RefreshBindingScope { get; set; }

        public event EventHandler EditCompleted;

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
                            if (EditCompleted != null)
                                EditCompleted(this, EventArgs.Empty);
                        }
                    };
                svc.UpdateAlbumAsync(Name, Description, _target.Id);
            }
            catch
            {
                IsBusy = false;
            }
        }
    }
}
