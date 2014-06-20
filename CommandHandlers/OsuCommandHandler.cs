using System.Collections.Generic;
using System.Net;
using ChatBot.Commands;
using Newtonsoft.Json.Linq;

namespace ChatBot.CommandHandlers
{
    public class OsuCommandHandler : ICommandHandler
    {
        public string Command
        {
            get { return "osu"; }
        }
        /// <summary>
        /// Gets a specific string from the osu api
        /// </summary>
        /// <param name="method"></param>
        /// <param name="req"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns>string response</returns>
        public string GetStr(string method, string req, string arg1 = "", string arg2 = "")
        {
            using (WebClient client = new WebClient())
            {
                string baseurl = "https://osu.ppy.sh/api/";
                string apikey = "594a013bcf9d0505ee1f55ed85f7936df80ccb28";

                string response = client.DownloadString(baseurl + method + "?" + "k=" + apikey + arg1 + arg2);
                JArray data = JArray.Parse(response);

                return data[0][req].ToString();
            }
        }
        /// <summary>
        ///  Gets the whole array instead. And yes I know I could include it in the first one but cba
        /// </summary>
        /// <param name="method"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns>JArray data</returns>
        public JArray GetData(string method, string arg1 = "", string arg2 = "")
        {
            using (WebClient client = new WebClient())
            {
                string baseurl = "https://osu.ppy.sh/api/";
                string apikey = "594a013bcf9d0505ee1f55ed85f7936df80ccb28";

                string response = client.DownloadString(baseurl + method + "?" + "k=" + apikey + arg1 + arg2);
                JArray data = JArray.Parse(response);

                return data;
            }
        }

        public void Handle(MessageSink messageSink, Command command)
        {
            /// you can start crying here
            IList<string> cmd = command.Arguments;
            int cnt = cmd.Count;


            if (cnt < 1)
            {
                messageSink("Usage:\n !osu [command] [args]\n Available Commands: stats,pp,acc,rank,score,playcount/pc,lastplayed/lp");
            }
            else
            {

                switch (cnt)
                {
                    case 1:
                        messageSink("Missing Second Argument!");
                        break;

                    case 2:
                    case 3:
                        switch (cmd[0])
                        {
                            case "pp":
                                {
                                    string req = GetStr("get_user", "pp_raw", "&u=" + cmd[1]);
                                    decimal pp;
                                    bool yes = decimal.TryParse(req.Replace(".", ","), out pp);

                                    if (yes & pp > 0)
                                    {
                                        messageSink(pp.ToString("#,#"));
                                    }
                                    else
                                    {
                                        messageSink("Invalid user or user has no pp!");
                                    }


                                }
                                break;
                            case"pc":
                            case "playcount":
                                {
                                    string req = GetStr("get_user", "playcount", "&u=" + cmd[1]);
                                    int playcount;
                                    bool yes = int.TryParse(req, out playcount);

                                    if (yes & playcount > 0)
                                    {
                                        messageSink(playcount.ToString("#,#"));
                                    }
                                    else
                                    {
                                        messageSink("Invalid user or user has not played yet!");
                                    }
                                }
                                break;
                            case "score":
                                {
                                    string req = GetStr("get_user", "ranked_score", "&u=" + cmd[1]);
                                    decimal score;
                                    bool yes = decimal.TryParse(req.Replace(".", ","), out score);

                                    if (yes & score > 0)
                                    {
                                        messageSink(score.ToString("#,#"));
                                    }
                                    else
                                    {
                                        messageSink("Invalid user or user has no score!");
                                    }

                                }
                                break;
                            case "rank":
                                {
                                    string req = GetStr("get_user", "pp_rank", "&u=" + cmd[1]);
                                    decimal rank;
                                    bool yes = decimal.TryParse(req.Replace(".", ","), out rank);

                                    if (yes & rank > 0)
                                    {
                                        messageSink(rank.ToString("#,#"));
                                    }
                                    else
                                    {
                                        messageSink("Invalid user or user has no rank!");
                                    }

                                }
                                break;
                            case "acc":
                            case "accuracy":
                                {
                                    string req = GetStr("get_user", "accuracy", "&u=" + cmd[1]);
                                    decimal acc;
                                    bool yes = decimal.TryParse(req.Replace(".", ","), out acc);

                                    if (yes & acc > 0)
                                    {
                                        messageSink(acc.ToString("F") + "%");

                                    }
                                    else
                                    {
                                        messageSink("Invalid user or user has no acc!");
                                    }
                                }
                                break;
                            case "lastplayed":
                            case "lp":
                                {
                                    JArray data = new JArray();
                                    if (cmd.Count > 2)
                                    {
                                        data = GetData("get_user_recent", "&u=" + cmd[1] + "&m=" + cmd[2]);
                                    }
                                    else
                                    {
                                        data = GetData("get_user_recent", "&u=" + cmd[1]);
                                    }
                                    int beatmapid;
                                    bool yes = int.TryParse(data[0]["beatmap_id"].ToString(), out beatmapid);

                                    if (yes)
                                    {
                                        if (beatmapid > 0)
                                        {
                                            string title = GetStr("get_beatmaps", "title", "&b=" + beatmapid);
                                            string artist = GetStr("get_beatmaps", "artist", "&b=" + beatmapid);
                                            string rank = data[0]["rank"].ToString();
                                            string maxcombo = data[0]["maxcombo"].ToString();
                                            string misses = data[0]["countmiss"].ToString();

                                            messageSink("Lastplayed: " + title + " by: " + artist + "\n" + "Rank: " + rank + "\n" + "Max Combo: " + maxcombo + "\n" + "Miss: " + misses);
                                        }
                                        else
                                        {
                                            messageSink("invalid beatmap?");
                                        }
                                    }
                                    else
                                    {
                                        messageSink("Couldn't Parse data!");
                                    }


                                }
                                break;
                            case "stats":
                                {
                                    JArray data = GetData("get_user", "&u=" + cmd[1]);

                                    /// lol

                                    decimal pp;
                                    decimal.TryParse(data[0]["pp_raw"].ToString().Replace(".", ","), out pp);
                                    decimal rank;
                                    decimal.TryParse(data[0]["pp_rank"].ToString().Replace(".", ","), out rank);
                                    decimal score;
                                    decimal.TryParse(data[0]["ranked_score"].ToString().Replace(".", ","), out score);
                                    decimal level;
                                    decimal.TryParse(data[0]["level"].ToString().Replace(".", ","), out level);
                                    decimal acc;
                                    decimal.TryParse(data[0]["accuracy"].ToString().Replace(".", ","), out acc);
                                    string country = data[0]["country"].ToString();

                                    messageSink(cmd[1] + "\n\n" + "Rank: " + rank.ToString("#,#") + "\nPP: " + pp.ToString("#,#") + "\nScore: " + score.ToString("#,#") + "\nLevel: " + level.ToString("#,#") + "\nAcc: " + acc.ToString("F") + "%" + "\n" + country);



                                }
                                break;
                        }
                        break;
                }
            }


        }
    }

}
