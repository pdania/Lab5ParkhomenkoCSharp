using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Lab5ParkhomenkoCSharp2019.Tools;
using Lab5ParkhomenkoCSharp2019.Tools.Managers;

namespace Lab5ParkhomenkoCSharp2019.ViewModels
{
    class ProcessListViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private Thread _workingThread;
        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;
        public static event Action _stopThreads;

        #region Fields

        public ObservableCollection<ProcessList> Processes { get; private set; }

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
            Processes = new ObservableCollection<ProcessList>();
            foreach (var p in Process.GetProcesses())
            {
                Processes.Add(new ProcessList(p));
            }

            //StartWorkingThread();
        }

        private ProcessList _selectedProcess;

        public ProcessList SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
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

        private void OpenFolderImplementation(object obj)
        {
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

        #endregion

        private void WatchModulesThreadsImplementation(object obj)
        {
            Modules = new ObservableCollection<ModuleItem>();
            Threads = new ObservableCollection<ThreadItem>();
            ProcessModuleCollection modules;
            try
            {
                modules = SelectedProcess.Process.Modules; 
            }
            catch
            {
                MessageBox.Show("Access to this process denied");
                return;
            }

            foreach (ProcessModule module in modules)
            {
                Modules.Add(new ModuleItem(module));
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
            SelectedProcess.Process.Kill();
            Processes.Remove(SelectedProcess);
        }

        private bool CanExecuteCommand()
        {
            return SelectedProcess != null;
        }

        private void StartWorkingThread()
        {
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _workingThread = new Thread(WorkingThreadProcess);
            _workingThread.Start();
            _stopThreads += StopWorkingThread;
        }

        private void WorkingThreadProcess()
        {
            int i = 0;
            while (!_token.IsCancellationRequested)
            {
                Processes = new ObservableCollection<ProcessList>();

                foreach (var p in Process.GetProcesses())
                {
                    Processes.Add(new ProcessList(p));
                }

                for (int j = 0; j < 3; j++)
                {
                    Thread.Sleep(500);
                    if (_token.IsCancellationRequested)
                        break;
                }

                if (_token.IsCancellationRequested)
                    break;
                LoaderManager.Instance.HideLoader();
                for (int j = 0; j < 10; j++)
                {
                    Thread.Sleep(500);
                    if (_token.IsCancellationRequested)
                        break;
                }

                if (_token.IsCancellationRequested)
                    break;
                i++;
            }
        }

        private void StopWorkingThread()
        {
            _tokenSource.Cancel();
            _workingThread.Join(2000);
            _workingThread.Abort();
            _workingThread = null;
        }


        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}