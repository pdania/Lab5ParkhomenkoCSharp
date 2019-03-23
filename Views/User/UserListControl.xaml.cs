using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab4ParkhomenkoCSharp2019.Tools.DataStorage;
using Lab4ParkhomenkoCSharp2019.Tools.Managers;
using Lab4ParkhomenkoCSharp2019.Tools.Navigation;
using Lab4ParkhomenkoCSharp2019.ViewModels;
using Lab4ParkhomenkoCSharp2019.ViewModels.Date;

namespace Lab4ParkhomenkoCSharp2019.Views.User
{
    /// <summary>
    /// Interaction logic for UserListControl.xaml
    /// </summary>
    public partial class UserListControl : UserControl, INavigatable
    {
        public UserListControl()
        {
            InitializeComponent();
            DataContext = new UserListViewModel();
        }
    }
}
