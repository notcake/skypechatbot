using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Eka.Web.PlotGenerator;

namespace ChatBot.CommandHandlers
{
    public class PlotCommandHandler : ICommandHandler
    {
        public string Command
        {
            get { return "plot"; }
        }

        public void Handle(MessageSink messageSink, Command command)
        {
            Plot plot = new Plot();
            if (plot.Success)
                messageSink(plot.PlotText);
        }
    }
}
