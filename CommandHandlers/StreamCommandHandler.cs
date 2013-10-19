using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace ChatBot.CommandHandlers
{
    public class StreamCommandHandler : ICommandHandler
    {
        public string Command
        {
            get { return "stream"; }
        }

        public void Handle(MessageSink messageSink, Command command)
        {
            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString("http://test.ryan-o.co.uk/radio/stats.php");

                Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

                if (data.ContainsKey("offline"))
                {
                    messageSink("Stream is offline.");
                }
                else
                {
                    messageSink("Now playing with " + data["listeners"] + " listeners: " + data["artist"] + " - " + data["title"] + "\n"
                        + "http://dedi.ryan-o.co.uk:8000/ukgamer.ogg"); 
                }
            }
        }
    }
}