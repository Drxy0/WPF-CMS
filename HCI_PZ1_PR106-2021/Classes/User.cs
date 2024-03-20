using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace HCI_PZ1_PR106_2021.Classes
{
	public class User
	{
		public enum UserRole { Visitor, Admin }
		private string username;
		private string password;
		private UserRole role;
		public User() { }
		public User(UserRole role,  string username, string password)
		{
			this.role = role;
			this.username = username;
			this.password = password;
		}
		#region properties
		public string Username {  get { return username; } set {  username = value; } }
		public string Password { get { return password; } set {  password = value; } }
		public UserRole Role { get { return role; } set { role = value; } }
		#endregion
		private List<User> LoadUsers()
		{
			string workingDirectory = Environment.CurrentDirectory;
			string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
			string usersPath = projectDirectory + "\\" + "users.xml";
			List<User> users = new List<User>();

			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(usersPath);
				XmlElement root = xmlDoc.DocumentElement;
				foreach (XmlNode node in root.SelectNodes("user"))
				{
					string userRole = node.SelectSingleNode("userRole").InnerText;
					string username = node.SelectSingleNode("username").InnerText;
					string password = node.SelectSingleNode("password").InnerText;

					if (userRole == "Admin")
					{
						users.Add(new User(User.UserRole.Admin, username, password));
					}
					else
					{
						users.Add(new User(User.UserRole.Visitor, username, password));
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
			return users;
		}

		public string CheckLogin(string username, string password)
		{
			List<User> users = LoadUsers();
			foreach (User user in users)
			{
				if (user.Username == username && user.Password == password && user.Role == User.UserRole.Admin)
					return "Admin";
				else if (user.Username == username && user.Password == password && user.Role == User.UserRole.Visitor)
					return "Visitor";
			}
			return string.Empty;
		}
	}
}
