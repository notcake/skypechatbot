using System.Text.RegularExpressions;
using Eka.Web.Pastebin;

namespace ChatBot.MessageSpanHandlers
{
    public class PastebinUrlSpanHandler : IMessageSpanHandler
    {
        private readonly string[] PastebinUrls =
        {
            "https?://(www\\.)?pastebin\\.com/raw.php?i=([a-zA-Z0-9_]+)",
            "https?://(www\\.)?pastebin\\.com/([a-zA-Z0-9_]+)"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (var pastebinUrlPattern in PastebinUrls)
            {
                foreach (Match match in new Regex(pastebinUrlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, match.Groups[2].ToString());
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            var paste = new Paste(actionSpan.Data);

            if (!paste.Exists)
            {
                return;
            }

            var lines = paste.Data.Substring(0, 256).Split('\n');

            var code = "";
            uint lineCount = 0;
            foreach (var line in lines)
            {
                if (line.Trim().Length == 0)
                {
                    continue;
                }

                code = code + "\n        " + line.Replace("\t", "        ");
                lineCount++;

                if (lineCount >= 3)
                {
                    break;
                }
                if (code.Length > 256)
                {
                    break;
                }
            }

            messageSink("Pastebin:" + code);
        }
    }
}