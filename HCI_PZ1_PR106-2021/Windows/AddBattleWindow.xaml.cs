using HCI_PZ1_PR106_2021.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class AddBattleWindow : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public static string path = string.Empty;
		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public AddBattleWindow()
        {
            InitializeComponent();
			Result_ComboBox.SelectedIndex = 0;
			FontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
			var colorList = typeof(Colors).GetProperties()
							  .Where(p => p.PropertyType == typeof(Color))
							  .OrderBy(p => p.Name)
							  .Select(p => new { ColorName = p.Name, ColorValue = (Color)p.GetValue(null) })
							  .ToList();
			TextColorComboBox.ItemsSource = colorList;

		}

		private void SelectImage_Button_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg;*.gif;*.bmp)|*.png;*.jpeg;*.jpg;*.gif;*.bmp|All files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == true)
			{
				string imagePath = openFileDialog.FileName;
				try
				{
					BitmapImage bitmap = new BitmapImage();
					bitmap.BeginInit();
					bitmap.UriSource = new Uri(imagePath);
					bitmap.EndInit();
					SelectedImage.Source = bitmap;
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error loading image: {ex.Message}");
				}
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			//TODO
			//check fields



			string name = Name_TextBox.Name;
			//DateTime date = BattleDate;
			string enemyName = EnemySideName_TextBox.Text;
			string mneCommander = MNECommanders_TextBox.Text;
			string enemyCommander = EnemyCommanders_TextBox.Text;
			string mneStrenght = MNEStrenght_TextBox.Text;
			string enemyStrenght = EnemyStrenght_TextBox.Text;
			string imagePath = SelectedImage.Source.ToString();
			//Description
		}

		private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void TextColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
