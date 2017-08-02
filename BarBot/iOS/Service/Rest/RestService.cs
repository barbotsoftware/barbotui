/*
 * RestService.cs
 * BarBot.iOS
 *
 * Created by Naveen Yadav on 07/19/17.
 * Copyright © 2017 BarBot Inc. All rights reserved.
 * 
 * This class uses the iOS ModernHttpClient implementation of
 * HttpClient to implement the IRestService interface.
 * Its purpose is to consume RESTful web services via HTTP.
 */

using System;
using System.Net.Http;
using System.Threading.Tasks;

using ModernHttpClient;

using BarBot.Core.Model;
using BarBot.Core.Service.Rest;

namespace BarBot.iOS.Service.Rest
{
    public class RestService : IRestService
    {
		HttpClient httpClient;
		string Host { get; set; }

        public RestService(string host)
        {
			httpClient = new HttpClient(new NativeMessageHandler());
			httpClient.MaxResponseContentBufferSize = 256000;

            // Set Host
            if (!host.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                Host = "http://" + host;
            }
            else
            {
                Host = host;
            }
        }

        /*
         * Registers a new User.
         */
        public async Task<User> RegisterUser(string name, string email, string password)
        {
            try
            {
				var uri = new Uri("http://" + Host + "/auth/register?name=" + name + "&email=" + email + "&password=" + password);

				var content = new StringContent("");

				HttpResponseMessage response = await httpClient.PostAsync(uri, content);

				if (response.IsSuccessStatusCode)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					if (responseContent.Contains("The name has already been taken"))
					{
						return new User("", "name_taken", "", "");
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
            return new User("", "exception", "", "");
        }

        /*
         * Logs in an existing User.
         */
        public async Task<bool> LoginUser(string email, string password)
        {
            try
            {
                var uri = new Uri("http://" + Host + "/auth/login?email=" + email + "&password=" + password);

                var content = new StringContent("");

                HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // check response content

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            return false;
        }

		public async Task<byte[]> LoadImage(string imageUrl)
		{
			try
			{
				// await! control returns to the caller and the task continues to run on another thread
				var contents = await httpClient.GetByteArrayAsync("http://" + Host + "/" + imageUrl);

				// return byte[]
				return contents;
			}
			catch (HttpRequestException e)
			{
				System.Diagnostics.Debug.WriteLine(e.ToString());
			}
			return null;
		}

		/*
         * Logs out an existing User.
         */
		//public async Task<bool> LogoutUser(User user)
		//{
		//    return false;
		//}
	}
}
