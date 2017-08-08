namespace BarBot.Core.Model
{
	public class User : JsonModelObject
	{
        string userId;
		string name;
        string emailAddress;
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

        public string EmailAddress
		{
			get
            {
                return emailAddress; 
            }

			set
			{
				emailAddress = value;
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

        public User(string userId, string name, string emailAddress, string password)
		{
            UserId = userId;
            Name = name;
			EmailAddress = emailAddress;
            Password = password;
		}

        public User(string json)
        {
			var u = (User)parseJSON(json, typeof(User));
            UserId = u.UserId;
            Name = u.Name;
            EmailAddress = u.EmailAddress;
            Password = u.Password;
		}
	}
}
