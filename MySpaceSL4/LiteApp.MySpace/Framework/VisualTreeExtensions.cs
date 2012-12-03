using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace LiteApp.MySpace.Framework
{
    public static class VisualTreeExtensions
    {
        public static IEnumerable<DependencyObject>
               GetChildren(this DependencyObject depObject)
        {
            int count = depObject.GetChildrenCount();
            for (int i = 0; i < count; i++)
            {
                yield return VisualTreeHelper.GetChild(depObject, i);
            }
        }

        public static DependencyObject GetChild(
               this DependencyObject depObject, int childIndex)
        {
            return VisualTreeHelper.GetChild(depObject, childIndex);
        }

        public static DependencyObject GetChild(
               this DependencyObject depObject, string name)
        {
            return depObject.GetChild(name, false);
        }

        public static DependencyObject GetChild(this DependencyObject depObject,
                      string name, bool recursive)
        {
            foreach (var child in depObject.GetChildren())
            {
                var element = child as FrameworkElement;
                if (element != null)
                {
                    // if it's a FrameworkElement check Name
                    if (element.Name == name)
                        return element;

                    // try to get it using FindByName
                    var innerElement = element.FindName(name) as DependencyObject;
                    if (innerElement != null)
                        return innerElement;
                }
                // if it's recursive search through its children
                if (recursive)
                {
                    var innerChild = child.GetChild(name, true);
                    if (innerChild != null)
                        return innerChild;
                }
            }
            return null;
        }

        public static int GetChildrenCount(this DependencyObject depObject)
        {
            return VisualTreeHelper.GetChildrenCount(depObject);
        }

        public static DependencyObject GetParent(this DependencyObject depObject)
        {
            return VisualTreeHelper.GetParent(depObject);
        }
    }
}
