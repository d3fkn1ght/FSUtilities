using System;
using FSManager.FSExplorer;

namespace FSManager
{
    class MainWindowViewModel : BindableBase
    {
        private BindableBase _CurrentViewModel = new FSViewModel();

        public MainWindowViewModel()
        {
        }

        public BindableBase CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }
        }
    }
}
