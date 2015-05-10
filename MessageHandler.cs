using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChatBot.MessageSpanHandlers;

namespace ChatBot
{
    public delegate void ActionSpanSink(Match match, string data);

    public delegate void MessageSink(string messageSink);

    public class MessageHandler
    {
        private readonly Logger Logger;
        private readonly List<IMessageSpanHandler> SpanHandlers = new List<IMessageSpanHandler>();

        public MessageHandler(Logger logger)
        {
            Logger = logger;
        }

        public void AddSpanHandler(IMessageSpanHandler messageSpanHandler)
        {
            SpanHandlers.Add(messageSpanHandler);

            Logger.Log("MessageSpanHandler " + messageSpanHandler.GetType().FullName + " registered.");
        }

        public void HandleMessage(MessageSink messageSink, string message)
        {
            var actionSpans = new List<MessageActionSpan>();

            foreach (var messageSpanHandler in SpanHandlers)
            {
                messageSpanHandler.IdentifyActionSpans(
                    (x, y) => actionSpans.Add(new MessageActionSpan(x, y, messageSpanHandler)), message);
            }

            if (actionSpans.Count > 0)
            {
                Logger.Log("Handling message:");
                Logger.Log("\t" + actionSpans.Count + " action span(s).");
            }

            foreach (var messageActionSpan in actionSpans.OrderBy(x => x.Position))
            {
                messageActionSpan.Handler.HandleSpan(messageSink, messageActionSpan);
            }
        }
    }
}