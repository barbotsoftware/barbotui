using System.Threading.Tasks;
using System.Net.Http;
using BarBot.Core;

namespace BarBot.iOS.Util
{
	public class AsyncUtil
	{
		public string IPAddress { get; set; }

		public AsyncUtil()
		{
		}

		public async Task<byte[]> LoadImage(string imageUrl)
		{
			var httpClient = new HttpClient();

			// await! control returns to the caller and the task continues to run on another thread
			var contents = await httpClient.GetByteArrayAsync("http://" + IPAddress + "/" + imageUrl);

			// return byte[]
			return contents;
		}
	}
}
