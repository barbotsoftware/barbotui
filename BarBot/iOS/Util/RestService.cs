using System;
using System.Net.Http;
using System.Threading.Tasks;
using BarBot.Core.Model;

namespace BarBot.iOS.Util
{
	public class RestService
	{
		HttpClient client;
		string HostName { get; set; }

		public RestService(string hostName)
		{
			client = new HttpClient();
			client.MaxResponseContentBufferSize = 256000;
			HostName = hostName;
		}

		public async Task<User> SaveUserNameAsync(string name)
		{
			try
			{
				var uri = new Uri("http://" + HostName + "/barbotweb/public/register?name=" + name);

				var content = new StringContent("");

				HttpResponseMessage response = await client.PostAsync(uri, content);

				if (response.IsSuccessStatusCode)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					if (responseContent.Contains("The name has already been taken"))
					{
						return new User("", "", "name_taken");
					}
					else {
						var u = new User(responseContent);
						return u;
					}
				}
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.ToString());
			}
			return new User("", "", "exception");
		}
	}
}
