using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ChatBot.Commands;
using Eka.Web.Thesaurus;

namespace ChatBot.CommandHandlers
{
    public class SynonymCommandHandler : ICommandHandler
    {
        public string Command
        {
            get { return "synonym"; }
        }

        public void Handle(MessageSink messageSink, Command command)
        {
            Thesaurus thesaurus = new Thesaurus(command.FullArguments);
            string message = "Synonyms for '" + command.FullArguments + "': ";


            if (thesaurus.Success)
            {
                foreach (string synonym in thesaurus.Synonyms)
                {
                    message += synonym + ", ";
                }
                messageSink(message);
            }
        }
    }
}
