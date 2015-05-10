using System.Text.RegularExpressions;
using ChatBot.MessageSpanHandlers;

namespace ChatBot
{
    public class MessageActionSpan
    {
        public MessageActionSpan(Match match, string data, IMessageSpanHandler handler)
        {
            Position = (uint) match.Index;
            Match = match;
            Data = data;

            Handler = handler;
        }

        public uint Position { get; protected set; }
        public Match Match { get; protected set; }
        public string Data { get; protected set; }
        public IMessageSpanHandler Handler { get; protected set; }
    }
}