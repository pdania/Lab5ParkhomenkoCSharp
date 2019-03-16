using System.Windows.Controls;
using Lab3ParkhomenkoCSharp2019.ViewModels.Date;

namespace Lab3ParkhomenkoCSharp2019.Views.Date
{
    public partial class DateControl : UserControl
    {
        public DateControl()
        {
            InitializeComponent();
            DataContext = new DateViewModel();
        }
    }
}
