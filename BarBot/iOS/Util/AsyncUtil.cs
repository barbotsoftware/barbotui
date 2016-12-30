using System.Threading.Tasks;
using System.Net.Http;
using System;

namespace BarBot.iOS.Util
{
	public class AsyncUtil
	{
		public string HostName { get; set; }

		public AsyncUtil()
		{
		}

		public async Task<byte[]> LoadImage(string imageUrl)
		{
			var httpClient = new HttpClient();

			try
			{
				// await! control returns to the caller and the task continues to run on another thread
				var contents = await httpClient.GetByteArrayAsync(HostName + "/" + imageUrl);

				// return byte[]
				return contents;
			}
			catch (HttpRequestException e)
			{
				System.Diagnostics.Debug.WriteLine(e.ToString());
			}
			return null;
		}
	}
}
