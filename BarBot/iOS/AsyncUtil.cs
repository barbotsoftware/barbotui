﻿using System.Threading.Tasks;
using System.Net.Http;

namespace BarBot.iOS
{
	public class AsyncUtil
	{
		public AsyncUtil()
		{
		}

		public static async Task<byte[]> LoadImage(string imageUrl)
		{
			var httpClient = new HttpClient();

			// await! control returns to the caller and the task continues to run on another thread
			var contents = await httpClient.GetByteArrayAsync(imageUrl);

			// return byte[]
			return contents;
		}
	}
}
