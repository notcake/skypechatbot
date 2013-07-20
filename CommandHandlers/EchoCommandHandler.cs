using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatBot.CommandHandlers
{
    public class EchoCommandHandler : ICommandHandler
    {
        public string Command
        {
            get { return "echo"; }
        }

        public void Handle(MessageSink messageSink, Command command)
        {
            messageSink(command.FullArguments);
        }
    }
}
