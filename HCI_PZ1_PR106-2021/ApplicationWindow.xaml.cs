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
using System.Windows.Shapes;

namespace HCI_PZ1_PR106_2021
{
    /// <summary>
    /// Interaction logic for ApplicationWindow.xaml
    /// </summary>
    public partial class ApplicationWindow : Window
    {
        public ApplicationWindow()
        {
            InitializeComponent();
        }

		private void App_Exit_Button_Click(object sender, RoutedEventArgs e)
		{
            Close();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.Show();
            }
        }

		private void ApplicationWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
            this.DragMove();
        }
    }
}
