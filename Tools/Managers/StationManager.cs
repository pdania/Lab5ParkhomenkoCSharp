using System;
using System.Windows;
using Lab4ParkhomenkoCSharp2019.ViewModels.Date;
using Lab4ParkhomenkoCSharp2019.Tools.DataStorage;

namespace Lab4ParkhomenkoCSharp2019.Tools.Managers
{
    internal static class StationManager
    {
        public static event Action StopThreads;

        private static IDataStorage _dataStorage;

        internal static Person CurrentPerson { get; set; }

        internal static IDataStorage DataStorage
        {
            get { return _dataStorage; }
        }

        internal static void Initialize(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        internal static void CloseApp()
        {
            MessageBox.Show("ShutDown");
            Environment.Exit(1);
        }
    }
}