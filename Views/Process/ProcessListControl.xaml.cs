using System.Windows.Controls;
using Lab5ParkhomenkoCSharp2019.ViewModels;

namespace Lab5ParkhomenkoCSharp2019.Views.Process
{
    /// <summary>
    /// Interaction logic for ProcessListControl.xaml
    /// </summary>
    public partial class ProcessListControl : UserControl
    {
        public ProcessListControl()
        {
            InitializeComponent();
            DataContext = new ProcessListViewModel();
        }
    }
}
