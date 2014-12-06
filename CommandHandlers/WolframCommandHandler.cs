using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ChatBot.Commands;
using Eka.Web.Wolfram;

namespace ChatBot.CommandHandlers
{
    public class WolframCommandHandler : ICommandHandler
    {
        public string Command
        {
            get { return "w"; }
        }

        public void Handle(MessageSink messageSink, Command command)
        {
            if (command.FullArguments == "") { return; }
            Wolfram response = new Wolfram(command.FullArguments);

            if (response.Success)
            {
                messageSink(response.Output);
            }
            else
            {
                messageSink("Some kind of error happened. Or Query took to long!");
            }
        }
    }
}
