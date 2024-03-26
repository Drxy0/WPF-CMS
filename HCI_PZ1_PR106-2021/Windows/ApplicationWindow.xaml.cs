using HCI_PZ1_PR106_2021.Classes;
using HCI_PZ1_PR106_2021.Windows;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
        public static ObservableCollection<Battle> Battles { get; set; }
		private NotificationManager notificationManager;
		private static bool viewModeIsVisitor = false;
        public ApplicationWindow(bool visitor)
        {
            InitializeComponent();
			DataContext = this;
			notificationManager = new NotificationManager();
			viewModeIsVisitor = visitor;
            Battles = new ObservableCollection<Battle>();
			DateTime dt = new DateTime(1913, 1, 1);
            Battles.Add(new Battle(0,
								   "C:\\Users\\drljo\\Desktop\\HCI_PZ1_PR106-2021\\HCI_PZ1_PR106-2021\\Flags\\Vucji_Do_flag.png",
								   "C:\\Users\\drljo\\Desktop\\HCI_PZ1_PR106-2021\\HCI_PZ1_PR106-2021\\rtf Documents\\Martinici.rtf",
								   "Martinici",
								   dt,
								   "Ottoman Empire", 
								   "kurci", "aaaa", 
								   "mnestrenght", "enemystrenght",
								   "Montenegrin Victory"));
            AdjustPage(visitor);
        }

		private void Exit_Button_Click(object sender, RoutedEventArgs e)
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
			addBattle.Closed += (s, args) =>
			{
				bool wasSaved = addBattle.ChangesSaved;
				AddBattle_Closed(wasSaved);
			};
			addBattle.Show();
		}
		private void AddBattle_Closed(bool wasSaved)
		{
			if (wasSaved)
			{
				this.ShowToastNotification(new ToastNotification("Success", "Successfully saved the battle", NotificationType.Success));
			}
		}

		private void AdjustPage(bool visitor)
        {
			if (visitor == true)
			{
				AddButton.IsEnabled = false;
				AddButton.Visibility = Visibility.Hidden;
				DeleteButton.IsEnabled = false;
				DeleteButton.Visibility = Visibility.Hidden;
				CheckBoxColumn.Visibility = Visibility.Hidden;
			}
		}

		private void NameTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

			var textBlock = sender as TextBlock;
			if (textBlock != null)
			{
				var battle = textBlock.DataContext as Battle;
				if (battle != null && viewModeIsVisitor == true)
				{
					ViewBattleWindow viewBattleWindow = new ViewBattleWindow();
					viewBattleWindow.Name_TextBlock.Text = battle.NameOfBattle;
					viewBattleWindow.BattleDate.Text = battle.Date.ToString("dd/MM/yyyy");
					viewBattleWindow.EnemySideName_TextBlock.Text = battle.EnemySide;
					SetMNEFlag(ref viewBattleWindow, battle);
					SetEnemyFlag(ref viewBattleWindow, battle);
					viewBattleWindow.MNECommanders_TextBlock.Text = battle.MNECommander;
					viewBattleWindow.EnemyCommanders_TextBlock.Text = battle.EnemyCommander;
					viewBattleWindow.MNEStrenght_TextBlock.Text = battle.MNEStrenght;
					viewBattleWindow.EnemyStrenght_TextBlock.Text = battle.EnemyStrenght;
					LoadDescription_RTF(battle.RtfPath, viewBattleWindow.Description_RichTextBox);
					viewBattleWindow.SelectedImage.Source = new BitmapImage(new Uri(battle.ImagePath));
					viewBattleWindow.Result_TextBlock.Text = battle.Result;
					viewBattleWindow.Show();
				}
				else if (battle !=null  && viewModeIsVisitor == false)
				{
					AddBattleWindow addBattle = new AddBattleWindow();
					addBattle.Name_TextBox.Text = battle.NameOfBattle;
					addBattle.BattleDate.SelectedDate = battle.Date;
					addBattle.EnemySideName_TextBox.Text = battle.EnemySide;
					addBattle.MNECommanders_TextBox.Text = battle.MNECommander;
					addBattle.EnemyCommanders_TextBox.Text = battle.EnemyCommander;
					addBattle.MNEStrenght_TextBox.Text = battle.MNEStrenght;
					addBattle.EnemyStrenght_TextBox.Text = battle.EnemyStrenght;
					LoadDescription_RTF(battle.RtfPath, addBattle.Description_RichTextBox);
					addBattle.SelectedImage.Source = new BitmapImage(new Uri(battle.ImagePath));
					Result_ComboBox_Selection(battle, ref addBattle);

					addBattle.Add_Button.Content = "Save Changes";
					addBattle.Show();
				}
			}
		}

		private void SetMNEFlag(ref ViewBattleWindow viewBattleWindow, Battle battle)
		{
			int year = int.Parse(battle.Date.Year.ToString());
			string dir = GetFlagsDir();

			if (year <= 1496)
				viewBattleWindow.MNEFlag.Source = new BitmapImage(new Uri(dir + "\\Montenegro_Crnojevic.png"));
			else if (year > 1496 && year < 1852)
				viewBattleWindow.MNEFlag.Source = new BitmapImage(new Uri(dir + "\\Montenegro_Krstas_white.png"));
			else if (year >= 1852 && year <= 1860)
				viewBattleWindow.MNEFlag.Source = new BitmapImage(new Uri(dir + "\\Montenegro(1852-1860).png"));
			else if (year > 1860 && year <= 1905)
				viewBattleWindow.MNEFlag.Source = new BitmapImage(new Uri(dir + "\\Montenegro(1860–1905).png"));
			else if (year > 1905)
				viewBattleWindow.MNEFlag.Source = new BitmapImage(new Uri(dir + "\\Montenegro(1905-1918).png"));
		}
		private void SetEnemyFlag(ref ViewBattleWindow viewBattleWindow, Battle battle)
		{
			string enemyName = viewBattleWindow.EnemySideName_TextBlock.Text;
			string dir = GetFlagsDir();
			int year = int.Parse(battle.Date.Year.ToString());
			viewBattleWindow.EnemyFlag_Border.Visibility = Visibility.Visible;
			if (enemyName == "France" ||  enemyName == "Francuska")
			{
				if (year >= 1804 && year <= 1815)
					viewBattleWindow.EnemyFlag.Source = new BitmapImage(new Uri(dir + "\\France_Napoleon.png"));
				else
					viewBattleWindow.EnemyFlag.Source = new BitmapImage(new Uri(dir + "\\France.png"));
			}
			else if (enemyName == "Ottoman Empire" || enemyName == "Osmansko carstvo")
			{
				if (year >= 1793)
					viewBattleWindow.EnemyFlag.Source = new BitmapImage(new Uri(dir + "\\Ottoman_Empire.png"));
				else
					viewBattleWindow.EnemyFlag.Source = new BitmapImage(new Uri(dir + "\\Ottoman_Empire(1517-1793).png"));
			}
			else
			{
				viewBattleWindow.EnemyFlag_Border.Visibility = Visibility.Hidden;
			}
		}

		private string GetFlagsDir()
		{
			DirectoryInfo parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
			DirectoryInfo parentDirectory2 = parentDirectory.Parent.Parent;
			string dir = parentDirectory2.FullName + "\\Flags";
			return dir;
		}

		private void Result_ComboBox_Selection(Battle battle, ref AddBattleWindow addBattle)
		{
			if (battle.Result == "Montenegrin Victory")
				addBattle.Result_ComboBox.SelectedIndex = 1;
			else if (battle.Result == "Montenegrin Defeat")
				addBattle.Result_ComboBox.SelectedIndex = 2;
			else
				addBattle.Result_ComboBox.SelectedIndex = 0;
		}

		private void LoadDescription_RTF(string filePath, System.Windows.Controls.RichTextBox richTextBox)
		{
			string descriptionRTF = string.Empty;

			if (File.Exists(filePath))
			{
				descriptionRTF = File.ReadAllText(filePath);
			}
			TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
			using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(descriptionRTF)))
			{
				range.Load(stream, DataFormats.Rtf);
			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			bool anyChecked = false;
			foreach (Battle battle in Battles)
			{
				if (battle.IsChecked)
				{
					anyChecked = true;
					if (File.Exists(battle.RtfPath))
					{
						File.Delete(battle.RtfPath);
					}
				}
			}
			if (anyChecked)
			{
				MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to remove the selected item(s)?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (messageBoxResult == MessageBoxResult.Yes)
				{
					for (int i = Battles.Count - 1; i >= 0; i--)
					{
						if (Battles[i].IsChecked)
						{
							Battles.RemoveAt(i);
						}
					}
					BattlesDataGrid.Items.Refresh();
					this.ShowToastNotification(new ToastNotification("Success", "Successfully removed item(s)", NotificationType.Success));
				}
				else
				{
					return;
				}
			}
			else
			{
				return;
			}
		}
		private void ShowToastNotification(ToastNotification toastNotification)
		{
			notificationManager.Show(toastNotification.Title, toastNotification.Message, toastNotification.Type, "AppWindowNotificationArea");
		}
	}
}
