using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Eka.Web.MusicBrainz;
using Eka.Web.Pastebin;
using Eka.Web.Twitter;
using Eka.Web.Wikipedia;
using Eka.Web.YouTube;

namespace ChatBot.MessageSpanHandlers
{
    public class GenericUrlSpanHandler : IMessageSpanHandler
    {
        private string[] Urls = new string[]
        {
            "https?://(www\\.)?xkcd\\.com(/\\d+)?/?",
            "https?://(www\\.)?facepunch\\.com/showthread\\.php\\?([^ ]+)?t=(\\d{7})",
            "https?://(www\\.)?facepunch\\.com/member\\.php\\?([^ ]+)?u=\\d+",
            "https?://(www\\.)?reddit\\.com/[a-zA-Z0-9/_]+",
            "https?://pay.reddit\\.com/[a-zA-Z0-9/_]+",
            "https?://(www\\.)?bbc\\.co\\.uk/news/.+",
            "https?://(www\\.)?newgrounds.com/portal/view/\\d+",
            
            
            // Steam
            "https?://(www\\.)?store\\.steampowered\\.com/app/(\\d+)",
            "https?://(www\\.)?forums\\.steampowered\\.com/forums/showthread\\.php\\?t=(\\d{7})",
            "https?://(www\\.)?steamcommunity\\.com/sharedfiles/filedetails.+",
            "https?://(www\\.)?steamcommunity\\.com/id/([a-zA-Z0-9_-]+)(/recommended/\\d+)?",
            "https?://(www\\.)?steamcommunity\\.com/profiles/(\\d+)(/recommended/\\d+)?",
            "https?://(www\\.)?steamcommunity\\.com/gid/(\\d+)",
            "https?://(www\\.)?steamcommunity\\.com/discussions/forum/([0-9/]+)",
            "https?://(www\\.)?steamcommunity\\.com/(app/\\d+|groups/[a-zA-Z0-9_-]+|workshop/filedetails)(/discussions?/\\d+/\\d+|[a-zA-Z0-9_-]+)",

            // Soundclound
            "https?://(www\\.)?(soundcloud.com|snd.cc)/.+",
            // osu
            "https?://(www\\.)?(osu.ppy.sh)/(b|u|forum/t|s)/([a-zA-Z0-9_-]+)",
            // imgur
            "https?://(www\\.)?(imgur\\.com)/gallery/([a-zA-Z0-9_-]+)",
            "https?://(www\\.)?(imgur\\.com)/a/([a-zA-Z0-9_-]+)"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (string urlPattern in this.Urls)
            {
                foreach (Match match in new Regex(urlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, match.ToString());
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            string url = actionSpan.Data;
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string html = webClient.DownloadString(url);
            if (html == null) { return; }

            Match match = new Regex("<title>([^<]+)</title>", RegexOptions.IgnoreCase).Match(html);
            if (match == null) { return; }

            messageSink(WebUtility.HtmlDecode(match.Groups[1].ToString()));
        }
    }
}