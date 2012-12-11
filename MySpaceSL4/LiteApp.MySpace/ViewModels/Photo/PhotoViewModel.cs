using System;
using Caliburn.Micro;
using System.Windows.Input;
using LiteApp.MySpace.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LiteApp.MySpace.ViewModels
{
    public class PhotoViewModel : Screen
    {
        static readonly string _defaultThumbURI = "../Assets/Images/photo.png";
        List<PhotoCommentViewModel> _comments;
        bool _isLoadingThumb = true;
        bool _isLoadingPhoto = true;

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

        public IEnumerable<PhotoCommentViewModel> Comments
        {
            get
            {
                if (_comments == null)
                {
                    _comments = new List<PhotoCommentViewModel>();
                    foreach (var i in Enumerable.Range(1, 100))
                    {
                        _comments.Add(new PhotoCommentViewModel() { Comment = "Comment " + i });
                    }
                }
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
    }
}
