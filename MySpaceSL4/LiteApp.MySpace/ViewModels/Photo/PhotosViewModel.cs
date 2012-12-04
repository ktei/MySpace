using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LiteApp.MySpace.Framework;
//using LiteApp.MySpace.Services.Photo;
using System.Linq;
using System.Windows.Input;
using LiteApp.MySpace.Services.Photo;

namespace LiteApp.MySpace.ViewModels
{
    [Export(typeof(IPage))]
    [PageMetadata("Photos")]
    public class PhotosViewModel : Screen, IPage
    {
        BindableCollection<UploadPhotoViewModel> _uploadItems = new BindableCollection<UploadPhotoViewModel>();
        //BindableCollection<AlbumViewModel> _albums;
        ServerSidePagedCollectionView<AlbumViewModel> _albums;
        bool _isBusy;

        protected override void OnInitialize()
        {
            LoadAlbums();
            base.OnInitialize();
        }

        public string Name
        {
            get { return "Photos"; }
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

        public IEnumerable<AlbumViewModel> Albums
        {
            get { return _albums; }
        }

        public IEnumerable<UploadPhotoViewModel> UploadItems
        {
            get { return _uploadItems; }
        }

        public void UploadPhoto()
        {
            var uploadPhotoViewModel = new UploadPhotoViewModel();
            uploadPhotoViewModel.UploadStarted += uploadPhotoViewModel_UploadStarted;
            uploadPhotoViewModel.UploadCanceled += uploadPhotoViewModel_UploadCanceled;
            uploadPhotoViewModel.UploadCompleted += uploadPhotoViewModel_UploadCompleted;
            IoC.Get<IWindowManager>().ShowDialog(uploadPhotoViewModel);
        }

        public void CreateAlbum()
        {
            IoC.Get<IWindowManager>().ShowDialog(new CreateAlbumViewModel());
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
            _albums = new ServerSidePagedCollectionView<AlbumViewModel>(new AlbumPagedDataSource(), 2);
            _albums.PageChanging += _albums_PageChanging;
            _albums.PageChanged += _albums_PageChanged;
            _albums.MoveToFirstPage();
            //_albums.MoveToFirstPage();
            //_albums.DataReceived += _albums_DataReceived;
            //try
            //{
            //    IsBusy = true;
            //    PhotoServiceClient svc = new PhotoServiceClient();
            //    svc.GetAllAlbumsCompleted += (sender, e) =>
            //        {
            //            IsBusy = false;
            //            if (e.Error != null)
            //            {
            //                //TODO: show and log error
            //            }
            //            else
            //            {
            //                _albums = new BindableCollection<AlbumViewModel>(e.Result.Select(x => MapToAlbumViewModel(x)));
            //                NotifyOfPropertyChange(() => Albums);
            //            }
            //        };
            //    svc.GetAllAlbumsAsync();
            //}
            //catch
            //{
            //    IsBusy = false;
            //}
        }

        void _albums_PageChanged(object sender, System.EventArgs e)
        {
            IsBusy = false;
            NotifyOfPropertyChange(() => Albums);
        }

        void _albums_PageChanging(object sender, System.ComponentModel.PageChangingEventArgs e)
        {
            IsBusy = true;
        }

        void DeleteAlbum(string id)
        {
            //try
            //{
            //    IsBusy = true;
            //    PhotoServiceClient svc = new PhotoServiceClient();
            //    svc.DeleteAlbumCompleted += (sender, e) =>
            //        {
            //            if (e.Error != null)
            //            {
            //                //TODO: show and log error
            //            }
            //            else
            //            {
            //                LoadAlbums();
            //            }
            //        };
            //    svc.DeleteAlbumAsync(id);
            //}
            //catch
            //{
            //    IsBusy = false;
            //}
        }

        static int idx;
        //AlbumViewModel MapToAlbumViewModel(Album album)
        //{
        //    if (idx == 0)
        //    {
        //        idx++;
        //        return new AlbumViewModel()
        //        {
        //            Id = album.Id,
        //            Name = album.Name,
        //            Description = album.Description,
        //            CoverUri = "http://dl.dropbox.com/u/63866164/MySpace/Photos/me.png"
        //        };

        //    }
        //    return new AlbumViewModel()
        //        {
        //            Id = album.Id,
        //            Name = album.Name,
        //            Description = album.Description,
        //            CoverUri = "http://dl.dropbox.com/u/63866164/MySpace/Photos/thunder.png"
        //        };
            
        //}

        void uploadPhotoViewModel_UploadCompleted(object sender, System.EventArgs e)
        {
            var model = (UploadPhotoViewModel)sender;
            model.UploadCompleted -= uploadPhotoViewModel_UploadCompleted;
            _uploadItems.Remove(model);
        }

        void uploadPhotoViewModel_UploadCanceled(object sender, System.EventArgs e)
        {
            var model = (UploadPhotoViewModel)sender;
            model.UploadCanceled -= uploadPhotoViewModel_UploadCanceled;
            _uploadItems.Remove(model);
        }

        void uploadPhotoViewModel_UploadStarted(object sender, System.EventArgs e)
        {
            var model = (UploadPhotoViewModel)sender;
            _uploadItems.Add(model);
            model.UploadStarted -= uploadPhotoViewModel_UploadStarted;
        }
    }
}
