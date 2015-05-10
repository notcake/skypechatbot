using ChatBot.Commands;
using Eka.Web.PlotGenerator;

namespace ChatBot.CommandHandlers
{
    public class PlotCommandHandler : ICommandHandler
    {
        public string Command => "plot";

        public void Handle(MessageSink messageSink, Command command)
        {
            var plot = new Plot();
            if (plot.Success)
                messageSink(plot.PlotText);
        }
    }
}