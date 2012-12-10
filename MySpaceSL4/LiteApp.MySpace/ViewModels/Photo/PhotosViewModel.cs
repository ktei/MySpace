using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Caliburn.Micro;
using System.Linq;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.Services.Photo;

namespace LiteApp.MySpace.ViewModels
{
    [Export(typeof(IPage))]
    [PageMetadata("Photos")]
    public class PhotosViewModel : Conductor<AlbumViewModel>, IPage
    {
        ServerSidePagedCollectionView<AlbumViewModel> _albums;
        bool _isBusy;
        ViewState _state;

        protected override void OnInitialize()
        {
            LoadAlbums();
            base.OnInitialize();
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

        public void CreateAlbum()
        {
            var model = new CreateAlbumViewModel();
            model.CreateCompleted += (sender, e) =>
                {
                    _albums.RefreshCurrentPage();
                };
            IoC.Get<IWindowManager>().ShowDialog(model);
        }

        public void ViewAlbum(AlbumViewModel album)
        {
            State = ViewState.Detail;
            ActivateItem(album);
        }

        public void ViewAlbums()
        {
            State = ViewState.Master;
        }

        public ICommand UploadPhotoCommand
        {
            get
            {
                return new Command(x =>
                    {
                        UploadPhoto((string)x);
                    });
            }
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

        void LoadAlbums()
        {
            _albums = new ServerSidePagedCollectionView<AlbumViewModel>(new AlbumPagedDataSource());
            _albums.PageChanging += _albums_PageChanging;
            _albums.PageChanged += _albums_PageChanged;
            _albums.MoveToFirstPage();
        }

        void UploadPhoto(string albumId)
        {
            var model = new UploadPhotoManagerViewModel();
            model.Album = _albums.Single(x => x.Id == albumId);
            model.DisplayName = "Upload Photo ";
            IoC.Get<IWindowManager>().ShowDialog(model);
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
            try
            {
                IsBusy = true;
                PhotoServiceClient svc = new PhotoServiceClient();
                svc.DeleteAlbumCompleted += (sender, e) =>
                    {
                        if (e.Error != null)
                        {
                            //TODO: show and log error
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
        }
    }
}
