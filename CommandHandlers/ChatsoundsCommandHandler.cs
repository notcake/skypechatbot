using System.Web;
using ChatBot.Commands;

namespace ChatBot.CommandHandlers
{
    public class ChatsoundsCommandHandler : ICommandHandler
    {
        public string Command => "cs";

        public void Handle(MessageSink messageSink, Command command)
        {
            if (command.FullArguments == "")
            {
                messageSink("[CS] http://cs.3kv.in/");
            }
            else
            {
                messageSink("[CS] " + GetLink(command.FullArguments));
            }
        }

        public static string GetLink(string expression)
        {
            var link = "http://cs.3kv.in/?s=";
            link = link + HttpUtility.UrlPathEncode(expression);
            return link;
        }
    }
}