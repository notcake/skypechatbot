using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
