using System.Text.RegularExpressions;
using Eka.Web.Vimeo;

namespace ChatBot.MessageSpanHandlers
{
    public class VimeoUrlSpanHandler : IMessageSpanHandler
    {
        private readonly string[] VimeoUrls =
        {
            "https?://(www\\.)?vimeo\\.com/([0-9]+)"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (var vimeoUrlPattern in VimeoUrls)
            {
                foreach (Match match in new Regex(vimeoUrlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, match.Groups[2].ToString());
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            var videoId = actionSpan.Data;

            var videoInfo = new VideoInfo(ulong.Parse(videoId));

            string formattedDuration = null;
            if (videoInfo.Duration.TotalHours < 1)
            {
                formattedDuration = videoInfo.Duration.Minutes.ToString("D2") + ":" +
                                    videoInfo.Duration.Seconds.ToString("D2");
            }
            else
            {
                formattedDuration = ((int) videoInfo.Duration.TotalHours).ToString("D2") + ":" +
                                    videoInfo.Duration.Minutes.ToString("D2") + ":" +
                                    videoInfo.Duration.Seconds.ToString("D2");
            }

            var videoInfoMessage = "Vimeo: " + videoInfo.Title + " [" + formattedDuration + "]";

            messageSink(videoInfoMessage);
        }
    }
}