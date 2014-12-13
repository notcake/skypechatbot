using Eka.Web.Vimeo;
using System.Text.RegularExpressions;

namespace ChatBot.MessageSpanHandlers
{
    public class VimeoUrlSpanHandler : IMessageSpanHandler
    {
        private string[] VimeoUrls = new string[]
        {
            "https?://(www\\.)?vimeo\\.com/([0-9]+)"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (string vimeoUrlPattern in this.VimeoUrls)
            {
                foreach (Match match in new Regex(vimeoUrlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, match.Groups[2].ToString());
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            string videoId = actionSpan.Data;

            VideoInfo videoInfo = new VideoInfo(ulong.Parse(videoId));

            string formattedDuration = null;
            if (videoInfo.Duration.TotalHours < 1)
            {
                formattedDuration = videoInfo.Duration.Minutes.ToString("D2") + ":" + videoInfo.Duration.Seconds.ToString("D2");
            }
            else
            {
                formattedDuration = ((int)videoInfo.Duration.TotalHours).ToString("D2") + ":" + videoInfo.Duration.Minutes.ToString("D2") + ":" + videoInfo.Duration.Seconds.ToString("D2");
            }

            string videoInfoMessage = "Vimeo: " + videoInfo.Title + " [" + formattedDuration + "]";

            messageSink(videoInfoMessage);
        }
    }
}