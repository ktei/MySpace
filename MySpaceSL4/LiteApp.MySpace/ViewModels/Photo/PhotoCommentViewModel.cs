using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using LiteApp.MySpace.Security;
using System.ComponentModel;

namespace LiteApp.MySpace.ViewModels
{
    public class PhotoCommentViewModel : PropertyChangedBase
    {
        bool _isDeleting;
        DateTime _createdOn;

        public PhotoCommentViewModel()
        {
            SecurityContext.Current.PropertyChanged += SecurityContext_PropertyChanged;
        }

        void SecurityContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsAuthenticated")
            {
                NotifyOfPropertyChange(() => CreatedBy);
            }
        }

        public string Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set
            {
                if (_createdOn != value)
                {
                    _createdOn = value.ToLocalDateTime();
                }
            }
        }

        public string Contents { get; set; }

        public bool IsDeleting
        {
            get { return _isDeleting; }
            set
            {
                if (_isDeleting != value)
                {
                    _isDeleting = value;
                    NotifyOfPropertyChange(() => IsDeleting);
                }
            }
        }
    }
}
