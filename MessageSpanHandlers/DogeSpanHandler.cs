using Eka.Web.Thesaurus;
using System;
using System.Text.RegularExpressions;

namespace ChatBot.MessageSpanHandlers
{
    public class DogeSpanHandler : IMessageSpanHandler
    {
        private DateTime lastdoge = DateTime.Now;
        private Random rnd = new Random();

        private string[] DogeRegEx = new string[]
        {
            "so", "much", "very", "such"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            string pattern_prefixes = "";
            bool first = true;

            foreach (string prefix in DogeRegEx)
            {
                if (first)
                    first = false;
                else
                    pattern_prefixes += "|";
                pattern_prefixes += prefix;
            }

            string pattern = "^(" + pattern_prefixes + ") ([a-zA-Z]+)$";

            foreach (Match match in new Regex(pattern, RegexOptions.IgnoreCase).Matches(message))
            {
                actionSpanSink(match, null);
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            if (DateTime.Now.Subtract(lastdoge).Seconds <= 10)
            {
                string prefix = actionSpan.Match.Groups[1].ToString();
                string word = actionSpan.Match.Groups[2].ToString();

                Thesaurus thesaurus = new Thesaurus(word);

                if (thesaurus.Success)
                {
                    string message = "";

                    foreach (string p in DogeRegEx)
                    {
                        if (thesaurus.Synonyms.Count == 0) break;
                        if (p != prefix)
                        {
                            int pos = rnd.Next(thesaurus.Synonyms.Count - 1);
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