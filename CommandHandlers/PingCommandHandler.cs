using ChatBot.Commands;

namespace ChatBot.CommandHandlers
{
    public class PingCommandHandler : ICommandHandler
    {
        public string Command => "ping";

        public void Handle(MessageSink messageSink, Command command)
        {
            messageSink("ICMP echo reply.");
        }
    }
}