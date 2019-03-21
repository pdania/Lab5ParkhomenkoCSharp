using System.Windows.Controls;
using Lab4ParkhomenkoCSharp2019.ViewModels.Date;

namespace Lab4ParkhomenkoCSharp2019.Views.Date
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
