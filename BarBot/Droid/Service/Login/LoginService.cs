using System;
using System.Net.Http;
using System.Threading.Tasks;

using BarBot.Core.Model;
using BarBot.Core.Service.Login;

namespace BarBot.Droid.Service.Login
{
    public class LoginService : ILoginService
    {
        HttpClient httpClient;
        string Host { get; set; }

        public LoginService(string host)
        {
            httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            Host = host;
        }

        public async Task<User> RegisterUser(string name, string email, string password)
        {
            try
            {
                var uri = new Uri(Host + "/auth/register?name=" + name + "&email=" + email + "&password=" + password);

                var content = new StringContent("");

                HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    System.Diagnostics.Debug.WriteLine(responseContent);

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
        public async Task<bool> LoginUser(string type, string username, string password)
        {
            try
            {
                var uri = new Uri(Host + "/auth/login?type=" + type + "&username" + username + "&password=" + password);

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

        public async Task<bool> ForgotPassword(string emailAddress)
        {
            try
            {
                var uri = new Uri(Host + "/auth/resetpassword?email=" + emailAddress);

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
    }
}
