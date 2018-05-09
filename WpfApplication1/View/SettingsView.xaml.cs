using ImageService.ViewModel;
using ImageServiceGUI.Model;
using ImageServiceGUI.ViewModel;
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


namespace ImageServiceGUI.View

{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private IViewModel vm;
        public SettingsView()
        {
            InitializeComponent();
            this.DataContext = new SettingsVM(new SettingsModel());
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //vm.onClick...
        }
    }
}
