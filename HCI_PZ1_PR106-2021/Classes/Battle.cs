using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_PZ1_PR106_2021.Classes
{
	public class Battle
	{
		private int id;
		private string imagePath;
		private string rtfPath;
		private string nameOfBattle;
		private DateTime date;
		private DateTime dateAdded;
		private string enemySide;
		private string mneCommander;
		private string enemyCommander;
		private string mneStrenght;
		private string enemyStrenght;
		private string result;

		public Battle() { }
		public Battle(int id, 
					   string imagePath, 
					   string rtfPath, 
					   string nameOfBattle, 
					   DateTime date, 
					   string enemySide, 
					   string mneCommander, 
					   string enemyCommander, 
					   string mneStrenght, 
					   string enemyStrenght, 
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
			this.mneStrenght = mneStrenght;
			this.enemyStrenght = enemyStrenght;
			this.result = result;
			this.DateAdded = DateTime.Now;
		}

		public int Id { get => id; set => id = value; }
		public string ImagePath { get { return imagePath; } set {  imagePath = value; } }
		public string RtfPath { get { return rtfPath; } set {  rtfPath = value; } }
		public string NameOfBattle { get => nameOfBattle; set => nameOfBattle = value; }
		public DateTime Date { get => date; set => date = value; }
		public DateTime DateAdded { get => dateAdded; set => dateAdded = value; }
		public string EnemySide { get => enemySide; set => enemySide = value; }
		public string MNECommander { get => mneCommander; set => mneCommander = value; }
		public string EnemyCommander { get => enemyCommander; set => enemyCommander = value; }
		public string MNEStrenght { get => mneStrenght; set => mneStrenght = value; }
		public string EnemyStrenght { get => enemyStrenght; set => enemyStrenght = value; }
		public string Result { get => result; set => result = value; }

		public override string ToString()
		{
			return $"Id: {Id}, ImagePath: {ImagePath}, RtfPath: {RtfPath}, NameOfBattle: {NameOfBattle}, " +
			$"Date: {Date}, EnemySide: {EnemySide}, MneCommander: {MNECommander}, " +
			$"EnemyCommander: {EnemyCommander}, MneStrength: {MNEStrenght}, EnemyStrength: {EnemyStrenght}, " +
			$"Result: {Result}";
		}
	}
}
