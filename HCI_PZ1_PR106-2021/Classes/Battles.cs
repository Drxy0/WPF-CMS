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
		string text;
		string imagePath;
		string rtfPath;

		public Battles() { }
		public int Id { get; set; }
		public string Text { get { return text; } set { text = value; } }
		public string ImagePath { get { return imagePath; } set {  imagePath = value; } }
		public string RtfPath { get { return rtfPath; } set {  rtfPath = value; } }
	}
}
