using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_PZ1_PR106_2021.Classes
{
	public class Battles
	{
		int id;
		string imagePath;
		string rtfPath;
		string nameOfBattle;
		DateTime date;
		string EnemySide;
		string MNECommander;
		string EnemyCommander;
		string MNEStrenght;
		string EnemyStrenght;
		int Result;

		public Battles(int id, 
					   string imagePath, 
					   string rtfPath, 
					   string nameOfBattle, 
					   DateTime date, 
					   string enemySide, 
					   string mNECommander, 
					   string enemyCommander, 
					   string mNEStrenght, 
					   string enemyStrenght, 
					   int result)
		{
			this.id = id;
			this.imagePath = imagePath;
			this.rtfPath = rtfPath;
			this.nameOfBattle = nameOfBattle;
			this.date = date;
			EnemySide = enemySide;
			MNECommander = mNECommander;
			EnemyCommander = enemyCommander;
			MNEStrenght = mNEStrenght;
			EnemyStrenght = enemyStrenght;
			Result = result;
		}

		public int Id { get => id; set => id = value; }
		public string ImagePath { get { return imagePath; } set {  imagePath = value; } }
		public string RtfPath { get { return rtfPath; } set {  rtfPath = value; } }
		public string NameOfBattle { get => nameOfBattle; set => nameOfBattle = value; }
		public DateTime Date { get => date; set => date = value; }
		public string EnemySide1 { get => EnemySide; set => EnemySide = value; }
		public string MNECommander1 { get => MNECommander; set => MNECommander = value; }
		public string EnemyCommander1 { get => EnemyCommander; set => EnemyCommander = value; }
		public string MNEStrenght1 { get => MNEStrenght; set => MNEStrenght = value; }
		public string EnemyStrenght1 { get => EnemyStrenght; set => EnemyStrenght = value; }
		public int Result1 { get => Result; set => Result = value; }
		
	}
}
