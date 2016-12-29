using System;
using System.Net.Http;
using System.Threading.Tasks;
using BarBot.Core.Model;

namespace BarBot.iOS.Util
{
	public class RestService
	{
		HttpClient client;
		string EndPoint { get; set; }

		public RestService(string endpoint)
		{
			client = new HttpClient();
			client.MaxResponseContentBufferSize = 256000;
			EndPoint = endpoint;
		}

		public async Task<User> SaveUserNameAsync(string name)
		{
			var uri = new Uri("http://" + EndPoint + "/barbotweb/public/register?name=" + name);

			var content = new StringContent("");

			HttpResponseMessage response = await client.PostAsync(uri, content);

			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				if (responseContent.Contains("The name has already been taken"))
				{
					return null;
				}
				else {
					var u = new User(responseContent);
					return u;
				}
			}
			return null;
		}
	}
}
