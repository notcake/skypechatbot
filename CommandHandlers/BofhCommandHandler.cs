using System;
using System.Linq;
using ChatBot.Commands;
using ChatBot.Properties;

namespace ChatBot.CommandHandlers
{
    public class BofhCommandHandler : ICommandHandler
    {
        private readonly string[] Excuses = Resources.bofh_excuses.Split('\n');
        private readonly Random rnd = new Random();

        public string Command => "bofh";

        public void Handle(MessageSink messageSink, Command command)
        {
            messageSink(GetRandomExcuse());
        }

        public string GetRandomExcuse()
        {
            var r = rnd.Next(Excuses.Count());
            return Excuses[r];
        }
    }
}