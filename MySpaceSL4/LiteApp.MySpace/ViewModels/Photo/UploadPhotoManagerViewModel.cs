using System.Collections.Generic;
using System.IO;
using Caliburn.Micro;
using LiteApp.MySpace.Assets.Strings;
using System.ComponentModel;
using System.Linq;

namespace LiteApp.MySpace.ViewModels
{
    public class UploadPhotoManagerViewModel : Screen
    {
        Queue<UploadPhotoViewModel> _pendingTasks = new Queue<UploadPhotoViewModel>();
        BindableCollection<UploadPhotoViewModel> _uploadItems = new BindableCollection<UploadPhotoViewModel>();
        bool _clearing;

        public UploadPhotoManagerViewModel()
        {
            DisplayName = AppStrings.UploadPhotoWindowTitle;
            MaximumActiveTasks = 1;
        }

        public int MaximumActiveTasks { get; set; }

        public AlbumViewModel Album { get; set; }

        public IEnumerable<UploadPhotoViewModel> UploadItems
        {
            get { return _uploadItems; }
        }

        public void StartUpload(IEnumerable<FileInfo> files)
        {
            foreach (var file in files)
            {
                var uploadPhotoViewModel = new UploadPhotoViewModel(file, Album.Id);
                uploadPhotoViewModel.UploadCompleted += uploadPhotoViewModel_UploadCompleted;
                uploadPhotoViewModel.UploadCanceled += uploadPhotoViewModel_UploadCanceled;
                _pendingTasks.Enqueue(uploadPhotoViewModel);
                _uploadItems.Add(uploadPhotoViewModel);
            }
            StartNextPendingTasks();
        }

        public void Clear()
        {
            _clearing = true;
            _pendingTasks.Clear();
            foreach (var item in _uploadItems)
            {
                item.CancelUpload();
            }
            _uploadItems.Clear();
            _clearing = false;
        }

        public bool HasMoreTasks()
        {
            lock (_pendingTasks)
            {
                return GetActiveTaskCount() > 0 || _pendingTasks.Any(x => x.Status != PhotoUploadStatus.Canceled);
            }
        }

        void uploadPhotoViewModel_UploadCanceled(object sender, System.EventArgs e)
        {
            var model = (UploadPhotoViewModel)sender;
            model.UploadCanceled -= uploadPhotoViewModel_UploadCanceled;
            model.UploadCompleted -= uploadPhotoViewModel_UploadCompleted;

            StartNextPendingTasks();
        }

        void uploadPhotoViewModel_UploadCompleted(object sender, PhotoUploadCompletedEventArgs e)
        {
            var model = (UploadPhotoViewModel)sender;
            model.UploadCanceled -= uploadPhotoViewModel_UploadCanceled;
            model.UploadCompleted -= uploadPhotoViewModel_UploadCompleted;

            if (e.Error == null)
            {
                Album.Covers = AlbumViewModel.GetCovers(e.NewCoverURIs);
                if (Album.IsActive)
                {
                    Album.RefreshPhotos();
                }
            }
            StartNextPendingTasks();
        }

        void StartNextPendingTasks()
        {
            if (_clearing)
                return;
            int tasksToAdd = MaximumActiveTasks - GetActiveTaskCount();
            while (_pendingTasks.Count > 0 && tasksToAdd > 0)
            {
                var nextTask = _pendingTasks.Dequeue();
                if (nextTask.Status != PhotoUploadStatus.Canceled)
                {
                    nextTask.StartUpload();
                    tasksToAdd--;
                }
            }
        }

        int GetActiveTaskCount()
        {
            return _uploadItems.Count(x => x.Status == PhotoUploadStatus.Uploading || x.Status == PhotoUploadStatus.Completing);
        }
    }
}
