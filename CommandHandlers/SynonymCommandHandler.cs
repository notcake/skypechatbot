using ChatBot.Commands;
using Eka.Web.Thesaurus;

namespace ChatBot.CommandHandlers
{
    public class SynonymCommandHandler : ICommandHandler
    {
        public string Command => "synonym";

        public void Handle(MessageSink messageSink, Command command)
        {
            var thesaurus = new Thesaurus(command.FullArguments);
            var message = "Synonyms for '" + command.FullArguments + "': ";

            if (thesaurus.Success)
            {
                foreach (var synonym in thesaurus.Synonyms)
                {
                    message += synonym + ", ";
                }
                messageSink(message);
            }
        }
    }
}