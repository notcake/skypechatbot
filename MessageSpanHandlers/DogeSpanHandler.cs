using System;
using System.Text.RegularExpressions;
using Eka.Web.Thesaurus;

namespace ChatBot.MessageSpanHandlers
{
    public class DogeSpanHandler : IMessageSpanHandler
    {
        private readonly string[] DogeRegEx =
        {
            "so", "much", "very", "such"
        };

        private readonly Random rnd = new Random();
        private DateTime lastdoge = DateTime.Now;

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            var pattern_prefixes = "";
            var first = true;

            foreach (var prefix in DogeRegEx)
            {
                if (first)
                    first = false;
                else
                    pattern_prefixes += "|";
                pattern_prefixes += prefix;
            }

            var pattern = "^(" + pattern_prefixes + ") ([a-zA-Z]+)$";

            foreach (Match match in new Regex(pattern, RegexOptions.IgnoreCase).Matches(message))
            {
                actionSpanSink(match, null);
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            if (DateTime.Now.Subtract(lastdoge).Seconds <= 10)
            {
                var prefix = actionSpan.Match.Groups[1].ToString();
                var word = actionSpan.Match.Groups[2].ToString();

                var thesaurus = new Thesaurus(word);

                if (thesaurus.Success)
                {
                    var message = "";

                    foreach (var p in DogeRegEx)
                    {
                        if (thesaurus.Synonyms.Count == 0) break;
                        if (p != prefix)
                        {
                            var pos = rnd.Next(thesaurus.Synonyms.Count - 1);
                            message += p + " " + thesaurus.Synonyms[pos] + "\n";
                            thesaurus.Synonyms.RemoveAt(pos);
                        }
                    }

                    messageSink(message);
                }
            }

            lastdoge = DateTime.Now;
        }
    }
}