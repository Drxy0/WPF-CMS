﻿using HCI_PZ1_PR106_2021.Classes;
using HCI_PZ1_PR106_2021.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Battle> Battles { get; set; }
        private static bool viewModeIsVisitor = false;
        public ApplicationWindow(bool visitor)
        {
            InitializeComponent();
			DataContext = this;
			viewModeIsVisitor = visitor;
            Battles = new ObservableCollection<Battle>();
			DateTime dt = DateTime.Now.Date;
            Battles.Add(new Battle(0,
								   "C:\\Users\\drljo\\Desktop\\HCI_PZ1_PR106-2021\\HCI_PZ1_PR106-2021\\Flags\\Vucji_Do_flag.png",
								   "C:\\Users\\drljo\\Desktop\\HCI_PZ1_PR106-2021\\HCI_PZ1_PR106-2021\\rtf Documents\\Dubrovnik.rtf",
								   "Martinici",
								   dt, 
								   "Turci", 
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
            addBattle.Show();
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
	}
}