using System.Text.RegularExpressions;
using Eka.Web.Twitter;

namespace ChatBot.MessageSpanHandlers
{
    public class TwitterUrlSpanHandler : IMessageSpanHandler
    {
        private readonly string[] TwitterUrls =
        {
            "https?://(www\\.)?twitter\\.com/([a-zA-Z0-9_]+)/status/([0-9]+)"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (var twitterUrlPattern in TwitterUrls)
            {
                foreach (Match match in new Regex(twitterUrlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, null);
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            var username = actionSpan.Match.Groups[2].ToString();
            var tweetId = ulong.Parse(actionSpan.Match.Groups[3].ToString());

            var tweet = new Tweet(tweetId);
            if (!tweet.Valid)
            {
                return;
            }

            var ttext = Regex.Replace(tweet.Text, "(pic\\.twitter\\.com/[a-zA-Z0-9_]+)", "http://$1");

            messageSink("Twitter: " + username + ": " + ttext + "\n        Posted: " +
                        tweet.Date.ToString("dd MMMM yyyy"));
        }
    }
}