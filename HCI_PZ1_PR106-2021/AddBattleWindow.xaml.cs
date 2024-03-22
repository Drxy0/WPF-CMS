using Microsoft.Win32;
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
    /// Interaction logic for AddBattle.xaml
    /// </summary>
    public partial class AddBattleWindow : Window
    {
        public AddBattleWindow()
        {
            InitializeComponent();
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
			this.Close();
		}
	}
}
