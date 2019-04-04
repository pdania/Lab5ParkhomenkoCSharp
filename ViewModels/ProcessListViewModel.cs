﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Lab5ParkhomenkoCSharp2019.Tools;

namespace Lab5ParkhomenkoCSharp2019.ViewModels
{
    class ProcessListViewModel : BaseViewModel, INotifyPropertyChanged
    {

        #region Fields
        
        private ObservableCollection<ProcessItem> _processes;

        public ObservableCollection<ProcessItem> Processes
        {
            get { return _processes; }
            private set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ModuleItem> _modules;

        public ObservableCollection<ModuleItem> Modules
        {
            get { return _modules; }
            private set
            {
                _modules = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ThreadItem> _threads;

        public ObservableCollection<ThreadItem> Threads
        {
            get { return _threads; }
            private set
            {
                _threads = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public ProcessListViewModel()
        {
            Processes = new ObservableCollection<ProcessItem>();
            foreach (var p in Process.GetProcesses())
            {
                Processes.Add(new ProcessItem(p));
            }
           
            Thread refreshMetadataThread = new Thread(RefreshMetadata);
            refreshMetadataThread.Start();
            Thread refreshProcessesThread = new Thread(RefreshProcesses);
            refreshProcessesThread.Start();
        }

        private ProcessItem _selectedProcess;

        public ProcessItem SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
            }
        }

        private int _sort;
        public int Sort
        {
            get
            {
                return _sort;
            }
            set
            {
                _sort = value;
                SortProcesses(_sort, Processes);
            }
        }

        #region Commands

        private RelayCommand<object> _kill;
        private RelayCommand<object> _watchModulesThreads;
        private RelayCommand<object> _openFolder;

        public RelayCommand<object> KillProcess
        {
            get
            {
                return _kill ?? (_kill = new RelayCommand<object>(
                           KillProcessImplementation, o => CanExecuteCommand()));
            }
        }

        public RelayCommand<object> WatchModulesThreads
        {
            get
            {
                return _watchModulesThreads ?? (_watchModulesThreads = new RelayCommand<object>(
                           WatchModulesThreadsImplementation, o => CanExecuteCommand()));
            }
        }


        public RelayCommand<object> OpenFolder
        {
            get
            {
                return _openFolder ?? (_openFolder = new RelayCommand<object>(
                           OpenFolderImplementation, o => CanExecuteCommand()));
            }
        }

        #endregion

        private void OpenFolderImplementation(object obj)
        {
            if (SelectedProcess == null)
            {
                MessageBox.Show("Select process first");
                return;
            }

            if (SelectedProcess.FileName == "Access denied")
            {
                MessageBox.Show("Can't open folder of this process");
                return;
            }

            try
            {
                Process.Start(Path.GetDirectoryName(SelectedProcess.FileName) ?? throw new InvalidOperationException());
            }
            catch
            {
            }
        }

        private void WatchModulesThreadsImplementation(object obj)
        {
            if (SelectedProcess == null)
            {
                MessageBox.Show("Select process first");
                return;
            }

            Modules = new ObservableCollection<ModuleItem>();
            Threads = new ObservableCollection<ThreadItem>();
            ProcessModuleCollection modules;
            try
            {
                modules = SelectedProcess.Process.Modules;
                foreach (ProcessModule module in modules)
                {
                    Modules.Add(new ModuleItem(module));
                }
            }
            catch
            {
            }

            ProcessThreadCollection threads;
            try
            {
                threads = SelectedProcess.Process.Threads;
            }
            catch
            {
                MessageBox.Show("Access to this process denied");
                return;
            }

            foreach (ProcessThread thread in threads)
            {
                Threads.Add(new ThreadItem(thread));
            }
        }

        private void KillProcessImplementation(object obj)
        {
            if (SelectedProcess == null)
            {
                MessageBox.Show("Select process first");
                return;
            }

            if (SelectedProcess.FileName == "Access denied")
            {
                MessageBox.Show("Can't kill system process");
                return;
            }

            try
            {
                SelectedProcess.Process.Kill();
                Processes.Remove(SelectedProcess);
            }
            catch
            {

            }
        }

        private async void SortProcesses(int sortBy, ObservableCollection<ProcessItem> collection)
        {
            ObservableCollection<ProcessItem> processesTemp = null;
            switch (sortBy)
            {
                case 0:
                    Processes = collection;
                    return;
                case 1:
                    await Task.Run(() =>
                        processesTemp =
                            new ObservableCollection<ProcessItem>(collection.OrderBy(i => i.Id)));
                    break;
                case 2:
                    await Task.Run(() =>
                        processesTemp =
                            new ObservableCollection<ProcessItem>(collection.OrderBy(i => i.ProcessName)));
                    break;
                case 3:
                    await Task.Run(() =>
                        processesTemp =
                            new ObservableCollection<ProcessItem>(collection.OrderBy(i => i.FileName)));
                    break;
                case 4:
                    await Task.Run(() =>
                        processesTemp =
                            new ObservableCollection<ProcessItem>(collection.OrderBy(i => i.Threads)));
                    break;
                case 5:
                    await Task.Run(() =>
                        processesTemp =
                            new ObservableCollection<ProcessItem>(collection.OrderBy(i => i.Cpu)));
                    break;
                case 6:
                    await Task.Run(() =>
                        processesTemp =
                            new ObservableCollection<ProcessItem>(collection.OrderBy(i => i.RamPercent)));
                    break;
                case 7:
                    await Task.Run(() =>
                        processesTemp =
                            new ObservableCollection<ProcessItem>(collection.OrderBy(i => i.RamVolume)));
                    break;
                case 8:
                    await Task.Run(() =>
                        processesTemp =
                            new ObservableCollection<ProcessItem>(collection.OrderBy(i => i.UserName)));
                    break;
                case 9:
                    await Task.Run(() =>
                        processesTemp =
                            new ObservableCollection<ProcessItem>(collection.OrderBy(i => i.StartTime)));
                    break;
                case 10:
                    await Task.Run(() =>
                        processesTemp =
                            new ObservableCollection<ProcessItem>(collection.OrderBy(i => i.Responding)));
                    break;
            }
            Processes = processesTemp;
        }

        private bool CanExecuteCommand()
        {
            return true;
        }

        private void RefreshMetadata()
        {
            while (true)
            {
                foreach (ProcessItem process in Processes)
                {
                    process.RefreshMetadata();
                }

                Processes = new ObservableCollection<ProcessItem>(Processes);
                Thread.Sleep(1000);
            }
        }

        private void RefreshProcesses()
        {
            while (true)
            {
                List<ProcessItem> oldProcesses = Processes.ToList();
                Process[] newProcesses = Process.GetProcesses();
                List<ProcessItem> newProcessesList =
                    (from pr in newProcesses select new ProcessItem(pr)).ToList();

                for (int i = 0; i < oldProcesses.Count; i++)
                {
                    if (!newProcessesList.Contains(oldProcesses[i]))
                    {
                        oldProcesses.RemoveAt(i);
                    }
                }

                foreach (ProcessItem item in newProcessesList)
                    if (!oldProcesses.Contains(item))
                    {
                        oldProcesses.Add(item);
                    }

                SortProcesses(Sort, new ObservableCollection<ProcessItem>(oldProcesses));
                Thread.Sleep(3000);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}