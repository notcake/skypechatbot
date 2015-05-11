using System.Text.RegularExpressions;
using Eka.Web.Vine;

namespace ChatBot.MessageSpanHandlers
{
    public class VineUrlSpanHandler : IMessageSpanHandler
    {
        private readonly string[] VineUrls =
        {
            "https?://(www\\.)?vine\\.co/v/([a-zA-Z0-9_]+)"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (var vineUrlPattern in VineUrls)
            {
                foreach (Match match in new Regex(vineUrlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, null);
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            var vineid = actionSpan.Match.Groups[2].ToString();

            var vine = new Vine(vineid);

            messageSink("Vine: " + vine.Title + " By: " + vine.Author);
        }
    }
}