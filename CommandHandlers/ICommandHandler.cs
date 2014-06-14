using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatBot.Commands;

namespace ChatBot.CommandHandlers
{
    public interface ICommandHandler
    {
        string Command { get; }

        void Handle(MessageSink messageSink, Command command);
    }
}
