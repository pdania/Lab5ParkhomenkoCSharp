using System;
using System.ComponentModel;
using System.Windows;
using Lab5ParkhomenkoCSharp2019.ViewModels;

namespace Lab5ParkhomenkoCSharp2019
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static event Action StopThreads;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        /*
         * Function that stop all threads after shutting down
         */
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            StopThreads?.Invoke();
            Environment.Exit(1);
        }
    }
}
