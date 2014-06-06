using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication1
{
    public static class GetMessage
    {
        

        public static void GetMessagesForUser(string screename)
        {
            Authenticate auth = new Authenticate();

            var screenName = screename;
            var timeLineFormat = "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}&include_rts=1&exclude_replies=1&count=0";
            var timeLineUrl = string.Format(timeLineFormat, screenName);

            var o = auth.DoGetAuthenticate(timeLineUrl);
            for (int x = 0; x < o.Count(); x++)
            {
                JObject tweet = JObject.Parse(o[x].ToString());
                var tweetText = tweet["text"].ToString();
                Console.WriteLine(tweetText);
            }
        }

        public static void GetMessageForUserWithHashtag(string screenname, string hashtag)
        {
            Authenticate auth = new Authenticate();
            if (!hashtag.Contains('#'))
            {
                Console.WriteLine("Hashtag does not contain a \"#\"");
                return;
            }
            var screenName = screenname;
            var hashTag = hashtag;
            var timeLineFormat = "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}&include_rts=1&exclude_replies=1&count=5";
            var timeLineUrl = string.Format(timeLineFormat, screenName);
            var o = auth.DoGetAuthenticate(timeLineUrl);

            for (int x = 0; x < o.Count(); x++)
            {
                JObject tweet = JObject.Parse(o[x].ToString());
                var tweetText = tweet["text"].ToString();
                if (tweetText.Contains(hashtag))
                {
                    Console.WriteLine(tweetText);
                }
            }
        }
    }
    
    
}
