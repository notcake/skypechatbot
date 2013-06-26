using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatBot
{
    public delegate void ActionSpanSink(Match match, string data);
    public delegate void MessageSink(string messageSink);

    public class MessageHandler
    {
        private Logger Logger;

        private List<IMessageSpanHandler> SpanHandlers = new List<IMessageSpanHandler>();

        public MessageHandler(Logger logger)
        {
            this.Logger = logger;
        }

        public void AddSpanHandler(IMessageSpanHandler messageSpanHandler)
        {
            this.SpanHandlers.Add(messageSpanHandler);

            this.Logger.Log("MessageSpanHandler " + messageSpanHandler.GetType().FullName + " registered.");
        }

        public void HandleMessage(MessageSink messageSink, string message)
        {
            List<MessageActionSpan> actionSpans = new List<MessageActionSpan>();

            foreach (IMessageSpanHandler messageSpanHandler in this.SpanHandlers)
            {
                messageSpanHandler.IdentifyActionSpans((x, y) => actionSpans.Add(new MessageActionSpan(x, y, messageSpanHandler)), message);
            }

            if (actionSpans.Count > 0)
            {
                this.Logger.Log("Handling message:");
                this.Logger.Log("\t" + actionSpans.Count + " action span(s).");
            }

            foreach (MessageActionSpan messageActionSpan in actionSpans.OrderBy(x => x.Position))
            {
                messageActionSpan.Handler.HandleSpan(messageSink, messageActionSpan);
            }
        }
    }
}
