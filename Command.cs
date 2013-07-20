using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatBot
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

            string[] commandParts = fullCommand.Split(' ');
            this.Name = commandParts[0].Substring(1);
            this.FullArguments = fullCommand.Substring(commandParts[0].Length + 1);
            this.arguments.AddRange(commandParts.Skip(1).AsEnumerable());
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
