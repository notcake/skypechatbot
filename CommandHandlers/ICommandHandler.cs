using ChatBot.Commands;

namespace ChatBot.CommandHandlers
{
    public interface ICommandHandler
    {
        string Command { get; }

        void Handle(MessageSink messageSink, Command command);
    }
}