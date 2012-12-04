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

namespace LiteApp.MySpace.Framework
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PageMetadataAttribute : ExportAttribute
    {
        public PageMetadataAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }

    public interface IPageMetadata
    {
        string Name { get; }
    }
}
