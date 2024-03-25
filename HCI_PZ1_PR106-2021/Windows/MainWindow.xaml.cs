using HCI_PZ1_PR106_2021.Classes;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Notification.Wpf;


namespace HCI_PZ1_PR106_2021
{
	public partial class MainWindow : Window
	{
		private NotificationManager notificationManager;
		MainWindow mainWindow;
		public MainWindow()
		{
			InitializeComponent();
			notificationManager = new NotificationManager();
			mainWindow = (MainWindow)Application.Current.MainWindow;
		}


		private void LogIn_Button_Click(object sender, RoutedEventArgs e)
		{
			User user = new User();
			string mode = user.CheckLogin(LogIn_Username.Text.Trim(), LogIn_PasswordBox.Password);
			if (mode == "Admin")
			{
				ApplicationWindow appWindow = new ApplicationWindow(false);
				appWindow.Show();
				Hide();
			}
			else if (mode == "Visitor")
			{
				ApplicationWindow appWindow = new ApplicationWindow(true);
				appWindow.Show();
				Hide();
			}
			else
			{
				mainWindow.ShowToastNotification(new ToastNotification("Sign in Error", "Invalid username or password!", NotificationType.Error));
			}
		}

		private void Exit_Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void LogInWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		public void ShowToastNotification(ToastNotification toastNotification)
		{
			notificationManager.Show(toastNotification.Title, toastNotification.Message, toastNotification.Type, "WindowNotificationArea");
		}
		private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				LogIn_Button_Click(sender, e);
			}
		}
	}


}