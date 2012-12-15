using System;
using Caliburn.Micro;
using System.Windows.Input;
using LiteApp.MySpace.Framework;
using System.Collections.Generic;
using System.Linq;
using LiteApp.MySpace.Services.Photo;
using LiteApp.MySpace.Security;

namespace LiteApp.MySpace.ViewModels
{
    public class PhotoViewModel : Screen
    {
        static readonly string _defaultThumbURI = "../Assets/Images/photo.png";
        BindableCollection<PhotoCommentViewModel> _comments;
        bool _isLoadingThumb = true;
        bool _isLoadingPhoto = true;
        bool _isLoadingComments;
        bool _canPostComment = true;

        public PhotoViewModel()
        {
            DisplayName = "View Photo";
        }

        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Description { get; set; }

        public string PhotoURI { get; set; }

        public string ThumbURI { get; set; }

        public bool IsLoadingThumb
        {
            get { return _isLoadingThumb; }
            set
            {
                if (_isLoadingThumb != value)
                {
                    _isLoadingThumb = value;
                    NotifyOfPropertyChange(() => IsLoadingThumb);
                }
            }
        }

        public bool IsLoadingPhoto
        {
            get { return _isLoadingPhoto; }
            set
            {
                if (_isLoadingPhoto != value)
                {
                    _isLoadingPhoto = value;
                    NotifyOfPropertyChange(() => IsLoadingPhoto);
                }
            }
        }

        public bool IsLoadingComments
        {
            get { return _isLoadingComments; }
            private set
            {
                if (_isLoadingComments != value)
                {
                    _isLoadingComments = value;
                    NotifyOfPropertyChange(() => IsLoadingComments);
                }
            }
        }

        public bool CanPostComment
        {
            get { return _canPostComment; }
            private set
            {
                if (_canPostComment != value)
                {
                    _canPostComment = value;
                    NotifyOfPropertyChange(() => CanPostComment);
                }
            }
        }

        public bool HasComment
        {
            get { return _comments != null && _comments.Count > 0; }
        }

        public IEnumerable<PhotoCommentViewModel> Comments
        {
            get
            {
                return _comments;
            }
        }


        public ICommand DoubleClickCommand
        {
            get
            {
                return new Command(x =>
                    {
                        IoC.Get<IWindowManager>().ShowDialog(this);
                    });
            }
        }

        public static string DefaultThumbURI
        {
            get { return _defaultThumbURI; }
        }

        public ICommand PostCommentCommand
        {
            get
            {
                return new Command(x =>
                    {
                        string contents = Convert.ToString(x);
                        PostComment(contents);
                    });
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            LoadComments();
        }

        void PostComment(string contents)
        {
            if (IsLoadingComments)
                return;
            if (!SecurityContext.Current.IsAuthenticated)
            {
                SignInViewModel signInModel = new SignInViewModel() { Message = "This operation needs you to sign in first." };
                IoC.Get<IWindowManager>().ShowDialog(signInModel);
                return;
            }
            CanPostComment = false;
            PhotoComment comment = new PhotoComment();
            comment.Contents = contents;
            comment.CreatedBy = SecurityContext.Current.User.Name;
            comment.PhotoId = Id;
            PhotoServiceClient svc = new PhotoServiceClient();
            try
            {
                svc.SaveCommentCompleted += (sender, e) =>
                    {
                        if (e.Error != null)
                        {
                            // TODO: log error
                        }
                        else
                        {
                            if (_comments != null)
                            {
                                _comments.Insert(0, MapToPhotoCommentViewModel(e.Result));
                            }
                        }
                        CanPostComment = true;
                        NotifyOfPropertyChange(() => HasComment);
                    };
                svc.SaveCommentAsync(comment);
            }
            catch
            {
                CanPostComment = true;
            }
        }

        void LoadComments()
        {
            IsLoadingComments = true;
            try
            {
                PhotoServiceClient svc = new PhotoServiceClient();
                svc.GetCommentsCompleted += (sender, e) =>
                    {
                        if (e.Error != null)
                        {
                            //TODO: show and log error
                        }
                        else
                        {
                            _comments = new BindableCollection<PhotoCommentViewModel>(e.Result.Select(x => MapToPhotoCommentViewModel(x)));
                            NotifyOfPropertyChange(() => Comments);
                            NotifyOfPropertyChange(() => HasComment);
                        }
                        IsLoadingComments = false;
                    };
                svc.GetCommentsAsync(Id);
            }
            catch
            {
                IsLoadingComments = false;
            }
        }

        PhotoCommentViewModel MapToPhotoCommentViewModel(PhotoComment photoComment)
        {
            return new PhotoCommentViewModel()
            {
                Id = photoComment.Id,
                CreatedBy = photoComment.CreatedBy,
                CreatedOn = photoComment.CreatedOn,
                Contents = photoComment.Contents
            };
        }
    }
}
