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

        public string AlbumId { get; set; }

        public IEnumerable<UploadPhotoViewModel> UploadItems
        {
            get { return _uploadItems; }
        }

        public void StartUpload(FileInfo file)
        {
            var uploadPhotoViewModel = new UploadPhotoViewModel();
            _uploadItems.Add(uploadPhotoViewModel);
            uploadPhotoViewModel.StartUpload(file, AlbumId);
        }
    }
}
