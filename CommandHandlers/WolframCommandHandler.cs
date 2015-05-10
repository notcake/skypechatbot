using ChatBot.Commands;
using Eka.Web.Wolfram;

namespace ChatBot.CommandHandlers
{
    public class WolframCommandHandler : ICommandHandler
    {
        public string Command => "w";

        public void Handle(MessageSink messageSink, Command command)
        {
            if (command.FullArguments == "")
            {
                return;
            }
            var response = new Wolfram(command.FullArguments);

            if (response.Success)
            {
                messageSink(response.Output);
            }
            else
            {
                messageSink("Some kind of error happened. Or Query took too long!");
            }
        }
    }
}