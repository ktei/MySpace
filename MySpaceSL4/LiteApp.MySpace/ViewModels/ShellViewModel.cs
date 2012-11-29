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
using System.ComponentModel.Composition;
using LiteApp.MySpace.Framework;
using Caliburn.Micro;
using System.Linq;
using System.Collections.Generic;

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
        
        protected override void OnInitialize()
        {
            base.OnInitialize();
            ActivatePage("Home");
        }
    }
}
