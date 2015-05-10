using System.Collections.Generic;

namespace ChatBot.Commands
{
    public class Command
    {
        private readonly List<string> arguments = new List<string>();

        public Command(string fullCommand)
        {
            FullCommand = fullCommand;

            var parsedCommand = new CommandParser().Parse(fullCommand);
            Name = parsedCommand.Item1;
            arguments.AddRange(parsedCommand.Item2);
            FullArguments = parsedCommand.Item3;
        }

        public string FullCommand { get; protected set; }
        public string FullArguments { get; protected set; }
        public string Name { get; protected set; }

        public IList<string> Arguments => arguments;
    }
}