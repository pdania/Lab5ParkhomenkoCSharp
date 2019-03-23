using System.Windows;
using System.Windows.Controls;
using Lab4ParkhomenkoCSharp2019.Tools.DataStorage;
using Lab4ParkhomenkoCSharp2019.Tools.Managers;
using Lab4ParkhomenkoCSharp2019.Tools.Navigation;
using Lab4ParkhomenkoCSharp2019.ViewModels;

namespace Lab4ParkhomenkoCSharp2019
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IContentOwner, INavigatable
    {
        public MainWindow()
        {
            StationManager.Initialize(new SerializedDataStorage());
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        private ContentControl _contentControl;

        public ContentControl ContentControl
        {
            get { return _contentControl; }
        }
    }
}
