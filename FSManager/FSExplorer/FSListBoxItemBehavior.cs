using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FSManager.FSExplorer
{
    class FSListBoxItemBehavior
    {
        #region Expanded
        public static bool GetFSDoubleClick(ListBoxItem item)
        {
            return (bool)item.GetValue(FSDoubleClickProperty);
        }

        public static void SetFSDoubleClick(ListBoxItem item, bool value)
        {
            item.SetValue(FSDoubleClickProperty, value);
        }

        public static readonly DependencyProperty FSDoubleClickProperty =
            DependencyProperty.RegisterAttached(
            "FSDoubleClick",
            typeof(bool),
            typeof(FSListBoxItemBehavior),
            new UIPropertyMetadata(false, OnFSDoubleClickChanged));

        static void OnFSDoubleClickChanged(
          System.Windows.DependencyObject depObj, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            ListBoxItem item = depObj as ListBoxItem;
            if (item == null)
                return;

            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
                item.MouseDoubleClick += OnFSDoubleClick;
            else
                item.MouseDoubleClick -= OnFSDoubleClick;
        }

        static void OnFSDoubleClick(object sender, RoutedEventArgs e)
        {
            // Only react to the Selected event raised by the TreeViewItem
            // whose IsSelected property was modified. Ignore all ancestors
            // who are merely reporting that a descendant's Selected fired.
            if (!Object.ReferenceEquals(sender, e.OriginalSource))
                return;

            ListBoxItem item = e.OriginalSource as ListBoxItem;
        }
        #endregion
    }
}
