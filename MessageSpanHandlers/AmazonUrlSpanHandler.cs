using System.Text.RegularExpressions;
using Eka.Web.Amazon;

namespace ChatBot.MessageSpanHandlers
{
    public class AmazonUrlSpanHandler : IMessageSpanHandler
    {
        private readonly string[] AmazonUrls =
        {
            "https?://(www\\.)?amazon\\.(com|de|co\\.uk)([^ ]+)?/[dg]p/(product/)?[a-zA-Z0-9]+"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (var amazonUrlPattern in AmazonUrls)
            {
                foreach (Match match in new Regex(amazonUrlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, null);
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            var amazon = new Amazon(actionSpan.Match.ToString());

            if (amazon.Success)
            {
                messageSink("Amazon: " + amazon.Title + "\n\t" + amazon.Price);
            }
        }
    }
}