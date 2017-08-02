using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Netder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // get fbusrId and fbtoken

            string fbUserId = "";
            string fbToken = "";

            // make http request for auth token, use all headers and correct url
            // get response 

            Task.Run(async () =>
            {
                await TinderAuth(fbUserId, fbToken);
            }).GetAwaiter().GetResult();

        }

        public static async Task TinderAuth(string fbUserId, string fbToken)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string requestPath = string.Format("/auth");
                    httpClient.BaseAddress = new Uri("https://api.gotinder.com");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Tinder Android Version 6.4.1");
                    httpClient.DefaultRequestHeaders.Add("os_version", "1935");
                    httpClient.DefaultRequestHeaders.Add("app-version", "371");
                    httpClient.DefaultRequestHeaders.Add("platform", "android");
                    httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
                    //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + "");

                    JObject jObject = new JObject(new JProperty("facebook_id", fbUserId), new JProperty("facebook_token", fbToken));

                    HttpContent content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(requestPath, content);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        string rawResponse = response.Content.ReadAsStringAsync().Result;
                       //JObject resJsonObject = JObject.Parse(responseString);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
