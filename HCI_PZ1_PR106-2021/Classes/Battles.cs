using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_PZ1_PR106_2021.Classes
{
	public class Battles
	{
		int count = 0;
		string text;
		string imagePath;
		string rtfPath;

		public int Count { get; set; }
		public string Text { get { return text; } set { text = value; } }
		public string ImagePath { get { return imagePath; } set {  imagePath = value; } }
		public string RtfPath { get { return rtfPath; } set {  rtfPath = value; } }
	}
}
