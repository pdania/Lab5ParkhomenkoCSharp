using System.Windows;
using Lab4ParkhomenkoCSharp2019.Tools;
using Lab4ParkhomenkoCSharp2019.Tools.Managers;

namespace Lab4ParkhomenkoCSharp2019.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel, ILoaderOwner
    {
        #region Fields
        private Visibility _loaderVisibility = Visibility.Hidden;
        private bool _isControlEnabled = true;
        #endregion

        #region Properties
        public Visibility LoaderVisibility
        {
            get { return _loaderVisibility; }
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged();
            }
        }
        public bool IsControlEnabled
        {
            get { return _isControlEnabled; }
            set
            {
                _isControlEnabled = value;
                OnPropertyChanged();
            }
        } 
        #endregion

        public MainWindowViewModel()
        {
            LoaderManager.Instance.Initialize(this);
        }
    }
}
