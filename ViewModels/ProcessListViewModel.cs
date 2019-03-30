using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Lab5ParkhomenkoCSharp2019.Tools;
using Lab5ParkhomenkoCSharp2019.Tools.Managers;

namespace Lab5ParkhomenkoCSharp2019.ViewModels
{
    class ProcessListViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<ProcessList> _users;
        private DateTime? _birthDate;
        private string _name;
        private string _lastName;
        private string _email;
        private RelayCommand<object> _getAddUser;
        private RelayCommand<object> _getSaveChange;

        public ObservableCollection<ProcessList> Users
        {
            get => _users;
            private set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public DateTime? Date
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public ProcessListViewModel()
        {
            _users = new ObservableCollection<ProcessList>();
            BackgroundTaskProcess();
        }

        public RelayCommand<object> AddUser
        {
            get
            {
                return _getAddUser ?? (_getAddUser = new RelayCommand<object>(
                           AddUserInplementation, o => CanExecuteCommand()));
            }
        }

        public RelayCommand<object> SaveChange
        {
            get
            {
                return _getSaveChange ?? (_getSaveChange = new RelayCommand<object>(
                           AddUserInplementation));
            }
        }

        private bool CanExecuteCommand()
        {
            return !string.IsNullOrWhiteSpace(_birthDate.ToString()) &&
                   !string.IsNullOrWhiteSpace(_name) &&
                   !string.IsNullOrWhiteSpace(_lastName) &&
                   !string.IsNullOrWhiteSpace(_email);
        }
        private async void AddUserInplementation(object obj)
        {
            LoaderManager.Instance.ShowLoader();
            //await Task.Run(() => AddUserTask());
            LoaderManager.Instance.HideLoader();
        }

        

        private void BackgroundTaskProcess()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}