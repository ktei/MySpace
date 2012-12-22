using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using LiteApp.MySpace.Assets.Strings;
using LiteApp.MySpace.Entities;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.Security;
using LiteApp.MySpace.Services.Photo;
using LiteApp.MySpace.ViewModels.Message;

namespace LiteApp.MySpace.ViewModels
{
    public class AlbumViewModel : Screen, IHandle<UpdateAlbumCoversMessage>
    {
        string _name;
        string _description;
        CoverViewModel[] _covers;
        ServerSidePagedCollectionView<PhotoViewModel> _photos;
        bool _isBusy;

        public AlbumViewModel()
        {
            SecurityContext.Current.PropertyChanged += SecurityContext_PropertyChanged;
            IoC.Get<IEventAggregator>().Subscribe(this);
        }

        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    NotifyOfPropertyChange(() => Description);
                }
            }
        }

        public string CreatedBy
        {
            get;
            set;
        }

        public CoverViewModel[] Covers
        {
            get { return _covers; }
            set
            {
                if (_covers != value)
                {
                    _covers = value;
                    NotifyOfPropertyChange(() => Covers);
                }
            }
        }

        public IEnumerable<PhotoViewModel> Photos
        {
            get { return _photos; }
        }

        public bool HasPhoto
        {
            get { return _photos != null && _photos.Count > 0; }
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

        public Visibility SensitiveButtonsVisibility
        {
            get
            {
                return (SecurityContext.Current.IsSuperAdminSignedIn() || SecurityContext.Current.IsUserSignedIn(CreatedBy)) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public void UploadPhoto()
        {
            var model = IoC.Get<UploadPhotoManagerViewModel>();
            model.Album = this;
            IoC.Get<IWindowManager>().ShowDialog(model);
        }

        public void DeleteSelectedPhotos()
        {
            if (IsBusy)
                return;
            if (_photos != null && _photos.Any(x => x.IsSelected))
            {
                MessageBoxViewModel message = new MessageBoxViewModel()
                {
                    Buttons = MessageBoxButtons.YesNo,
                    MessageLevel = MessageLevel.Exclamation,
                    Header = AppStrings.DeletePhotosMessageHeader,
                    Message = AppStrings.ConfirmDeleteItemsMessage,
                    DisplayName = AppStrings.ConfirmationWindowTitle
                };
                message.Closed += (messageSender, messageEventArgs) =>
                    {
                        if (message.Result != MessageBoxResult.Positive)
                            return;
                        IsBusy = true;
                        try
                        {
                            var parameters = _photos.Where(x => x.IsSelected).Select(x =>
                                new DeletePhotoParameters() { PhotoId = x.Id, FileName = Path.GetFileName(x.PhotoURI) }).ToList();
                            PhotoServiceClient svc = new PhotoServiceClient();
                            svc.DeletePhotosCompleted += (sender, e) =>
                                {
                                    IsBusy = false;
                                    if (e.Error != null)
                                    {
                                        e.Error.Handle();
                                    }
                                    else
                                    {
                                        _photos.RefreshCurrentPage();
                                        Covers = GetCovers(e.Result);
                                    }
                                };
                            svc.DeletePhotosAsync(parameters, Id);
                        }
                        catch
                        {
                            IsBusy = false;
                        }
                    };
                IoC.Get<IWindowManager>().ShowDialog(message);
            }
        }

        public void Handle(UpdateAlbumCoversMessage message)
        {
            if (message.AlbumId == Id)
            {
                this.Covers = GetCovers(message.NewCoverURIs);
                if (this.IsActive)
                {
                    _photos.RefreshCurrentPage();
                }
            }
        }

        public static CoverViewModel[] GetCovers(IList<string> uris)
        {
            if (uris == null)
                return new CoverViewModel[] { CoverViewModel.Default, CoverViewModel.Default, CoverViewModel.Default };
            CoverViewModel[] covers = new CoverViewModel[] { CoverViewModel.Default, CoverViewModel.Default, CoverViewModel.Default };
            for (int i = 0; i < Math.Min(3, uris.Count); ++i)
            {
                covers[i] = new CoverViewModel() { SourceURI = uris[i] };
            }
            return covers;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            LoadPhotos();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            if (_photos != null)
            {
                _photos.PageChanging -= _photos_PageChanging;
                _photos.PageChanged -= _photos_PageChanged;
                _photos = null;
            }
        }

        void LoadPhotos()
        {
            _photos = new ServerSidePagedCollectionView<PhotoViewModel>(new PhotoPagedDataSource(Id)) { PageSize = 20 };
            _photos.PageChanging += _photos_PageChanging;
            _photos.PageChanged += _photos_PageChanged;
            _photos.MoveToFirstPage();
        }

        void _photos_PageChanged(object sender, EventArgs e)
        {
            IsBusy = false;
            NotifyOfPropertyChange(() => HasPhoto);
        }

        void _photos_PageChanging(object sender, System.ComponentModel.PageChangingEventArgs e)
        {
            IsBusy = true;
        }

        void SecurityContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsAuthenticated")
            {
                NotifyOfPropertyChange(() => SensitiveButtonsVisibility);
            }
        }

        static CoverViewModel[] GetCovers(string combinedCoverURIs)
        {
            string[] splits = combinedCoverURIs.Split(";".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            CoverViewModel[] covers = new CoverViewModel[] { CoverViewModel.Default, CoverViewModel.Default, CoverViewModel.Default };
            for (int i = 0; i < Math.Min(3, splits.Length); ++i)
            {
                covers[i] = new CoverViewModel() { SourceURI = splits[i] };
            }
            return covers;
        }
    }
}
