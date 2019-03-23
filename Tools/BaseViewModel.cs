using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lab4ParkhomenkoCSharp2019.Properties;

namespace Lab4ParkhomenkoCSharp2019.Tools
{
    [Serializable]
    internal abstract class BaseViewModel:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
