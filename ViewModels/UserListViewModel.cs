using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Lab4ParkhomenkoCSharp2019.Tools;
using Lab4ParkhomenkoCSharp2019.Tools.DataStorage;
using Lab4ParkhomenkoCSharp2019.Tools.Managers;
using Lab4ParkhomenkoCSharp2019.ViewModels.Date;

namespace Lab4ParkhomenkoCSharp2019.ViewModels
{
    class UserListViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<Person> _users;
        private DateTime? _birthDate;
        private string _name;
        private string _lastName;
        private string _email;
        private RelayCommand<object> _getAddUser;
        private RelayCommand<object> _getSaveChange;

        public ObservableCollection<Person> Users
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

        public UserListViewModel()
        {
            _users = new ObservableCollection<Person>(StationManager.DataStorage.UsersList);
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
                           SaveChangeImplementation));
            }
        }

        private bool CanExecuteCommand()
        {
            return !string.IsNullOrWhiteSpace(_birthDate.ToString()) &&
                   !string.IsNullOrWhiteSpace(_name) &&
                   !string.IsNullOrWhiteSpace(_lastName) &&
                   !string.IsNullOrWhiteSpace(_email);
        }

        private void SaveChangeImplementation(object obj)
        {
            StationManager.DataStorage.SaveChanges();
        }
        private async void AddUserInplementation(object obj)
        {
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() => AddUserTask());
            LoaderManager.Instance.HideLoader();
        }

        private void AddUserTask()
        {
            if (!StationManager.DataStorage.UserExists(_email))
            {
                try
                {
                    var user = new Person(Name, LastName, Date, Email);
                    StationManager.DataStorage.AddUser(user);
                    StationManager.CurrentPerson = user;
                    _users = new ObservableCollection<Person>(StationManager.DataStorage.UsersList);
                }
                catch (PersonDiedException ex)
                {
                    MessageBox.Show(ex.Message + ex.Value);
                    
                }
                catch (PersonTooYoungException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (EmailException ex)
                {
                    MessageBox.Show(ex.Message + ex.Value);
                }
            }
            else
            {
                MessageBox.Show("User with such email already exist");
            }
        }

        private void BackgroundTaskProcess()
        {
            int i = 0;
            if (_users.ToList().Count == 0)
            {
                while (i < 50)
                {
                    var user = new Person("FirstName" + i, "LastName" + i, DateTime.Today, $"email{i}@ukr.net");
                    StationManager.DataStorage.AddUser(user);
                    StationManager.CurrentPerson = user;
                    i++;
                }

                _users = new ObservableCollection<Person>(StationManager.DataStorage.UsersList);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}