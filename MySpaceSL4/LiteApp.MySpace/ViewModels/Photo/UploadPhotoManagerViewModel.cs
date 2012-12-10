using System.Collections.Generic;
using System.IO;
using Caliburn.Micro;

namespace LiteApp.MySpace.ViewModels
{
    public class UploadPhotoManagerViewModel : Screen
    {
        BindableCollection<UploadPhotoViewModel> _uploadItems = new BindableCollection<UploadPhotoViewModel>();

        public UploadPhotoManagerViewModel()
        {
            DisplayName = "Upload Photo";
        }

        public AlbumViewModel Album { get; set; }

        public IEnumerable<UploadPhotoViewModel> UploadItems
        {
            get { return _uploadItems; }
        }

        public void StartUpload(FileInfo file)
        {
            var uploadPhotoViewModel = new UploadPhotoViewModel();
            uploadPhotoViewModel.UploadCompleted += uploadPhotoViewModel_UploadCompleted;
            _uploadItems.Add(uploadPhotoViewModel);
            uploadPhotoViewModel.StartUpload(file, Album.Id);
        }

        void uploadPhotoViewModel_UploadCompleted(object sender, PhotoUploadCompletedEventArgs e)
        {
            ((UploadPhotoViewModel)sender).UploadCompleted -= uploadPhotoViewModel_UploadCompleted;
            if (e.Error == null)
            {
                Album.Covers = AlbumViewModel.GetCovers(e.NewCoverURIs);
                if (Album.IsActive)
                {
                    Album.RefreshPhotos();
                }
            }
        }
    }
}
