using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatBot
{
    public class MessageActionSpan
    {
        public uint Position { get; protected set; }
        public Match Match { get; protected set; }
        public string Data { get; protected set; }

        public IMessageSpanHandler Handler { get; protected set; }

        public MessageActionSpan(Match match, string data, IMessageSpanHandler handler)
        {
            this.Position = (uint)match.Index;
            this.Match = match;
            this.Data = data;

            this.Handler = handler;
        }
    }
}
