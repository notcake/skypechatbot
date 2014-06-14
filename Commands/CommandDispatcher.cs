using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ChatBot.CommandHandlers;

namespace ChatBot.Commands
{
    public class CommandDispatcher
    {
        private Logger Logger;

        private Dictionary<string, ICommandHandler> CommandHandlers = new Dictionary<string, ICommandHandler>();
        private Dictionary<string, ICommandHandler> LowercaseCommandHandlers = new Dictionary<string, ICommandHandler>();

        public CommandDispatcher(Logger logger)
        {
            this.Logger = logger;
        }

        public void AddCommandHandler(ICommandHandler commandHandler)
        {
            this.CommandHandlers.Add(commandHandler.Command, commandHandler);

            if (!this.LowercaseCommandHandlers.ContainsKey(commandHandler.Command.ToLower()))
            {
                this.LowercaseCommandHandlers.Add(commandHandler.Command.ToLower(), commandHandler);
            }

            this.Logger.Log("CommandHandler " + commandHandler.Command + " (" + commandHandler.GetType().FullName + ") registered.");
        }

        public bool HandleMessage(MessageSink messageSink, string message)
        {
            if (message[0] != '!' &&
                message[0] != '/' &&
                (message[0] != ' ' || message[0] != '/'))
            {
                return false;
            }

            Command command = new Command(message);

            ICommandHandler commandHandler = null;

            if (this.CommandHandlers.ContainsKey(command.Name))
            {
                commandHandler = this.CommandHandlers[command.Name];
            }
            else if (this.CommandHandlers.ContainsKey(command.Name))
            {
                commandHandler = this.LowercaseCommandHandlers[command.Name.ToLower()];
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
