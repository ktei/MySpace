using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.Security;
using LiteApp.MySpace.ViewModels.Message;
using LiteApp.MySpace.Assets.Strings;

namespace LiteApp.MySpace.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IPage>, IShell, IHandle<SignedOutMessage>
    {
        public ShellViewModel()
        {
            IoC.Get<IEventAggregator>().Subscribe(this);
        }

        [ImportMany]
        public IEnumerable<Lazy<IPage, IPageMetadata>> Pages { get; set; }

        public event EventHandler<PageActivatedEventArgs> PageActivatged;

        public void ActivatePage(string name)
        {
            var page = Pages.FirstOrDefault(x => x.Metadata.Name == name);
            if (page != null)
            {
                ActivateItem(page.Value);
                if (PageActivatged != null)
                    PageActivatged(this, new PageActivatedEventArgs(name));
            }
        }

        public void SignIn()
        {
            IoC.Get<IWindowManager>().ShowDialog(new SignInViewModel());
        }

        public void SignOut()
        {
            var uploadPhotoManager = IoC.Get<UploadPhotoManagerViewModel>();
            if (uploadPhotoManager.HasMoreTasks())
            {
                MessageBoxViewModel message = new MessageBoxViewModel();
                message.DisplayName = AppStrings.SignOutWindowTitle;
                message.Header = AppStrings.UnfinishedPhotoUploadsMessageHeader;
                message.Message = AppStrings.UnfinishedPhotoUploadsMessage;
                message.Buttons = MessageBoxButtons.YesNo;
                message.MessageLevel = MessageLevel.Question;
                message.Closed += (mSender, mEventArgs) =>
                {
                    if (message.Result == MessageBoxResult.Positive)
                    {
                        uploadPhotoManager.Clear();
                        SecurityContext.Current.SignOut();
                    }
                };
                IoC.Get<IWindowManager>().ShowDialog(message);
            }
            else
            {
                SecurityContext.Current.SignOut();
            }
        }

        public void Handle(SignedOutMessage message)
        {
            ActivatePage("Home");
        }
        
        protected override void OnInitialize()
        {
            base.OnInitialize();
            ActivatePage("Home");
        }
    }
}
