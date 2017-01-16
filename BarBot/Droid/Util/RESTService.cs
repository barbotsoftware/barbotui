using System;
using System.Net.Http;
using System.Threading.Tasks;

using ModernHttpClient;

using BarBot.Core.Model;

namespace BarBot.Droid.Util
{
	public class RESTService
	{
		HttpClient client;
		string HostName { get; set; }

		public RESTService(string hostName)
		{
			client = new HttpClient(new NativeMessageHandler());
			client.MaxResponseContentBufferSize = 256000;
			HostName = hostName;
		}

		public async Task<byte[]> LoadImage(string imageUrl)
		{
			try
			{
				// await! control returns to the caller and the task continues to run on another thread
				var contents = await client.GetByteArrayAsync("http://" + HostName + "/" + imageUrl);

				// return byte[]
				return contents;
			}
			catch (HttpRequestException e)
			{
				System.Diagnostics.Debug.WriteLine(e.ToString());
			}
			return null;
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
					else
					{
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
