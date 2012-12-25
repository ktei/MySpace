using Caliburn.Micro;
using LiteApp.MySpace.Assets.Strings;
using System;

namespace LiteApp.MySpace.ViewModels
{
    public class MessageBoxViewModel : Screen
    {
        string _header;
        string _message;
        MessageLevel _messageLevel;
        string _positiveButtonText;
        string _negativeButtonText;
        string _cancelButtonText;
        MessageBoxButtons _buttons;

        public MessageBoxViewModel()
        {
            DisplayName = AppStrings.ApplicationName;
        }

        public event EventHandler Closed;

        public string Header
        {
            get { return _header; }
            set
            {
                if (_header != value)
                {
                    _header = value;
                    NotifyOfPropertyChange(() => Header);
                }
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyOfPropertyChange(() => Message);
                }
            }
        }

        public MessageLevel MessageLevel
        {
            get { return _messageLevel; }
            set
            {
                if (_messageLevel != value)
                {
                    _messageLevel = value;
                    NotifyOfPropertyChange(() => MessageLevel);
                }
            }
        }

        public string PositiveButtonText
        {
            get { return _positiveButtonText; }
            set
            {
                if (_positiveButtonText != value)
                {
                    _positiveButtonText = value;
                    NotifyOfPropertyChange(() => PositiveButtonText);
                }
            }
        }

        public string NegativeButtonText
        {
            get { return _negativeButtonText; }
            set
            {
                if (_negativeButtonText != value)
                {
                    _negativeButtonText = value;
                    NotifyOfPropertyChange(() => NegativeButtonText);
                }
            }
        }

        public string CancelButtonText
        {
            get { return _cancelButtonText; }
            set
            {
                if (_cancelButtonText != value)
                {
                    _cancelButtonText = value;
                    NotifyOfPropertyChange(() => CancelButtonText);
                }
            }
        }

        public MessageBoxButtons Buttons
        {
            get { return _buttons; }
            set
            {
                if (_buttons != value)
                {
                    _buttons = value;
                    NotifyOfPropertyChange(() => Buttons);
                }
            }
        }

        public MessageBoxResult Result
        {
            get;
            set;
        }

        public void RaiseClosed()
        {
            if (Closed != null)
                Closed(this, EventArgs.Empty);
        }
    }

    public enum MessageLevel
    {
        Information = 0,
        Success,
        Exclamation,
        Question,
        Error
    }

    public enum MessageBoxButtons
    {
        OK = 0,
        YesNo,
        OKCancel,
        Custom
    }

    public enum MessageBoxResult
    {
        Positive,
        Negative,
        Cancel
    }
}
