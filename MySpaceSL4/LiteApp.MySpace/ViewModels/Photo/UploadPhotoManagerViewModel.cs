using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Caliburn.Micro;
using LiteApp.MySpace.Assets.Strings;

namespace LiteApp.MySpace.ViewModels
{
    public class UploadPhotoManagerViewModel : Screen
    {
        Queue<UploadPhotoViewModel> _pendingTasks = new Queue<UploadPhotoViewModel>();
        BindableCollection<UploadPhotoViewModel> _archivedTasks = new BindableCollection<UploadPhotoViewModel>();
        bool _clearing;

        public UploadPhotoManagerViewModel()
        {
            DisplayName = AppStrings.UploadPhotoWindowTitle;
            MaximumActiveTasks = 1;
        }

        public int MaximumActiveTasks { get; set; }

        public AlbumViewModel Album { get; set; }

        public event EventHandler NoMoreTasks;

        public IEnumerable<UploadPhotoViewModel> ArchivedTasks
        {
            get { return _archivedTasks; }
        }

        public void StartUpload(IEnumerable<FileInfo> files)
        {
            foreach (var file in files)
            {
                var uploadPhotoViewModel = new UploadPhotoViewModel(file, Album);
                uploadPhotoViewModel.UploadCompleted += uploadPhotoViewModel_UploadCompleted;
                uploadPhotoViewModel.UploadCanceled += uploadPhotoViewModel_UploadCanceled;
                _pendingTasks.Enqueue(uploadPhotoViewModel);
                _archivedTasks.Add(uploadPhotoViewModel);
            }
            StartNextPendingTasks();
        }

        public void Clear()
        {
            _clearing = true;
            lock (_pendingTasks)
            {
                _pendingTasks.Clear();
            }
            _archivedTasks.Clear();
            _clearing = false;
        }

        public bool HasMoreTasks()
        {
            bool hasPendingTasks;
            lock (_pendingTasks)
            {
                hasPendingTasks = _pendingTasks.Any(x => x.Status != PhotoUploadStatus.Canceled);
            }
            return GetActiveTaskCount() > 0 || hasPendingTasks;
        }

        public bool HasArchivedTasks()
        {
            return _archivedTasks.Count > 0;
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

            StartNextPendingTasks();
        }

        void StartNextPendingTasks()
        {
            if (_clearing)
                return;
            int tasksToAdd = MaximumActiveTasks - GetActiveTaskCount();
            lock (_pendingTasks)
            {
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

            // Check and notify if there are more active or pending tasks
            if (!HasMoreTasks())
            {
                if (NoMoreTasks != null)
                    NoMoreTasks(this, EventArgs.Empty);
            }
        }

        int GetActiveTaskCount()
        {
            return _archivedTasks.Count(x => x.Status == PhotoUploadStatus.Uploading || x.Status == PhotoUploadStatus.Completing);
        }
    }
}
