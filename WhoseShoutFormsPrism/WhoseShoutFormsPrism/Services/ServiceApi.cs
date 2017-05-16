using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WhoseShoutFormsPrism.Helpers;
using WhoseShoutFormsPrism.Models;
using WhoseShoutWebService.Models;

namespace WhoseShoutFormsPrism.Services
{
    public class ServiceApi
    {
        public string Token { get; set; }
        public string Url { get; set; } = "http://whoseshoutwebservice20170509074122.azurewebsites.net";

        private async Task<T> ReadAsAsync<T>(HttpContent content)
        {
            string contentString = await content.ReadAsStringAsync();

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(contentString);
        }
        private async Task<HttpResponseMessage> PostAsJsonAsync<T>(HttpClient client, string requestUri, T value)
        {
            string contentString = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            StringContent content = new System.Net.Http.StringContent(contentString, Encoding.UTF8, "application/json");

            return await client.PostAsync(requestUri, content);
        }

        private async Task<HttpResponseMessage> PatchAsJsonAsync<T>(HttpClient client, string requestUri, T value)
        {
            string contentString = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            StringContent content = new System.Net.Http.StringContent(contentString, Encoding.UTF8, "application/json");

            return await client.PatchAsync(requestUri, content);
        }

        private HttpClient NewHttpClient(System.Net.CookieContainer cookies = null)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                UseDefaultCredentials = true,
                AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip
            };

            if (cookies != null)
            {
                handler.UseCookies = true;
                handler.CookieContainer = cookies;
            }

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(Url)
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!String.IsNullOrEmpty(Token))
            {
                client.DefaultRequestHeaders.Add("Authorization-Token", Token);
            }

            return client;
        }

        public async Task<ShoutUserDto> GetShoutUserBySocial(ShoutUserDto user)
        {
            try
            {
                using (var client = NewHttpClient())
                {
                    var response = await client.GetAsync("api/shoutusers?socialId=" + user.ShoutSocialID + "&authType=" + user.AuthType);
                    response.EnsureSuccessStatusCode();
                    return await ReadAsAsync<ShoutUserDto>(response.Content);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ShoutUserDto> GetShoutUserByEmail(string email)
        {
            try
            {
                using (var client = NewHttpClient())
                {
                    var response = await client.GetAsync("api/shoutusers?email=" + email);
                    response.EnsureSuccessStatusCode();
                    return await ReadAsAsync<ShoutUserDto>(response.Content);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ShoutUserDto> PatchShoutUser(ShoutUserDto shoutUser)
        {
            try
            {
                using (var client = NewHttpClient())
                {

                    var response = await PatchAsJsonAsync(client, "api/shoutusers/" + shoutUser.ID, shoutUser);
                    response.EnsureSuccessStatusCode();
                    return await ReadAsAsync<ShoutUserDto>(response.Content);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> NewShoutUser(ShoutUserDto user)
        {
            try
            {
                using (var client = NewHttpClient())
                {
                    var response = await PostAsJsonAsync(client, "api/shoutusers", user);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task CreateGroupCommand(ShoutGroupDto group)
        {
            try
            {
                using (var client = NewHttpClient())
                {
                    var response = await PostAsJsonAsync(client, "api/shoutgroups", group);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception )
            {
                return;
            }
        }

        public async Task<List<ShoutGroupDto>> GetShoutGroups(string userId)
        {
            try
            {
                using (var client = NewHttpClient())
                {
                    var response = await client.GetAsync("api/shoutgroups?shoutuserid=" + userId);
                    response.EnsureSuccessStatusCode();
                    return await ReadAsAsync<List<ShoutGroupDto>>(response.Content);
                }
            }
            catch (Exception )
            {
                return null;
            }
        }

        public async Task<List<ShoutDto>> GetShoutsForGroup(string groupId)
        {
            try
            {
                using (var client = NewHttpClient())
                {
                    var response = await client.GetAsync("api/shouts?groupid=" + groupId);
                    response.EnsureSuccessStatusCode();
                    return await ReadAsAsync<List<ShoutDto>>(response.Content);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        

        public async Task NewShout(ShoutDto shout)
        {
            try
            {
                using (var client = NewHttpClient())
                {
                    var response = await PostAsJsonAsync(client, "api/shouts", shout);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch(Exception)
            {
                return;
            }
        }
    }
}
