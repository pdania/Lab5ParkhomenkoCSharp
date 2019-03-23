using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Lab4ParkhomenkoCSharp2019.Tools;
using Lab4ParkhomenkoCSharp2019.Tools.Managers;
using Lab4ParkhomenkoCSharp2019.Tools.Navigation;

namespace Lab4ParkhomenkoCSharp2019.ViewModels.Date
{
    internal class DateViewModel : BaseViewModel
    {
        #region Fields

        private DateTime? _birthDate;
        private string _name;
        private string _lastName;
        private string _email;

        private ObservableCollection<Person> _users;
        private Task _backgroundTask;
        #endregion

        #region Commands

        private RelayCommand<object> _getDateCommand;
        private RelayCommand<object> _getAddUser;

        #endregion

        #region Properties

        public DateTime? Date
        {
            get { return _birthDate; }
            set
            {
                _birthDate = (DateTime) value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Person> Users
        {
            get => _users;
            private set
            {
                _users = value;
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

        #region Commands

        public RelayCommand<object> AddUser
        {
            get
            {
                return _getAddUser ?? (_getAddUser = new RelayCommand<object>(
                           DateInplementation, o => CanExecuteCommand()));
            }
        }

        #endregion

        #endregion

        //public DateViewModel()
        //{
        //    _users = new ObservableCollection<Person>(StationManager.DataStorage.UsersList);
        //    BackgroundTaskProcess();
        //    StationManager.StopThreads += StopBackgroundTask;
        //}

        private void BackgroundTaskProcess()
        {
            int i = 0;
            while (i < 50)
            {
                var users = _users.ToList();
                users.Add(new Person("FirstName" + i, "LastName" + i, DateTime.Today, "email@ukr.net" + i));
                LoaderManager.Instance.ShowLoader();
                Users = new ObservableCollection<Person>(users);
                i++;
            }
        }

        internal void StopBackgroundTask()
        {
            _backgroundTask.Wait(2000);
            _backgroundTask.Dispose();
            _backgroundTask = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanExecuteCommand()
        {
            return !string.IsNullOrWhiteSpace(_birthDate.ToString()) &&
                   !string.IsNullOrWhiteSpace(_name) &&
                   !string.IsNullOrWhiteSpace(_lastName) &&
                   !string.IsNullOrWhiteSpace(_email);
        }

        private async void DateInplementation(object obj)
        {
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() => StartWork());
            LoaderManager.Instance.HideLoader();
        }

        private void StartWork()
        {
            Person user;
            Thread.Sleep(2000);
            
        }
    }
}