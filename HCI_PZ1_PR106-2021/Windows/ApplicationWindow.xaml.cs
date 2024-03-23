using HCI_PZ1_PR106_2021.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class ApplicationWindow : Window
    {
        public ObservableCollection<Battles> Battles { get; set; }
        public ApplicationWindow()
        {
            InitializeComponent();
            Battles = new ObservableCollection<Battles>();
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

		private void AddItem_Button_Click(object sender, RoutedEventArgs e)
		{
			AddBattleWindow addBattle = new AddBattleWindow();
            addBattle.Show();
		}
	}
}
