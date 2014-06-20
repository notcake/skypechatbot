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
        /// thanks stackoverflow
        /// </summary>
        public class Result
        {
            public bool Success { get; set; }
            public string Str { get; set; }
            public JArray Data { get; set; }
            public string Err { get; set; }
        }


        /// <summary>
        /// I'm really proud of this one
        /// </summary>
        /// <returns>string response or JArray data</returns>
        public Result GetData(string handler, string req = "", string arg1 = "", string arg2 = "")
        {
            Result result = new Result();
            using (WebClient client = new WebClient())
            {
                string baseurl = "https://osu.ppy.sh/api/";
                string apikey = "594a013bcf9d0505ee1f55ed85f7936df80ccb28";

                try
                {
                    if (req.Length == 0)
                    {
                        result.Data = JArray.Parse(client.DownloadString(baseurl + handler + "?" + "k=" + apikey + arg1 + arg2));
                        if (result.Data.Count > 0)
                        {
                            result.Success = true;
                        }
                        else
                        {
                            result.Success = false;
                            result.Err = "Got no data... Did you type it correctly?";
                        }
                    }
                    else
                    {
                        JArray data = JArray.Parse(client.DownloadString(baseurl + handler + "?" + "k=" + apikey + arg1 + arg2));
                        if (data.Count > 0)
                        {
                            result.Str = data[0][req].ToString();
                            result.Success = true;
                        }
                        else
                        {
                            result.Success = false;
                            result.Err = "Got no data... Did you type it correctly?";
                        }
                    }

                }

                catch (WebException ex)
                {
                    result.Err = ex.Message;
                    result.Success = false;
                }
            }

            return result;


        }

        public void Handle(MessageSink messageSink, Command command)
        {
            /// you can start crying here
            IList<string> cmd = command.Arguments;
            int cnt = cmd.Count;
            string method = "";
            string handler = "";
            string nothing = "";

            // 1 = get_beatmaps
            // 2 = get_user
            // 3 = get_scores
            // 4 = get_user_best
            // 5 = get_user_rent
            // 6 = get_match

            //            string beatmap_methods = @"
            //            approved,approved_date,last_update,artist,beatmap_id,beatmapset_id,bpm,creator,difficultyrating,
            //            diff_size,diff_overall,diff_approach,diff_drain,hit_length,source,title,total_length,version,mode";

            //            string user_methods = @"
            //            user_id,username,count300,count100,count50,playcount,ranked_score,total_score,
            //            pp_rank,level,pp_raw,accuracy,count_rank_ss,count_rank_s,count_rank_a,country";

            //            string score_methods = @"
            //            score,username,count300,count100,count50,countmiss,maxcombo,countkatu,countgeki,perfect,enabled_mods,user_id,date,rank,pp";

            //            string user_best_methods = @"
            //            beatmap_id,score,username,count300,count100,count50,countmiss,maxcombo,countkatu,countgeki,perfect,enabled_mods,user_id,date,rank,pp";

            //            string user_recent_methods = @"
            //            beatmap_id,score,count300,count100,count50,countmiss,maxcombo,countkatu,countgeki,perfect,enabled_mods,user_id,date,rank";

            //            string multiplayer_methods = @"
            //            match,games";

            //            Dictionary<string,string> methods = new Dictionary<string,string>();
            //            for (int i = 1; i <= 19; i++)
            //            {
            //                methods.Add("", "");
            //            }





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
                            // u wot m8
                            case "stats": { handler = "get_user"; } break;
                            case "pp": { handler = "get_user"; method = "pp_raw"; } break;
                            case "pc":
                            case "playcount": { handler = "get_user"; method = "playcount"; } break;
                            case "acc":
                            case "accuracy": { handler = "get_user"; method = "accuracy"; } break;
                            case "score": { handler = "get_user"; method = "ranked_score"; } break;
                            case "rank": { handler = "get_user"; method = "pp_rank"; } break;
                            case "level": { handler = "get_user"; method = "level"; } break;
                            case "lp":
                            case "lastplayed": { handler = "get_user_recent"; } break;
                        }
                        break;
                }
            }

            if (handler.Length > 0 & method.Length > 0)
            {
                Result result = GetData(handler, method, "&u=" + cmd[1]);
                if (result.Success)
                {
                    decimal num;
                    bool yes = decimal.TryParse(result.Str.Replace(".", ","), out num);

                    if (yes)
                    {
                        if (num > 0)
                        {
                            messageSink(num.ToString("#,#"));
                        }
                        else
                        {
                            messageSink("User has no " + cmd[0] + ".");
                        }
                    }
                }
                else
                {
                    messageSink("ERR: " + result.Err);
                }
            }
            else if (handler.Length > 0 & method.Length < 1)
            {
                Result result = GetData(handler, nothing, "&u=" + cmd[1]);
                if (result.Success)
                {
                    if (handler == "get_user_recent")
                    {
                        int beatmapid;
                        int.TryParse(result.Data[0]["beatmap_id"].ToString(), out beatmapid);
                        Result beatmap = GetData("get_beatmaps", nothing, "&b=" + beatmapid);
                        if (beatmap.Success)
                        {

                            string title = beatmap.Data[0]["title"].ToString();
                            string artist = beatmap.Data[0]["artist"].ToString();
                            int rank;
                            int.TryParse(result.Data[0]["rank"].ToString(), out rank);
                            int maxcombo;
                            int.TryParse(result.Data[0]["maxcombo"].ToString(), out maxcombo);
                            int misses;
                            int.TryParse(result.Data[0]["countmiss"].ToString(), out misses);

                            messageSink("Lastplayed: " + title + " by: " + artist + "\n" + "Rank: " + rank.ToString("#,#") + "\n" + "Max Combo: " + maxcombo.ToString("#,#") + "\n" + "Miss: " + misses.ToString("#,#"));
                        }
                        else
                        {
                            messageSink("Could not get Beatmap!");
                        }
                    }
                    else if (handler == "get_user")
                    {
                        decimal pp;
                        decimal.TryParse(result.Data[0]["pp_raw"].ToString().Replace(".", ","), out pp);
                        decimal rank;
                        decimal.TryParse(result.Data[0]["pp_rank"].ToString().Replace(".", ","), out rank);
                        decimal score;
                        decimal.TryParse(result.Data[0]["ranked_score"].ToString().Replace(".", ","), out score);
                        decimal level;
                        decimal.TryParse(result.Data[0]["level"].ToString().Replace(".", ","), out level);
                        decimal acc;
                        decimal.TryParse(result.Data[0]["accuracy"].ToString().Replace(".", ","), out acc);
                        string country = result.Data[0]["country"].ToString();

                        messageSink(cmd[1] + "\n\n" + "Rank: " + rank + "\nPP: " + pp.ToString("#,#") + "\nScore: " + score.ToString("#,#") + "\nLevel: " + level.ToString("#,#") + "\nAcc: " + acc.ToString("F") + "%" + "\n" + country);

                    }

                }
                else
                {
                    messageSink("ERR: " + result.Err);
                }

            }
        }
    }
}