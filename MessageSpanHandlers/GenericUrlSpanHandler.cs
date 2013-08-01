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
            "https?://(www\\.)?facepunch.com/showthread.php\\?.*t=(\\d{7})",
	
            // Steam
            "https?://(www\\.)?store.steampowered\\.com/app/(\\d+)",
            "https?://(www\\.)?steamcommunity\\.com/sharedfiles/filedetails.+",
            "https?://(www\\.)?steamcommunity\\.com/app/(\\d+)([a-zA-Z0-9/]*)",
            "https?://(www\\.)?steamcommunity\\.com/id/([a-zA-Z0-9_]+)",
            "https?://(www\\.)?steamcommunity\\.com/profiles/(\\d+)",
            "https?://(www\\.)?steamcommunity\\.com/groups/([a-zA-Z0-9_]+)",
            "https?://(www\\.)?steamcommunity\\.com/gid/(\\d+)",
            "https?://(www\\.)?steamcommunity\\.com/discussions/forum/([0-9/]+)"
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
            string html = new WebClient().DownloadString(url);
            if (html == null) { return; }

            Match match = new Regex("<title>([^<]+)</title>", RegexOptions.IgnoreCase).Match(html);
            if (match == null) { return; }

            messageSink(match.Groups[1].ToString());
        }
    }
}
