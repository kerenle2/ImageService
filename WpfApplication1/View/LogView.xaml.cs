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
using ImageServiceGUI.ViewModel;
namespace ImageServiceGUI.View
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        private LogVM LogVM;

        public LogView()
        {
            InitializeComponent();
            LogVM = new LogVM();
            this.DataContext = LogVM;
          //  dataGrid.ItemsSource = LogVM.logMessage;
        }
    }
}
