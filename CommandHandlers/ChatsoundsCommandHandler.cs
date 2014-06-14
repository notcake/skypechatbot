using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ChatBot.Commands;

namespace ChatBot.CommandHandlers
{
    public class ChatsoundsCommandHandler : ICommandHandler
    {
       public static String GetLink(string expression)
       {
           String link = "http://cs.3kv.in/?s=";
           link = link + System.Web.HttpUtility.UrlPathEncode(expression);
           return link;
       }

        public string Command
        {
            get { return "cs"; }
        }

        public void Handle(MessageSink messageSink, Command command)
        {
            messageSink("[CS] " + GetLink(command.FullArguments));
        }
    }
}
