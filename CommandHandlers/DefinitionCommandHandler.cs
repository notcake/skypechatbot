using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Eka.Web.UrbanDictionary;

namespace ChatBot.CommandHandlers
{
    public class DefinitionCommandHandler : ICommandHandler
    {
        public string Command
        {
            get { return "define"; }
        }

        public void Handle(MessageSink messageSink, Command command)
        {
            UrbanDictionary dictionary = new UrbanDictionary(command.FullArguments);
            if (dictionary.Success)
                messageSink(dictionary.Definition + (dictionary.Example != null ? "\nExample: " + dictionary.Example : ""));
            else
            	messageSink("No definition found.");
        }
    }
}
