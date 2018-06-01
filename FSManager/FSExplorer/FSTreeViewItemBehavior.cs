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
    class FSTreeViewItemBehavior
    {
        #region Expanded
        public static bool GetFSExpanded(TreeViewItem treeViewItem)
        {
            return (bool)treeViewItem.GetValue(FSExpandedProperty);
        }

        public static void SetFSExpanded(TreeViewItem treeViewItem, bool value)
        {
            treeViewItem.SetValue(FSExpandedProperty, value);
        }

        public static readonly DependencyProperty FSExpandedProperty =
            DependencyProperty.RegisterAttached(
            "FSExpanded",
            typeof(bool),
            typeof(FSTreeViewItemBehavior),
            new UIPropertyMetadata(false, OnFSExpandedChanged));

        static void OnFSExpandedChanged(
          System.Windows.DependencyObject depObj, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem item = depObj as TreeViewItem;
            if (item == null)
                return;

            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
                item.Expanded += OnFSExpanded;
            else
                item.Expanded -= OnFSExpanded;
        }

        static void OnFSExpanded(object sender, RoutedEventArgs e)
        {
            // Only react to the Selected event raised by the TreeViewItem
            // whose IsSelected property was modified. Ignore all ancestors
            // who are merely reporting that a descendant's Selected fired.
            if (!Object.ReferenceEquals(sender, e.OriginalSource))
                return;

            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            if (tvi != null)
            {
                tvi.IsSelected = true;
                FSModel tviFSM = tvi.Header as FSModel;

                if (tviFSM != null)
                {
                    if (tviFSM.fsType == "Server" || tviFSM.fsType == "file")
                        return;

                    try
                    {
                        DateTime lastWriteTime = Directory.GetLastWriteTime(tviFSM.Path);
                        if (tviFSM.lastModified == lastWriteTime && tviFSM.Children[0] != null)
                            return;
                    }
                    catch (Exception ex)
                    {
                        if (ex is ArgumentException || ex is PathTooLongException || ex is UnauthorizedAccessException || ex is IOException)
                        {
                            return;
                        }
                        else
                        {
                            throw;
                        }
                    }

                    tviFSM.GetChildren();
                }
            }
        }
        #endregion
    }
}
