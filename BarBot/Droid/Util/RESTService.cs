using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Android.Graphics;

using ModernHttpClient;

using BarBot.Core.Model;

namespace BarBot.Droid.Util
{
	public class RESTService
	{
		HttpClient httpClient;
		string HostName { get; set; }

		public RESTService(string hostName)
		{
			httpClient = new HttpClient(new NativeMessageHandler());
			httpClient.MaxResponseContentBufferSize = 256000;
			HostName = hostName;
		}

		public Bitmap GetImageBitmapFromUrl(string imageUrl)
		{
			Bitmap imageBitmap = null;
			var url = "http://" + HostName + "/" + imageUrl;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
		}

		//public async Task<byte[]> LoadImage(string imageUrl)
		//{
		//	try
		//	{
		//		var uri = new Uri("http://" + HostName + "/" + imageUrl);

		//		// await! control returns to the caller and the task continues to run on another thread
		//		var contents = await httpClient.DownloadDataTaskAsync(uri);

		//		// return byte[]
		//		return contents;
		//	}
		//	catch (HttpRequestException e)
		//	{
		//		System.Diagnostics.Debug.WriteLine(e.ToString());
		//	}
		//	return null;
		//}

		public async Task<User> SaveUserNameAsync(string name)
		{
			try
			{
				var uri = new Uri("http://" + HostName + "/barbotweb/public/register?name=" + name);

				var content = new StringContent("");

				HttpResponseMessage response = await httpClient.PostAsync(uri, content);

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
