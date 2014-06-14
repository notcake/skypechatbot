using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatBot.Commands
{
    public class Command
    {
        public string FullCommand { get; protected set; }
        public string FullArguments { get; protected set; }

        public string Name { get; protected set; }
        private List<string> arguments = new List<string>();

        public Command(string fullCommand)
        {
            this.FullCommand = fullCommand;

            Tuple<string, string[], string> parsedCommand = new CommandParser().Parse(fullCommand);
            this.Name = parsedCommand.Item1;
            this.arguments.AddRange(parsedCommand.Item2);
            this.FullArguments = parsedCommand.Item3;
        }

        public IList<string> Arguments
        {
            get
            {
                return this.arguments;
            }
        }
    }
}
