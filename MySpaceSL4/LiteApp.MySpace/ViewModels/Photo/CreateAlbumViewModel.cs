﻿using System;
using LiteApp.MySpace.Assets.Strings;
using LiteApp.MySpace.Entities;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.Services.Photo;
using LiteApp.Portable.Mvvm.Validation;

namespace LiteApp.MySpace.ViewModels
{
    public class CreateAlbumViewModel : EditableViewModel
    {
        string _name;
        bool _isBusy;

        public CreateAlbumViewModel()
        {
            DisplayName = AppStrings.CreateAlbumWindowTitle;
            RefreshBindingScope = new RefreshBindingScope();
        }

        public RefreshBindingScope RefreshBindingScope { get; set; }

        public event EventHandler<AsyncOperationCompletedEventArgs> CreateCompleted;

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
                svc.CreateAlbumCompleted += (sender, e) =>
                {
                    IsBusy = false;
                    if (e.Error != null)
                    {
                        e.Error.Handle();
                    }
                    if (CreateCompleted != null)
                        CreateCompleted(this, new AsyncOperationCompletedEventArgs(e.Error));
                };
                svc.CreateAlbumAsync(album);
            }
            catch
            {
                IsBusy = false;
            }
        }
    }
}
