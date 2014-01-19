using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eka.Web.Thesaurus;

namespace ChatBot.MessageSpanHandlers
{
    public class DogeSpanHandler : IMessageSpanHandler
    {

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
            string prefix = actionSpan.Match.Groups[1].ToString();
            string word = actionSpan.Match.Groups[2].ToString();

            Thesaurus thesaurus = new Thesaurus(word);

            if (thesaurus.Success)
            {
                string message = "";
                int i = 1;

                foreach (string p in DogeRegEx)
                {
                    if (p != prefix)
                    {
                        message += p + " " + thesaurus.Synonyms[i++] + "\n";
                    }
                }
                
                messageSink(message);
            }
        }
    }
}
