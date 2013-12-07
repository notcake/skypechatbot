using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eka.Web.Amazon;

namespace ChatBot.MessageSpanHandlers
{
    public class AmazonUrlSpanHandler : IMessageSpanHandler
    {
        private string[] AmazonUrls = new string[]
        {
            "https?://(www\\.)?amazon\\.(com|de|co\\.uk)([^ ]+)?/[dg]p/(product/)?[a-zA-Z0-9]+",
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (string amazonUrlPattern in this.AmazonUrls)
            {
                foreach (Match match in new Regex(amazonUrlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, null);
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            Amazon amazon = new Amazon(actionSpan.Match.ToString());

            if (amazon.Success)
            {
                messageSink("Amazon: " + amazon.Title + "\n\t" + amazon.Price + ' ' + amazon.Currency);
            }
        }
    }
}
