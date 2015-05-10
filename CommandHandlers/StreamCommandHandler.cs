using System.Collections.Generic;
using System.Net;
using ChatBot.Commands;
using Newtonsoft.Json;

namespace ChatBot.CommandHandlers
{
    public class StreamCommandHandler : ICommandHandler
    {
        private static readonly string url = "http://join.zombie.computer:8000/ukgamer.ogg";
        private static readonly string statsurl = "http://test.ryan-o.co.uk/radio/stats.php";

        public string Command => "stream";

        public void Handle(MessageSink messageSink, Command command)
        {
            using (var client = new WebClient())
            {
                var response = client.DownloadString(statsurl);

                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

                if (data.ContainsKey("offline"))
                {
                    messageSink("Stream is offline.");
                    return;
                }

                var listeners = int.Parse(data["listeners"]);
                if (listeners > 0)
                {
                    messageSink(string.Format("Now playing with {0} listener{1}: {2} - {3}\n{4}", listeners,
                        ((listeners == 1) ? "" : "s"), data["artist"], data["title"], url));
                }
                else
                {
                    messageSink(string.Format("Now playing: {0} - {1}\n{2}", data["artist"], data["title"], url));
                }
            }
        }
    }
}