﻿using HCI_PZ1_PR106_2021.Classes;
using HCI_PZ1_PR106_2021.Windows;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace HCI_PZ1_PR106_2021
{
    public partial class ApplicationWindow : Window
    {
        public static ObservableCollection<Battle> Battles { get; set; }
		public static List<string> ImagesToRemove { get; set; }
		private NotificationManager notificationManager;
		private static bool viewModeIsVisitor = false;
        public ApplicationWindow(bool isVisitor)
        {
            InitializeComponent();
			DataContext = this;
			notificationManager = new NotificationManager();
			viewModeIsVisitor = isVisitor;
            Battles = new ObservableCollection<Battle>();
			ImagesToRemove = new List<string>();
			LoadBattlesFromRecords();
			Closing += ApplicationWindow_Closing;
            AdjustPageForVisitor(isVisitor);
        }

		private void LoadBattlesFromRecords()
		{
			string recordsDir = GetDir("Recorded Battles");
			if (Directory.Exists(recordsDir))
			{
				string[] files = Directory.GetFiles(recordsDir);
				foreach (string file in files)
				{
					if (System.IO.Path.GetExtension(file).Equals(".xml", StringComparison.OrdinalIgnoreCase))
					{
						Battle battle = LoadFromXml(file);
						string pattern = @"(?<=\/)[^\/]+$"; // regex text after last /
						Match match = Regex.Match(battle.ImagePath, pattern);
						battle.ImageFileName = match.Value;
						battle.ImagePathAbsolute = battle.GetImagePathAbsolute();
						Battles.Add(battle);
					}
				}
			}
			string imagesDir = GetDir("Images");
			if (Directory.Exists(imagesDir))
			{
				string[] files = Directory.GetFiles(imagesDir);
				foreach (string file in files)
				{
					int occurences = 0;
					foreach (Battle battle in Battles)
					{
						if (battle.ImagePathAbsolute == file)
							occurences++;
					}
					if (occurences == 0)
					{
						try { File.Delete(file); }
						catch { }
					}
				}
			}

		}

		private Battle LoadFromXml(string filePath)
		{
			try
			{
				using (var stream = new StreamReader(filePath, System.Text.Encoding.UTF8))
				using (var reader = XmlReader.Create(stream))
				{
					XDocument doc = XDocument.Load(reader);
					XElement battleElement = doc.Element("Battle");

					Battle battle = new Battle();

					battle.Id = uint.Parse(battleElement.Element("Id").Value);
					battle.ImagePath = battleElement.Element("ImagePath").Value;
					battle.RtfPath = battleElement.Element("RtfPath").Value;
					battle.NameOfBattle = battleElement.Element("NameOfBattle").Value;
					battle.Date = DateTime.Parse(battleElement.Element("Date").Value);
					battle.DateAdded = DateTime.Parse(battleElement.Element("DateAdded").Value);
					battle.EnemySide = battleElement.Element("EnemySide").Value;
					battle.MNECommander = battleElement.Element("MNECommander").Value;
					battle.EnemyCommander = battleElement.Element("EnemyCommander").Value;
					battle.MNEStrength = battleElement.Element("MNEStrength").Value;
					battle.EnemyStrength = battleElement.Element("EnemyStrength").Value;
					battle.Result = battleElement.Element("Result").Value;

					return battle;

				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error loading Battle from XML: {ex.Message}");
				return new Battle();
			}
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

		private void ApplicationWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			RecordBattlesList();
		}

		private void RecordBattlesList()
		{
			string recordsDir = GetDir("Recorded Battles");
			if (Directory.Exists(recordsDir))
			{
				// Delete all files in the directory first
				string[] files = Directory.GetFiles(recordsDir);
				foreach (string file in files)
				{
					File.Delete(file);
				}
			}

			foreach(Battle battle in Battles)
			{
				if (battle.NameOfBattle != null)
				{
					string xml = battle.Serialize();
					string xmlFilePath = recordsDir + "\\" + battle.NameOfBattle + ".xml";
					File.WriteAllText(xmlFilePath, xml);
				}
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

		private void AdjustPageForVisitor(bool isVisitor)
        {
			if (isVisitor == true)
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
			//Hyperlink
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
					viewBattleWindow.MNEStrenght_TextBlock.Text = battle.MNEStrength;
					viewBattleWindow.EnemyStrenght_TextBlock.Text = battle.EnemyStrength;
					LoadDescription_RTF(battle.RtfPath, viewBattleWindow.Description_RichTextBox);
					viewBattleWindow.SelectedImage.Source = new BitmapImage(new Uri(battle.ImagePathAbsolute));
					viewBattleWindow.Result_TextBlock.Text = battle.Result;
					viewBattleWindow.Show();
				}
				else if (battle != null  && viewModeIsVisitor == false)
				{
					AddBattleWindow addBattle = new AddBattleWindow();
					addBattle.Name_TextBox.Text = battle.NameOfBattle;
					addBattle.BattleDate.SelectedDate = battle.Date;
					addBattle.EnemySideName_TextBox.Text = battle.EnemySide;
					addBattle.MNECommanders_TextBox.Text = battle.MNECommander;
					addBattle.EnemyCommanders_TextBox.Text = battle.EnemyCommander;
					addBattle.MNEStrenght_TextBox.Text = battle.MNEStrength;
					addBattle.EnemyStrenght_TextBox.Text = battle.EnemyStrength;
					LoadDescription_RTF(battle.RtfPath, addBattle.Description_RichTextBox);
					//Image loading
					string basePath = AppDomain.CurrentDomain.BaseDirectory;
					string relativeImagePath = battle.ImagePath;
					string imagePath = System.IO.Path.Combine(basePath, relativeImagePath);
					addBattle.SelectedImage.Source = new BitmapImage(new Uri(imagePath));
					//
					Result_ComboBox_Selection(battle, ref addBattle);
					addBattle.Add_Button.Content = "Save Changes";
					addBattle.Show();
				}
			}
		}

		private void SetMNEFlag(ref ViewBattleWindow viewBattleWindow, Battle battle)
		{
			int year = int.Parse(battle.Date.Year.ToString());
			string dir = GetDir("Flags");

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
			string dir = GetDir("Flags");
			int year = int.Parse(battle.Date.Year.ToString());
			viewBattleWindow.EnemyFlag_Border.Visibility = Visibility.Visible;
			if (enemyName == "France" ||  enemyName == "Francuska")
			{
				/*if (year >= 1804 && year <= 1815)
					viewBattleWindow.EnemyFlag.Source = new BitmapImage(new Uri(dir + "\\France_Napoleon.png"));
				else*/
					viewBattleWindow.EnemyFlag.Source = new BitmapImage(new Uri(dir + "\\France.png"));
			}
			else if (enemyName == "Ottoman Empire" || enemyName == "Osmansko carstvo")
			{
				if (year > 1796)
					viewBattleWindow.EnemyFlag.Source = new BitmapImage(new Uri(dir + "\\Ottoman_Empire.png"));
				else
					viewBattleWindow.EnemyFlag.Source = new BitmapImage(new Uri(dir + "\\Ottoman_Empire(1517-1793).png"));
			}
			else
			{
				viewBattleWindow.EnemyFlag_Border.Visibility = Visibility.Hidden;
			}
		}

		public static string GetDir(string subDir)
		{
			DirectoryInfo? parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
			DirectoryInfo parentDirectory2 = parentDirectory.Parent.Parent;
			string dir = parentDirectory2.FullName + "\\" + subDir;
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

					if (File.Exists(battle.ImagePathAbsolute))
					{
						ImagesToRemove.Add(battle.ImagePathAbsolute);
					}
				}
			}
			if (anyChecked)
			{
				MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete the selected item(s)?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

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
					this.ShowToastNotification(new ToastNotification("Success", "Successfully delete item(s)", NotificationType.Success));
				}
				else return;
			}
			else return;
		}
		private void ShowToastNotification(ToastNotification toastNotification)
		{
			notificationManager.Show(toastNotification.Title, toastNotification.Message, toastNotification.Type, "AppWindowNotificationArea");
		}
	}
}
