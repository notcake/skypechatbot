﻿using System.Net;
using System.Text.RegularExpressions;

namespace ChatBot.MessageSpanHandlers
{
    public class ShortenedUrlSpanHandler : IMessageSpanHandler
    {
        private readonly string[] UrlShorteners =
        {
            "https?://(www\\.)?goo.gl/([a-zA-Z0-9_\\-]+)",
            "https?://(www\\.)?redd.it/([a-zA-Z0-9_\\-]+)",
            "https?://(www\\.)?bit.ly/([a-zA-Z0-9_\\-]+)",
            "https?://(www\\.)?tinyurl.com/([a-zA-Z0-9_\\-]+)",
            "https?://(www\\.)?t.co/([a-zA-Z0-9_\\-]+)",
            "https?://(www\\.)?db.tt/([a-zA-Z0-9_\\-]+)"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (var urlPattern in UrlShorteners)
            {
                foreach (Match match in new Regex(urlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, match.ToString());
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            var httpRequest = WebRequest.Create(actionSpan.Data);
            var httpResponse = httpRequest.GetResponse();

            messageSink("URL Redirect: " + httpResponse.ResponseUri.AbsoluteUri);
        }
    }
}