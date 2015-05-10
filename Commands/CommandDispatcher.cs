using System.Collections.Generic;
using ChatBot.CommandHandlers;

namespace ChatBot.Commands
{
    public class CommandDispatcher
    {
        private readonly Dictionary<string, ICommandHandler> CommandHandlers = new Dictionary<string, ICommandHandler>();
        private readonly Logger Logger;

        private readonly Dictionary<string, ICommandHandler> LowercaseCommandHandlers =
            new Dictionary<string, ICommandHandler>();

        public CommandDispatcher(Logger logger)
        {
            Logger = logger;
        }

        public void AddCommandHandler(ICommandHandler commandHandler)
        {
            CommandHandlers.Add(commandHandler.Command, commandHandler);

            if (!LowercaseCommandHandlers.ContainsKey(commandHandler.Command.ToLower()))
            {
                LowercaseCommandHandlers.Add(commandHandler.Command.ToLower(), commandHandler);
            }

            Logger.Log("CommandHandler " + commandHandler.Command + " (" + commandHandler.GetType().FullName +
                       ") registered.");
        }

        public bool HandleMessage(MessageSink messageSink, string message)
        {
            if (message[0] != '!' &&
                message[0] != '/' &&
                (message[0] != ' ' || message[0] != '/'))
            {
                return false;
            }

            var command = new Command(message);

            ICommandHandler commandHandler = null;

            if (CommandHandlers.ContainsKey(command.Name))
            {
                commandHandler = CommandHandlers[command.Name];
            }
            else if (CommandHandlers.ContainsKey(command.Name))
            {
                commandHandler = LowercaseCommandHandlers[command.Name.ToLower()];
            }
            else
            {
                return false;
            }

            commandHandler.Handle(messageSink, command);

            return true;
        }
    }
}