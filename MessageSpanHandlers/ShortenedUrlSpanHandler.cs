using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatBot.MessageSpanHandlers
{
    public class ShortenedUrlSpanHandler : IMessageSpanHandler
    {
        private string[] UrlShorteners = new string[]
        {
            "https?://(www\\.)?goo.gl/([a-zA-Z0-9_\\-]+)",
            "https?://(www\\.)?redd.it/([a-zA-Z0-9_\\-]+)",
            "https?://(www\\.)?bit.ly/([a-zA-Z0-9_\\-]+)",
            "https?://(www\\.)?tinyurl.com/([a-zA-Z0-9_\\-]+)"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (string urlPattern in this.UrlShorteners)
            {
                foreach (Match match in new Regex(urlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, match.ToString());
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            WebRequest httpRequest = WebRequest.Create(actionSpan.Data);
            WebResponse httpResponse = httpRequest.GetResponse();

            messageSink("URL Redirect: " + httpResponse.ResponseUri.AbsoluteUri);
        }
    }
}
