using FSManager.FSExplorer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FSManager.FSExplorer
{
    public class FSModel : BindableBase
    {
        #region readonly
        private ObservableCollection<FSModel> _Children = new ObservableCollection<FSModel>();
        #endregion
        
        #region fields
        bool _isAccessible;
        string _fsType;
        DateTime _lastModified;
        string _Name;
        string _Path;
        #endregion

        #region properties
        public ObservableCollection<FSModel> Children { get => _Children; }

        public bool IsAccessible
        { 
            get => _isAccessible;
            private set { SetProperty(ref _isAccessible, value); }
        }

        public string fsType { get { return _fsType; } }

        public DateTime lastModified
        {
            get { return _lastModified; }
            private set { SetProperty(ref _lastModified, value); }
        }

        public string Name
        {
            get { return _Name; }
        }

        private void Name_SetValue(string value)
        {
            if (_Name != value)
                _Name = value;

            base.OnPropertyChanged(_Name);
        }

        public string Path
        {
            get { return _Path; }
        }

        private void Path_SetValue(string value)
        {
            if (_Path != value)
                _Path = value;

            base.OnPropertyChanged(_Path);
        }

        #endregion

        #region ctor
        public FSModel(string name, string path, string type, bool canAccess, DateTime lm)
        {
            Name_SetValue(name);
            Path_SetValue(path);
            _fsType = type;
            IsAccessible = canAccess;
            lastModified = lm;
        }
        #endregion

        #region methods
        internal void GetChildren()
        {
            if (fsType == "Server")
                return;

            _Children.Clear();

            bool isAccessible;
            int grandChildrenCount;

            DirectoryInfo di;
            IEnumerable<DirectoryInfo> dirs;
            IEnumerable<FileInfo> files;

            di = new DirectoryInfo(Path);

            try
            {
                dirs = di.EnumerateDirectories();
                files = di.EnumerateFiles();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException || ex is PathTooLongException || ex is UnauthorizedAccessException || ex is IOException)
                {
                    this.IsAccessible = false;
                    return;
                }
                throw;
            }

            foreach (var dir in dirs)
            {
                isAccessible = true;
                FSModel fm = new FSModel(dir.Name, Path + "\\" + dir.Name,"Folder",isAccessible, dir.LastWriteTime);
                _Children.Add(fm);

                try
                {
                    grandChildrenCount = 0;
                    grandChildrenCount = dir.EnumerateDirectories().Count();
                    grandChildrenCount += dir.EnumerateFiles().Count();

                    if (grandChildrenCount > 0)
                        fm.Children.Add(null);

                }
                catch (Exception ex)
                { 
                    if (ex is ArgumentException || ex is PathTooLongException || ex is IOException)
                    {
                        return;
                    }
                    else if (ex is UnauthorizedAccessException)
                    {
                        fm.IsAccessible = false;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            foreach (var file in files)
            {
                isAccessible = true;
                FSModel fm = new FSModel(file.Name, Path + file.Name,"File",isAccessible,file.LastWriteTimeUtc);
                _Children.Add(fm);
            }
        }

        #endregion
    }
}
