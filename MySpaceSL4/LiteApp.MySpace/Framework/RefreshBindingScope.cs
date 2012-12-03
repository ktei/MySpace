using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace LiteApp.MySpace.Framework
{
    /// <summary>
    /// Validation Scope
    /// </summary>
    /// <remarks>This implementation based on article http://www.thejoyofcode.com/Silverlight_Validation_and_MVVM_Part_II.aspx
    /// </remarks>
    public class RefreshBindingScope
    {
        /// <summary>
        /// List of UI Controls that we want to refresh and the default bound property
        /// </summary>
        private static readonly Dictionary<Type, DependencyProperty> BoundProperties =
            new Dictionary<Type, DependencyProperty>
            {
                { typeof(TextBox), TextBox.TextProperty },
                { typeof(ItemsControl), ItemsControl.ItemsSourceProperty },
                { typeof(ComboBox), ItemsControl.ItemsSourceProperty },
                { typeof(DataGrid), DataGrid.ItemsSourceProperty},
                { typeof(AutoCompleteBox), AutoCompleteBox.TextProperty},
                { typeof(DatePicker), DatePicker.SelectedDateProperty},
                { typeof(ListBox), ItemsControl.ItemsSourceProperty },
                { typeof(PasswordBox), PasswordBox.PasswordProperty },
            };

        /// <summary>
        /// Scope
        /// </summary>
        /// <remarks>This defines the controls included in the validation check</remarks>
        public FrameworkElement ScopeElement { get; private set; }

        /// <summary>
        /// GetScope
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static RefreshBindingScope GetScope(DependencyObject obj)
        {
            return (RefreshBindingScope)obj.GetValue(ScopeProperty);
        }

        /// <summary>
        /// SetScope
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetScope(DependencyObject obj, RefreshBindingScope value)
        {
            obj.SetValue(ScopeProperty, value);
        }

        /// <summary>
        /// Dependency property Scope
        /// </summary>
        public static readonly DependencyProperty ScopeProperty =
            DependencyProperty.RegisterAttached("Scope", typeof(RefreshBindingScope), typeof(RefreshBindingScope), new PropertyMetadata(null, ScopeChanged));

        /// <summary>
        /// Validation scope changed
        /// </summary>
        /// <param name="source">Source (must be a Framework Element)</param>
        /// <param name="args">Old and new scope</param>
        private static void ScopeChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            // clear old scope
            var oldScope = args.OldValue as RefreshBindingScope;
            if (oldScope != null)
            {
                oldScope.ScopeElement = null;
            }

            // assign new scope
            var scopeElement = source as FrameworkElement;
            if (scopeElement == null)
            {
                throw new ArgumentException(string.Format(
                    "'{0}' is not a valid type.Scope attached property can only be specified on types inheriting from FrameworkElement.",
                    source));
            }

            var newScope = (RefreshBindingScope)args.NewValue;
            newScope.ScopeElement = scopeElement;
        }

        /// <summary>
        /// Scope of elements to refresh binding
        /// </summary>
        public void Scope()
        {
            RefreshBinding(ScopeElement);
        }

        /// <summary>
        /// Recursively force each bound property to refresh it's binding
        /// </summary>
        /// <param name="dependencyObject">UI control</param>
        /// <remarks>This "walks" the visual tree to find controls that have a bound property
        /// and it then refreshes the binding, which forces any validation rule to be re-run.
        /// If the control does not have a bound property, the children controls are checked.
        /// This handles situations when text boxes or other validated controls are within
        /// an <c>ItemsControl</c>, <c>ListBox</c>, <c>DataGrid</c>, <c>Grid</c>
        /// <c>StackPanel</c>, or <c>Canvas</c>. Child controls for <c>Button</c>,
        /// <c>HyperlinkButton</c>, and <c>ValidationSummary</c> are ignored, since they shouldn't
        /// have bound children with validation rules.</remarks>
        private static void RefreshBinding(DependencyObject dependencyObject)
        {
            Debug.WriteLine(dependencyObject.GetType());

            // stop if we've reached a validation summary
            var validationSummary = dependencyObject as ValidationSummary;
            if (validationSummary != null) return;

            // don't do buttons - should be nothing to validate
            var button = dependencyObject as Button;
            if (button != null) return;

            // don't do hyperlink buttons - should be nothing to validate
            var hyperLinkButton = dependencyObject as HyperlinkButton;
            if (hyperLinkButton != null) return;

            foreach (var item in dependencyObject.GetChildren())
            {
                var found = false;

                // get bound property (use list from BindingHelper, so we don't repeat it in this class)
                DependencyProperty boundProperty;
                if (BoundProperties.TryGetValue(item.GetType(), out boundProperty))
                {
                    // get BindingExpression and, if exists, force it to refresh
                    var be = ((FrameworkElement)item).GetBindingExpression(boundProperty);
                    if (be != null) be.UpdateSource();

                    // binding refreshed, so don't look for children
                    found = true;

                    Debug.WriteLine(string.Format("{0} binding refreshed ({1}).", item, item.GetValue(boundProperty)));
                }

                // get children recursively if bound property has not already been found
                if (!found)
                {
                    RefreshBinding(item);
                }
            }
        }
    }
}
