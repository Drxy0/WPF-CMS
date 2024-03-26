using HCI_PZ1_PR106_2021.Classes;
using Microsoft.Win32;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class AddBattleWindow : Window, INotifyPropertyChanged
	{
		private NotificationManager notificationManager;
		public event PropertyChangedEventHandler PropertyChanged;
		private static int id = 0;
		public bool ChangesSaved = false;
		private SolidColorBrush defaultTextBoxBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xAB, 0xAD, 0xB3));

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
			notificationManager = new NotificationManager();
			Result_ComboBox.SelectedIndex = 0;
			FontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
			FontSizeComboBox.ItemsSource = Enumerable.Range(1, 48).Select(i => (double)i).ToList();
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
			ChangesSaved = false;
			this.Close();
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			if (!FormErrorOccured())
			{
				string name = Name_TextBox.Text;
				DateTime date = (DateTime)BattleDate.SelectedDate;
				string enemyName = EnemySideName_TextBox.Text;
				string mneCommander = MNECommanders_TextBox.Text;
				string enemyCommander = EnemyCommanders_TextBox.Text;
				string mneStrenght = MNEStrenght_TextBox.Text;
				string enemyStrenght = EnemyStrenght_TextBox.Text;
				string imagePath = SelectedImage.Source.ToString();
				string rtfPath = SaveToRTF();
				string result = Result_ComboBox.SelectedValue.ToString();

				Battle battle = new Battle(id++, imagePath, rtfPath, name, date, enemyName, mneCommander, enemyCommander, mneStrenght, enemyStrenght, result);
				ApplicationWindow.Battles.Add(battle);
				ChangesSaved = true;
				this.Close();
			}
		}

		private bool FormErrorOccured()
		{
			bool errorOccured = false;

			if (Name_TextBox.Text.Equals(String.Empty))
			{
				ToastError();
				NameError_Label.Content = "Field cannot be left empty!";
				Name_TextBox.BorderBrush = Brushes.Yellow;
				errorOccured = true;
			}
			else
			{
				NameError_Label.Content = "";
				Name_TextBox.BorderBrush = defaultTextBoxBrush;
			}

			if (EnemySideName_TextBox.Text.Equals(String.Empty))
			{
				ToastError();
				EnemySideNameError_Label.Content = "Field cannot be left empty!";
				EnemySideName_TextBox.BorderBrush = Brushes.Yellow;
				errorOccured = true;
			}
			else
			{
				EnemySideNameError_Label.Content = "";
				EnemySideName_TextBox.BorderBrush = defaultTextBoxBrush;
			}

			if (MNECommanders_TextBox.Text.Equals(String.Empty))
			{
				ToastError();
				MNECommandersError_Label.Content = "Field cannot be left empty!";
				MNECommanders_TextBox.BorderBrush = Brushes.Yellow;
				errorOccured = true;
			}
			else
			{
				MNECommandersError_Label.Content = "";
				MNECommanders_TextBox.BorderBrush = defaultTextBoxBrush;
			}

			if (EnemyCommanders_TextBox.Text.Equals(String.Empty))
			{
				ToastError();
				EnemyCommandersError_Label.Content = "Field cannot be left empty!";
				EnemyCommanders_TextBox.BorderBrush = Brushes.Yellow;
				errorOccured = true;
			}
			else
			{
				EnemyCommandersError_Label.Content = "";
				EnemyCommanders_TextBox.BorderBrush = defaultTextBoxBrush;
			}

			if (MNEStrenght_TextBox.Text.Equals(String.Empty))
			{
				ToastError();
				MNEStrenghtError_Label.Content = "Field cannot be left empty!";
				MNEStrenght_TextBox.BorderBrush = Brushes.Yellow;
				errorOccured = true;
			}
			else if (!int.TryParse(MNEStrenght_TextBox.Text.Replace(" ", ""), out _))
			{
				MNEStrenghtError_Label.Content = "Field must be an integer!";
				MNEStrenght_TextBox.BorderBrush = Brushes.Yellow;
				errorOccured = true;
			}
			else
			{
				MNEStrenghtError_Label.Content = "";
				MNEStrenght_TextBox.BorderBrush = defaultTextBoxBrush;
			}

			if (EnemyStrenght_TextBox.Text.Equals(String.Empty))
			{
				ToastError();
				EnemyStrenghtError_Label.Content = "Field cannot be left empty!";
				EnemyStrenght_TextBox.BorderBrush = Brushes.Yellow;
				errorOccured = true;
			}
			else if (!int.TryParse(EnemyStrenght_TextBox.Text.Replace(" ", ""), out _))
			{
				EnemyStrenghtError_Label.Content = "Field must be an integer!";
				EnemyStrenght_TextBox.BorderBrush = Brushes.Yellow;
				errorOccured = true;
			}
			else
			{
				EnemyStrenghtError_Label.Content = "";
				EnemyStrenght_TextBox.BorderBrush = defaultTextBoxBrush;
			}

			if (!BattleDate.SelectedDate.HasValue)
			{
				ToastError();
				DateError_Label.Content = "Invalid date!";
				BattleDate.BorderBrush = Brushes.Yellow;
				errorOccured = true;
			}
			else
			{
				DateError_Label.Content = "";
				BattleDate.BorderBrush = defaultTextBoxBrush;
			}

			try
			{
				string imagePath = SelectedImage.Source.ToString();
			}
			catch (Exception ex)
			{
				ToastError();
				ImageError_Label.Content = "An image must be selected!";
				errorOccured = true;
			}

			return errorOccured;
		}

		private string SaveToRTF()
		{
			FlowDocument document = Description_RichTextBox.Document;
			TextRange textRange = new TextRange(document.ContentStart, document.ContentEnd);
			string uniqueFileName = $"{Name_TextBox.Text.ToString()}.rtf";
			string filePath = System.IO.Path.Combine("../../../rtf Documents/", uniqueFileName);
			FileStream fileStream = File.Create(filePath);
			textRange.Save(fileStream, DataFormats.Rtf);
			fileStream.Close();
			return filePath;
		}
		#region RichTextBox stuff
		private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (FontSizeComboBox.SelectedItem != null && !Description_RichTextBox.Selection.IsEmpty)
			{
				Description_RichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSizeComboBox.SelectedItem);
			}
		}
		private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (FontFamilyComboBox.SelectedItem != null && !Description_RichTextBox.Selection.IsEmpty)
			{
				Description_RichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyComboBox.SelectedItem);
			}
		}

		private void TextColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (TextColorComboBox.SelectedItem != null && !Description_RichTextBox.Selection.IsEmpty)
			{
				var selectedColor = (Color)typeof(Colors).GetProperty(((dynamic)TextColorComboBox.SelectedItem).ColorName).GetValue(null);
				var brush = new SolidColorBrush(selectedColor);
				Description_RichTextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, brush);
			}
		}
		private void Description_RichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			object fontWeight = Description_RichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
			BoldToggleButton.IsChecked = (fontWeight != DependencyProperty.UnsetValue) && (fontWeight.Equals(FontWeights.Bold));

			object fontStyle = Description_RichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
			ItalicToggleButton.IsChecked = (fontStyle != DependencyProperty.UnsetValue) && (fontStyle.Equals(FontStyles.Italic));

			object textDecoration = Description_RichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
			UnderlineToggleButton.IsChecked = (textDecoration != DependencyProperty.UnsetValue) && ((textDecoration as TextDecorationCollection).Contains(TextDecorations.Underline[0]));

			object fontSize = Description_RichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty);
			FontSizeComboBox.SelectedItem = fontSize;

			var colorList = typeof(Colors).GetProperties()
							  .Where(p => p.PropertyType == typeof(Color))
							  .OrderBy(p => p.Name)
							  .Select(p => new { ColorName = p.Name, ColorValue = (Color)p.GetValue(null) })
							  .ToList();

			object fontColor = Description_RichTextBox.Selection.GetPropertyValue(Inline.ForegroundProperty);
			TextColorComboBox.SelectedItem = fontColor;
			for (int i = 0; i < colorList.Count; i++)
			{
				if (colorList[i].ColorValue.ToString().Equals(fontColor.ToString()))
				{
					TextColorComboBox.SelectedItem = colorList[i];
				}
			}

		}

		private void Description_RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			UpdateWordCount();
		}
		private void UpdateWordCount()
		{
			string text = new TextRange(Description_RichTextBox.Document.ContentStart, Description_RichTextBox.Document.ContentEnd).Text;
			int wordCount = text.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
			WordCount_TextBox.Text = $"Word Count: {wordCount}";
		}
		#endregion
		public void ToastError()
		{
			ShowToastNotification(new ToastNotification("Error", "Invalid fields!", NotificationType.Error));
		}
		public void ShowToastNotification(ToastNotification toastNotification)
		{
			notificationManager.Show(toastNotification.Title, toastNotification.Message, toastNotification.Type, "AddBattleWindowNotificationArea");
		}

		private void BattleDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (BattleDate.SelectedDate.HasValue)
			{
				DateTime selectedDate = BattleDate.SelectedDate.Value;

				if (selectedDate.Year < 1492 || selectedDate.Year > 1914)
				{
					ShowToastNotification(new ToastNotification("Error", "Date must be between years 1492 and 1914", NotificationType.Error));
					BattleDate.SelectedDate = null;
					DateError_Label.Content = "Invalid date!";
					BattleDate.BorderBrush = Brushes.Yellow;
				}
                else
                {
					DateError_Label.Content = "";
					BattleDate.BorderBrush = defaultTextBoxBrush;
				}
            }
		}
	}
}
