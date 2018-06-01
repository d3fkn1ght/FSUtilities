using System.Windows;
using System.Windows.Controls;

namespace FSManager.FSExplorer
{
    public class FSTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            FSModel fsm = item as FSModel;

            if (element != null && fsm != null)
            {
                if (fsm.fsType == "Server")
                    return element.FindResource("FSServerTemplate") as DataTemplate;
                if (fsm.fsType == "Drive")
                    return element.FindResource("FSDriveTemplate") as DataTemplate;
                if (fsm.fsType == "Folder")
                    return element.FindResource("FSFolderTemplate") as DataTemplate;
            }
            return element.FindResource("FSFileTemplate") as DataTemplate;
        }
    }
}