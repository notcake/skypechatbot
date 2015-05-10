using System.Data;
using ChatBot.Commands;

namespace ChatBot.CommandHandlers
{
    public class EvalCommandHandler : ICommandHandler
    {
        public string Command => "eval";

        public void Handle(MessageSink messageSink, Command command)
        {
            messageSink(Evaluate(command.FullArguments));
        }

        public static string Evaluate(string expression)
        {
            var table = new DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), expression);
            var row = table.NewRow();
            table.Rows.Add(row);
            return double.Parse((string) row["expression"]).ToString();
        }
    }
}