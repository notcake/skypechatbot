using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatBot
{
    public interface IMessageSpanHandler
    {
        void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message);
        void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan);
    }
}
