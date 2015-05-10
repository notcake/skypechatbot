using ChatBot.Commands;
using Eka.Web.UrbanDictionary;

namespace ChatBot.CommandHandlers
{
    public class DefinitionCommandHandler : ICommandHandler
    {
        public string Command => "define";

        public void Handle(MessageSink messageSink, Command command)
        {
            var dictionary = new UrbanDictionary(command.FullArguments);
            if (dictionary.Success)
            {
                messageSink(dictionary.Definition +
                            (dictionary.Example != null ? "\nExample: " + dictionary.Example : ""));
            }
            else
            {
                messageSink("No definition found.");
            }
        }
    }
}