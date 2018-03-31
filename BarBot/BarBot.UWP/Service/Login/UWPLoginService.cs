using BarBot.Core.Model;
using BarBot.Core.Service.Login;
using BarBot.Core.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web;
using Windows.Web.Http;

namespace BarBot.UWP.Service.Login
{
    public class UWPLoginService : ILoginService
    {
        private string endpoint;
        private HttpClient httpClient;

        public UWPLoginService(string endpoint)
        {
            this.endpoint = endpoint;
            this.httpClient = new HttpClient();
        }

        public Task<bool> ForgotPassword(string emailAddress)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> LoginUser(string type, string username, string password)
        {
            var uri = new Uri("http://" + this.endpoint + "/auth/login?type=" + type + "&username=" +
                  username + "&password=" + password);

            var request = new HttpRequestMessage(HttpMethod.Post, uri);

            try
            {
                HttpResponseMessage response = await httpClient.SendRequestAsync(request);
            
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<Message>(responseContent);

                    System.Diagnostics.Debug.WriteLine(responseContent.ToString());

                    if (json.Result == "success")
                    {
                        return true;
                    }
                    else if (json.Result == "error")
                    {
                        // Incorrect password or webserver error
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                WebErrorStatus error = WebError.GetStatus(ex.HResult);
                System.Diagnostics.Debug.WriteLine(error.ToString());
            }
            return false;
        }

        public Task<User> RegisterUser(string name, string emailAddress, string password)
        {
            throw new NotImplementedException();
        }
    }
}
