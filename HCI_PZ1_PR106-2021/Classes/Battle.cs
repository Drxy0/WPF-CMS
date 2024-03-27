using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Windows;

namespace HCI_PZ1_PR106_2021.Classes
{
	[Serializable]
	public class Battle : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private uint id;
		private string imagePath;
		private string rtfPath;
		private string nameOfBattle;
		private DateTime date;
		private DateTime dateAdded;
		private string enemySide;
		private string mneCommander;
		private string enemyCommander;
		private string mneStrength;
		private string enemyStrength;
		private string result;
		private bool isChecked;

		public Battle() { }
		public Battle(uint id, 
					   string imagePath, 
					   string rtfPath, 
					   string nameOfBattle, 
					   DateTime date, 
					   string enemySide, 
					   string mneCommander, 
					   string enemyCommander, 
					   string mneStrength, 
					   string enemyStrength, 
					   string result)
		{
			this.id = id;
			this.imagePath = imagePath;
			this.rtfPath = rtfPath;
			this.nameOfBattle = nameOfBattle;
			this.date = date.Date;
			this.enemySide = enemySide;
			this.mneCommander = mneCommander;
			this.enemyCommander = enemyCommander;
			this.mneStrength = mneStrength;
			this.enemyStrength = enemyStrength;
			this.result = result;
			this.DateAdded = DateTime.Now;
		}

		public uint Id { get => id; set => id = value; }
		public string ImagePath { get { return imagePath; } set {  imagePath = value; } }
		public string RtfPath { get { return rtfPath; } set {  rtfPath = value; } }
		public string NameOfBattle { get => nameOfBattle; set => nameOfBattle = value; }
		public DateTime Date { get => date; set => date = value; }
		public DateTime DateAdded { get => dateAdded; set => dateAdded = value; }
		public string EnemySide { get => enemySide; set => enemySide = value; }
		public string MNECommander { get => mneCommander; set => mneCommander = value; }
		public string EnemyCommander { get => enemyCommander; set => enemyCommander = value; }
		public string MNEStrength { get => mneStrength; set => mneStrength = value; }
		public string EnemyStrength { get => enemyStrength; set => enemyStrength = value; }
		public string Result { get => result; set => result = value; }

		public override string ToString()
		{
			return $"Id: {Id}, ImagePath: {ImagePath}, RtfPath: {RtfPath}, NameOfBattle: {NameOfBattle}, " +
			$"Date: {Date}, EnemySide: {EnemySide}, MneCommander: {MNECommander}, " +
			$"EnemyCommander: {EnemyCommander}, MneStrength: {MNEStrength}, EnemyStrength: {EnemyStrength}, " +
			$"Result: {Result}";
		}

		public bool IsChecked
		{
			get { return isChecked; }
			set
			{
				if (isChecked != value)
				{
					isChecked = value;
					OnPropertyChanged(nameof(IsChecked));
				}
			}
		}

		public string Serialize()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Battle));
			StringWriter writer = new StringWriter();
			serializer.Serialize(writer, this);
			return writer.ToString();
		}


	}
}
