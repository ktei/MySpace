using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LiteApp.MySpace.Framework;


namespace LiteApp.MySpace.ViewModels
{
    [Export(typeof(IPage))]
    [PageMetadata("Photos")]
    public class PhotosViewModel : Screen, IPage
    {
        BindableCollection<UploadPhotoViewModel> _uploadItems = new BindableCollection<UploadPhotoViewModel>();

        public PhotosViewModel()
        {
            
        }

        public string Name
        {
            get { return "Photos"; }
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
