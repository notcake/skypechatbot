using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatBot.Commands;

namespace ChatBot.CommandHandlers
{
    public class PingCommandHandler : ICommandHandler
    {
        public string Command
        {
            get { return "ping"; }
        }

        public void Handle(MessageSink messageSink, Command command)
        {
            messageSink("ICMP echo reply.");
        }
    }
}
