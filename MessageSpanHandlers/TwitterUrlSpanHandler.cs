using Eka.Web.Twitter;
using System.Text.RegularExpressions;

namespace ChatBot.MessageSpanHandlers
{
    public class TwitterUrlSpanHandler : IMessageSpanHandler
    {
        private string[] TwitterUrls = new string[]
        {
            "https?://(www\\.)?twitter\\.com/([a-zA-Z0-9_]+)/status/([0-9]+)"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (string twitterUrlPattern in this.TwitterUrls)
            {
                foreach (Match match in new Regex(twitterUrlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, null);
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            string username = actionSpan.Match.Groups[2].ToString();
            ulong tweetId = ulong.Parse(actionSpan.Match.Groups[3].ToString());

            Tweet tweet = new Tweet(tweetId);
            if (!tweet.Valid) { return; }

            messageSink("Twitter: " + username + ": " + tweet.Text + "\n        Posted: " + tweet.Date.ToString("dd MMMM yyyy"));
        }
    }
}