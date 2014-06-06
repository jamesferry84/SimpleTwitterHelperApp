using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script;
using System.Web.Script.Serialization;

namespace ConsoleApplication1
{
    public class Authenticate
    {
        string oAuthConsumerKey = "";
        string oAuthConsumerSecret = "";
        string oauth_token = "";
        string oauth_token_secret = "";
        string oAuthUrl = "https://api.twitter.com/oauth2/token";
        JArray jArrayObject;
        public JArray DoGetAuthenticate(string urlQuery)
        {
            var authHeaderFormat = "Basic {0}";
            var authHeader = string.Format(authHeaderFormat, Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(oAuthConsumerKey) + ":" +
                Uri.EscapeDataString((oAuthConsumerSecret)))));
            var postBody = "grant_type=client_credentials";

            HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(oAuthUrl);
            authRequest.Headers.Add("Authorization", authHeader);
            authRequest.Method = "POST";
            authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (Stream stream = authRequest.GetRequestStream())
            {
                byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }

            authRequest.Headers.Add("Accept-Encoding", "gzip");

            WebResponse authResponse = authRequest.GetResponse();

            TwitAuthenticateResponse twitAuthResponse;

            using (authResponse)
            {
                using (var reader = new StreamReader(authResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var objectText = reader.ReadToEnd();
                    twitAuthResponse = JsonConvert.DeserializeObject<TwitAuthenticateResponse>(objectText);
                }
            }

            HttpWebRequest timeLineRequest = (HttpWebRequest)WebRequest.Create(urlQuery);
            var timeLineHEaderFormat = "{0} {1}";
            timeLineRequest.Headers.Add("Authorization", string.Format(timeLineHEaderFormat, twitAuthResponse.Token_Type, twitAuthResponse.Access_Token));
            timeLineRequest.Method = "Get";
            WebResponse timeLineResponse = timeLineRequest.GetResponse();
            var timeLineJson = string.Empty;
            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    timeLineJson = reader.ReadToEnd();
                    var results = JsonConvert.DeserializeObject<dynamic>(timeLineJson);
                    jArrayObject = JArray.Parse(timeLineJson);
                }
            }

            return jArrayObject;
        }
    }

    public class TwitAuthenticateResponse
    {
        public string Token_Type { get; set; }
        public string Access_Token { get; set; }
    }


}
