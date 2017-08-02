namespace BarBot.Core.Model
{
	public class User : JsonModelObject
	{
        string userId;
		string name;
        string email;
        string password;

		public string UserId
		{
			get
            {
                return userId;
            }
			set
			{
				userId = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public string Email
		{
			get
            {
                return email; 
            }

			set
			{
				email = value;
			}
		}

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public User()
        {
        }

        public User(string userId, string name, string email, string password)
		{
            UserId = userId;
            Name = name;
			Email = email;
            Password = password;
		}

        public User(string json)
        {
			var u = (User)parseJSON(json, typeof(User));
            UserId = u.UserId;
            Name = u.Name;
            Email = u.Email;
            Password = u.Password;
		}
	}
}
