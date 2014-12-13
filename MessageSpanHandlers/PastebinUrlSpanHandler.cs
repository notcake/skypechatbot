using Eka.Web.Pastebin;
using System.Text.RegularExpressions;

namespace ChatBot.MessageSpanHandlers
{
    public class PastebinUrlSpanHandler : IMessageSpanHandler
    {
        private string[] PastebinUrls = new string[]
        {
            "https?://(www\\.)?pastebin\\.com/raw.php?i=([a-zA-Z0-9_]+)",
            "https?://(www\\.)?pastebin\\.com/([a-zA-Z0-9_]+)"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (string pastebinUrlPattern in this.PastebinUrls)
            {
                foreach (Match match in new Regex(pastebinUrlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, match.Groups[2].ToString());
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            Paste paste = new Paste(actionSpan.Data);

            if (!paste.Exists) { return; }

            string[] lines = paste.Data.Substring(0, 256).Split('\n');

            string code = "";
            uint lineCount = 0;
            foreach (string line in lines)
            {
                if (line.Trim().Length == 0) { continue; }

                code = code + "\n        " + line.Replace("\t", "        ");
                lineCount++;

                if (lineCount >= 3) { break; }
                if (code.Length > 256) { break; }
            }

            messageSink("Pastebin:" + code);
        }
    }
}