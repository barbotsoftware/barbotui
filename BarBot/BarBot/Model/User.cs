using System.ComponentModel;

namespace BarBot.Core.Model
{
	public class User : JsonModelObject, INotifyPropertyChanged
	{
		private string _email;
		private string _name;
		private string _uid;

		public string Email
		{
			get { return _email; }
			set
			{
				_email = value;
				OnPropertyChanged("Email");
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged("Name");
			}
		}

		public string Uid
		{
			get { return _uid; }
			set
			{
				_uid = value;
				OnPropertyChanged("Uid");
			}
		}

		public User()
		{
		}

		public User(string email, string name, string uid)
		{
			Email = email;
			Name = name;
			Uid = uid;
		}

		public User(string json)
		{
			var u = (User)parseJSON(json, typeof(User));
			Email = u.Email;
			Name = u.Name;
			Uid = u.Uid;
		}
	}
}
