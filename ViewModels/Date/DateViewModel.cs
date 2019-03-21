using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Lab4ParkhomenkoCSharp2019.Tools;
using Lab4ParkhomenkoCSharp2019.Tools.Managers;

namespace Lab4ParkhomenkoCSharp2019.ViewModels.Date
{
    internal class DateViewModel : BaseViewModel
    {
        #region Fields

        private DateTime? _birthDate;
        private string _isAdult;
        private string _westZodiac;
        private string _chineseZodiac;
        private string _name;
        private string _lastName;
        private string _email;
        private string _isBirthday;
        private string _nameToSet;
        private string _lastNameToSet;
        private string _emailToSet;
        private string _birthDateToSet;

        #endregion

        #region Commands

        private RelayCommand<object> _getDateCommand;

        #endregion

        #region Properties

        public string IsAdult
        {
            get { return _isAdult; }
            set
            {
                _isAdult = value;
                OnPropertyChanged();
            }
        }

        public DateTime? Date
        {
            get { return _birthDate; }
            set
            {
                _birthDate = (DateTime) value;
                OnPropertyChanged();
            }
        }

        public string WestZodiac
        {
            get { return _westZodiac; }
            set
            {
                _westZodiac = value;
                OnPropertyChanged();
            }
        }

        public string ChineseZodiac
        {
            get { return _chineseZodiac; }
            set
            {
                _chineseZodiac = value;
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

        public string NameToSet
        {
            get { return _nameToSet; }
            private set
            {
                _nameToSet = value;
                OnPropertyChanged();
            }
        }

        public string LastNameToSet
        {
            get { return _lastNameToSet; }
            private set
            {
                _lastNameToSet = value;
                OnPropertyChanged();
            }
        }

        public string EmailToSet
        {
            get { return _emailToSet; }
            private set
            {
                _emailToSet = value;
                OnPropertyChanged();
            }
        }

        public string BirthDateToSet
        {
            get { return _birthDateToSet; }
            private set
            {
                _birthDateToSet = value;
                OnPropertyChanged();
            }
        }

        public string IsBirthday
        {
            get { return _isBirthday; }
            private set
            {
                _isBirthday = value;
                OnPropertyChanged();
            }
        }

        #region Commands

        public RelayCommand<object> GetDate
        {
            get
            {
                return _getDateCommand ?? (_getDateCommand = new RelayCommand<object>(
                           DateInplementation, o => CanExecuteCommand()));
            }
        }

        #endregion

        #endregion

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
            try
            {
                user = new Person(_name, _lastName, _birthDate, _email);
                IsAdult = "IsAdult? " + (user.IsAdult() ? "Yes" : "No");
                NameToSet = $"Name: {user.Name}";
                LastNameToSet = $"Last name: {user.LastName}";
                EmailToSet = $"Email: {user.Email}";
                BirthDateToSet = $"Birth date: {Convert.ToDateTime(user.BirthDate).ToShortDateString()}";
                ChineseZodiac = $"ChineseSign: {user.ChineseSign()}";
                WestZodiac = $"SunSign: {user.SunSign()}";
                IsBirthday = "Is birthday? " + (user.IsBirthday() ? "Yes" : "No");
            }
            catch (EmailException ex)
            {
                SetToDefault();
                MessageBox.Show(ex.Message + ex.Value);
            }
            catch (PersonDiedException ex)
            {
                SetToDefault();
                MessageBox.Show(ex.Message + ex.Value);
            }
            catch (PersonTooYoungException ex)
            {
                SetToDefault();
                MessageBox.Show(ex.Message);
            }
        }

        private void SetToDefault()
        {
            IsAdult = "";
            NameToSet = "";
            LastNameToSet = "";
            EmailToSet = "";
            BirthDateToSet = "";
            ChineseZodiac = "";
            WestZodiac = "";
            IsBirthday = "";
        }
    }
}