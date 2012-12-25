using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Caliburn.Micro;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.Services.Photo;
using LiteApp.MySpace.ViewModels.Message;
using LiteApp.MySpace.Assets.Strings;
using LiteApp.MySpace.Security;

namespace LiteApp.MySpace.ViewModels
{
    [Export(typeof(IPage))]
    [PageMetadata("Photos")]
    public class PhotosPageViewModel : Conductor<AlbumViewModel>, IPage, 
        IHandle<RequestAlbumsViewMessage>, 
        IHandle<SignedOutMessage>
    {
        ServerSidePagedCollectionView<AlbumViewModel> _albums;
        bool _isBusy;
        ViewState _state;

        public PhotosPageViewModel()
        {
            IoC.Get<IEventAggregator>().Subscribe(this);
        }

        public string Name
        {
            get { return "Photos"; }
        }

        public bool HasAlbum
        {
            get { return _albums != null && _albums.Count > 0; }
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

        public ViewState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    NotifyOfPropertyChange(() => State);
                }
            }
        }

        public IEnumerable<AlbumViewModel> Albums
        {
            get { return _albums; }
        }


        public ICommand DeleteAlbumCommand
        {
            get
            {
                return new Command(x =>
                {
                    //TODO: confirm user action
                    DeleteAlbum((string)x);
                });
            }
        }


        public void CreateAlbum()
        {
            var model = new CreateAlbumViewModel();
            model.CreateCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    _albums.RefreshCurrentPage();
                }
            };
            IoC.Get<IWindowManager>().ShowDialog(model);
        }

        public void ViewAlbum(AlbumViewModel album)
        {
            State = ViewState.Detail;
            ActivateItem(album);
        }

        public void EditAlbum(AlbumViewModel album)
        {
            EditAlbumViewModel model = new EditAlbumViewModel(album);
            IoC.Get<IWindowManager>().ShowDialog(model);
        }

        public void Handle(RequestAlbumsViewMessage message)
        {
            DeactivateItem(message.Requester, true);
            State = ViewState.Master;
            // This is a bit cheating: what we trying to
            // do is force to refresh covers because users
            // might have changed to album view before all covers were
            // loaded and if we don't do this, the covers will
            // always stay as IsLoading
            foreach (var a in _albums)
            {
                foreach (var cover in a.Covers)
                {
                    var sourceURI = cover.SourceURI;
                    cover.IsLoading = true;
                    cover.SourceURI = string.Empty;
                    cover.SourceURI = sourceURI;
                }
            }
        }

        public void Handle(SignedOutMessage message)
        {
            if (ActiveItem != null)
            {
                DeactivateItem(ActiveItem, true);
            }
            State = ViewState.Master;
            UnloadAlbums();
        }

        public void RefreshData()
        {
            if (_albums != null)
            {
                _albums.RefreshCurrentPage();
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            LoadAlbums();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            UnloadAlbums();
        }

        void LoadAlbums()
        {
            _albums = new ServerSidePagedCollectionView<AlbumViewModel>(new AlbumPagedDataSource());
            _albums.RefreshDataFailed += _albums_RefreshDataFailed;
            _albums.PageChanging += _albums_PageChanging;
            _albums.PageChanged += _albums_PageChanged;
            _albums.MoveToFirstPage();
        }

        void _albums_RefreshDataFailed(object sender, RefreshPagedDataFailedEventArgs e)
        {
            e.Error.Handle();
        }

        void _albums_PageChanged(object sender, System.EventArgs e)
        {
            IsBusy = false;
            NotifyOfPropertyChange(() => HasAlbum);
        }

        void _albums_PageChanging(object sender, System.ComponentModel.PageChangingEventArgs e)
        {
            IsBusy = true;
        }

        void DeleteAlbum(string id)
        {
            if (IsBusy)
                return;
            MessageBoxViewModel message = new MessageBoxViewModel()
            {
                Buttons = MessageBoxButtons.YesNo,
                MessageLevel = MessageLevel.Exclamation,
                Header = AppStrings.DeleteAlbumMessageHeader,
                Message = AppStrings.ConfirmDeleteAlbumMessage,
                DisplayName = AppStrings.ConfirmationWindowTitle
            };
            message.Closed += (messageSender, messageEventArgs) =>
                {
                    if (message.Result != MessageBoxResult.Positive)
                        return;

                    try
                    {
                        IsBusy = true;
                        PhotoServiceClient svc = new PhotoServiceClient();
                        svc.DeleteAlbumCompleted += (sender, e) =>
                            {
                                if (e.Error != null)
                                {
                                    e.Error.Handle();
                                }
                                else
                                {
                                    _albums.RefreshCurrentPage();
                                }
                            };
                        svc.DeleteAlbumAsync(id);
                    }
                    catch
                    {
                        IsBusy = false;
                    }
                };
            IoC.Get<IWindowManager>().ShowDialog(message);
        }

        void UnloadAlbums()
        {
            if (_albums != null)
            {
                _albums.PageChanging -= _albums_PageChanging;
                _albums.PageChanged -= _albums_PageChanged;
                _albums = null;
            }
        }
    }
}
