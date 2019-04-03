using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        private RelayCommand<object> _kill;
        private Thread _workingThread;
        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;
        public static event Action _stopThreads;
        public ObservableCollection<ProcessList> Processes { get; set; }

        public ProcessListViewModel()
        {
            Processes = new ObservableCollection<ProcessList>();
            foreach (var p in Process.GetProcesses())
            {
                Processes.Add(new ProcessList(p));
            }

            //StartWorkingThread();
        }

        private Process _selectedProcess;
        public Process SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand<object> KillProcess
        {
            get
            {
                return _kill ?? (_kill = new RelayCommand<object>(
                           KillProcessImplementation, o => CanExecuteCommand()));
            }
        }

        private void KillProcessImplementation(object obj)
        {
            SelectedProcess.Kill();
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