using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security;
using System.Windows;

namespace FSManager.FSExplorer
{
    public class FSViewModel : BindableBase
    {
        #region fields
        private ObservableCollection<FSModel> _Children = new ObservableCollection<FSModel>();
        private FSModel _Selected;
        #endregion

        #region properties
        public ObservableCollection<FSModel> Children { get => _Children; }

        public FSModel Selected
        {
            get => _Selected;
            set { SetProperty(ref _Selected, value); }
        }

        public RelayCommand<FSModel> UpdateListBoxCommand { get; private set; }
        public RelayCommand<FSModel> UpdateTreeViewCommand { get; private set; }

        #endregion

        #region ctor
        public FSViewModel()
        {
            _Children.Add(new FSModel("My Computer", null , "Server", true, DateTime.Now));
            UpdateListBoxCommand = new RelayCommand<FSModel>(UpdateListBox, CanUpdateListBox);
            UpdateTreeViewCommand = new RelayCommand<FSModel>(UpdateTreeView, CanUpdateTreeView);
            GetDrives();
        }
        #endregion

        #region commands
        private bool CanUpdateListBox(object obj)
        {
            FSModel fsm = obj as FSModel;

            if (fsm == null)
                return false;

            return true;
        }

        private void UpdateListBox(object obj)
        {
            Selected = obj as FSModel;
        }

        private bool CanUpdateTreeView(object obj)
        {
            return true;
        }

        private void UpdateTreeView(object obj)
        {
            MessageBox.Show("Yes!");
        }
        #endregion

        void GetDrives()
        {
            string[] logicalDrives = new string[] { };
            try
            {
                logicalDrives = Directory.GetLogicalDrives();
            }
            catch (IOException)
            {

            }
            catch (UnauthorizedAccessException)
            {

            }

            foreach (string ld in logicalDrives)
            {
                bool IsAccessible = true;
                try
                {
                    // Make Async
                    DirectoryInfo di = new DirectoryInfo(ld);
                    FSModel dm = new FSModel(ld, ld, "Drive", IsAccessible, di.LastWriteTime);
                    Children[0].Children.Add(dm);
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentException || ex is PathTooLongException || ex is SecurityException || ex is IOException)
                    {
                        IsAccessible = false;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            foreach (var child in Children[0].Children)
            {
                if (child.IsAccessible)
                    child.GetChildren();
            }
        }
    }
}