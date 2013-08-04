using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatBot.CommandHandlers
{
    public class EvalCommandHandler : ICommandHandler
    {
       public static double Evaluate(string expression)  
       {  
           System.Data.DataTable table = new System.Data.DataTable();  
           table.Columns.Add("expression", string.Empty.GetType(), expression);  
           System.Data.DataRow row = table.NewRow();  
           table.Rows.Add(row);  
           return double.Parse((string)row["expression"]);  
       }

        public string Command
        {
            get { return "eval"; }
        }

        public void Handle(MessageSink messageSink, Command command)
        {
            messageSink(Evaluate(command.FullArguments));
        }
    }
}
