using Netder.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static Netder.Models.AuthResponseVM;
using static Netder.Models.MatchesVM;

namespace Netder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // get fbusrId and fbtoken
            string fbUserId = "";
            string fbToken = "";
            
            //Task.Run(async () =>
            //{
            //    await TinderAuth(fbUserId, fbToken);
            //}).GetAwaiter().GetResult();
            string authToken = TinderAuthentication(fbUserId, fbToken);

            //TinderChangeLocation("28.655521", "77.117608", authToken);
            
            if (!string.IsNullOrEmpty(authToken))
            {
                TinderGetMatches(authToken);
            }
        }

        public static void TinderChangeLocation(string lat, string lon, string authToken)
        {
            var client = new RestClient("https://api.gotinder.com");

            var request = new RestRequest("user/ping", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("User-Agent", "Tinder Android Version 6.4.1");
            request.AddHeader("X-Auth-Token", $"{authToken}");
            request.AddHeader("os_version", "1935");
            request.AddHeader("app-version", "371");
            request.AddHeader("platform", "android");
            request.AddHeader("Accept-Encoding", "gzip");

            LocationChangeVM location = new LocationChangeVM()
            {
                lat = lat,
                lon = lon
            };
            request.AddBody(location);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string jsonResponse = response.Content.ToString();
            }
        }

        public static void TinderGetMatches(string authToken)
        {
            // get a list of matches 

            var client = new RestClient("https://api.gotinder.com");

            var request = new RestRequest("user/recs", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("User-Agent", "Tinder Android Version 6.4.1");
            request.AddHeader("X-Auth-Token", $"{authToken}");
            request.AddHeader("os_version", "1935");
            request.AddHeader("app-version", "371");
            request.AddHeader("platform", "android");
            request.AddHeader("Accept-Encoding", "gzip");

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string jsonResponse = response.Content.ToString();
                MatchRootObject rootObject = JsonConvert.DeserializeObject<MatchRootObject>(jsonResponse);

                var matches = rootObject.results.ToList();

                List<string> matcheIds = new List<string>();

                foreach (var item in matches)
                {
                    if (item.gender == 1) // 0 = male, 1 = female
                    {
                        matcheIds.Add(item._id);
                    }
                    
                }
                int flag = 0;
                foreach (var matcheId in matcheIds)
                {
                    // call api to like this id
                    TinderLikeAMatche(matcheId, authToken);
                    flag++;
                    if (flag == matcheIds.Count)
                    {
                        TinderGetMatches(authToken); // not tested
                    }
                }
            }
            // loop through that list and call API which will swipe it right
            // once list size is zero, then call again API to get matches.
            // 
        }

        public static void TinderLikeAMatche(string matcheId, string authToken)
        {
            var client = new RestClient("https://api.gotinder.com");

            var request = new RestRequest($"/like/{matcheId}", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("User-Agent", "Tinder Android Version 6.4.1");
            request.AddHeader("X-Auth-Token", $"{authToken}");
            request.AddHeader("os_version", "1935");
            request.AddHeader("app-version", "371");
            request.AddHeader("platform", "android");
            request.AddHeader("Accept-Encoding", "gzip");

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string jsonResponse = response.Content.ToString();
                Console.WriteLine($"response by Like A Match");
            }
        }


        public static string TinderAuthentication(string fbUserId, string fbToken)
        {
            try
            {
                var client = new RestClient("https://api.gotinder.com");

                var request = new RestRequest("auth", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.AddHeader("User-Agent", "Tinder Android Version 6.4.1");
                request.AddHeader("os_version", "1935");
                request.AddHeader("app-version", "371");
                request.AddHeader("platform", "android");
                request.AddHeader("Accept-Encoding", "gzip");

                AuthRequestVM data = new AuthRequestVM()
                {
                    facebook_id = fbUserId,
                    facebook_token = fbToken
                };

                request.AddBody(data);
                string authToken = string.Empty;
                var response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string jsonResponse = response.Content.ToString();
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonResponse);
                    authToken = rootObject.token;
                }
                return authToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
