using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using LiteApp.MySpace.Framework;

namespace LiteApp.MySpace.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IPage>, IShell
    {
        public ShellViewModel()
        {
            
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
        
        protected override void OnInitialize()
        {
            base.OnInitialize();
            ActivatePage("Home");
        }
    }
}
