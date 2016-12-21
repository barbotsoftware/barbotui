using Foundation;
using UIKit;
using System.Threading.Tasks;
using System.Net.Http;

namespace BarBot.iOS
{
	public class AsyncUtil
	{
		public AsyncUtil()
		{
		}

		public static async Task<UIImage> LoadImage(string imageUrl)
		{
			var httpClient = new HttpClient();

			// await! control returns to the caller and the task continues to run on another thread
			var contents = await httpClient.GetByteArrayAsync(imageUrl);

			// load from bytes
			return UIImage.LoadFromData(NSData.FromArray(contents));
		}
	}
}
