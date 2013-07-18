using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatBot
{
    public interface ICommandHandler
    {
        string Command { get; }

        void Handle(MessageSink messageSink, Command command);
    }
}
