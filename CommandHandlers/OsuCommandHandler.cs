using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
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
            public string DBG { get; set; }
            public JArray Data { get; set; }
            public string Err { get; set; }
        }

        [Flags]
        public enum Mods
        {

            None = 0,
            NF = 1,
            EZ = 2,
            //NV = 4,
            HD = 8,
            HR = 16,
            SD = 32,
            DT = 64,
            RX = 128,
            HT = 256,
            NC = 512,
            FL = 1024,
            AP = 2048,
            SO = 4096,
            RX2 = 8192,  // Autopilot?
            PF = 16384,
            Key4 = 32768,
            Key5 = 65536,
            Key6 = 131072,
            Key7 = 262144,
            Key8 = 524288,
            keyMod = Key4 | Key5 | Key6 | Key7 | Key8,
            FadeIn = 1048576,
            Random = 2097152,
            LastMod = 4194304,
            FreeModAllowed = NF | EZ | HD | HR | SD | FL | FadeIn | RX | RX2 | SO | keyMod
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
                            result.DBG = result.Data.ToString();
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
                            result.DBG = data.ToString();
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
            string pattern = "";
            string extra = "";

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
                            case "pp": { handler = "get_user"; method = "pp_raw"; pattern = "F1"; } break;
                            case "pc":
                            case "playcount": { handler = "get_user"; method = "playcount"; pattern = "#,#"; } break;
                            case "acc":
                            case "accuracy": { handler = "get_user"; method = "accuracy"; pattern = "F3"; extra = "%"; } break;
                            case "score": { handler = "get_user"; method = "ranked_score"; pattern = "#,#"; } break;
                            case "rank": { handler = "get_user"; method = "pp_rank"; pattern = "#,#"; } break;
                            case "level": { handler = "get_user"; method = "level"; pattern = "F0"; } break;
                            case "lp":
                            case "lastplayed": { handler = "get_user_recent"; } break;
                        }
                        break;
                }
            }

            if (handler.Length > 0 & method.Length > 0)
            {
                Result user = GetData(handler, method, "&u=" + cmd[1]);
                if (user.Success)
                {
                    decimal num;
                    bool yes = decimal.TryParse(user.Str, out num);

                    if (yes)
                    {
                        if (num > 0)
                        {
                            messageSink(num.ToString(pattern) + extra);
                        }
                        else
                        {
                            messageSink("User has no " + cmd[0] + ".");
                        }
                    }
                }
                else
                {
                    messageSink("ERR: " + user.Err);
                }
            }
            else if (handler.Length > 0 & method.Length < 1)
            {
                Result user = GetData(handler, nothing, "&u=" + cmd[1]);
                if (user.Success)
                {
                    if (handler == "get_user_recent")
                    {
                        int beatmapid;
                        int.TryParse(user.Data[0]["beatmap_id"].ToString(), out beatmapid);
                        Result beatmap = GetData("get_beatmaps", nothing, "&b=" + beatmapid);
                        int userid;
                        int.TryParse(user.Data[0]["user_id"].ToString(), out userid);
                        if (beatmap.Success)
                        {

                            string title = beatmap.Data[0]["title"].ToString();
                            string artist = beatmap.Data[0]["artist"].ToString();
                            string diff = beatmap.Data[0]["version"].ToString();
                            string rank = user.Data[0]["rank"].ToString();
                            int maxcombo;
                            int.TryParse(user.Data[0]["maxcombo"].ToString(), out maxcombo);
                            int misses;
                            int.TryParse(user.Data[0]["countmiss"].ToString(), out misses);
                            int emods;
                            int.TryParse(user.Data[0]["enabled_mods"].ToString(), out emods);
                            int pf;
                            int.TryParse(user.Data[0]["perfect"].ToString(), out pf);
                            Result score = GetData("get_scores", "", "&u=" + userid, "&b=" + beatmapid);
                            float pp = 0;
                            if (score.Success)
                            {
                                float.TryParse(score.Data[0]["pp"].ToString(), out pp);
                                
                            }
                            else
                            {
                                messageSink("Could not get score!");
                            }
                            string perfect = "";

                            if (pf > 0)
                            {
                                perfect = "FULL COMBO!";
                            }

                            string url = "https://osu.ppy.sh/b/" + beatmapid;


                            Mods mods = (Mods)emods;


                            messageSink("Lastplayed: " + title + " by: " + artist + " [" + diff + "] " + "\n" + url + "\nMods: " + mods + "\nRank: " + rank + "\n" + "Max Combo: " + maxcombo.ToString("#,#") + "\n" + "Miss: " + misses.ToString("#,#") + "\nPP: " + pp.ToString() + "\n" + perfect);
                        }
                        else
                        {
                            messageSink("Could not get Beatmap!");
                        }
                    }
                    else if (handler == "get_user")
                    {
                        decimal pp;
                        decimal.TryParse(user.Data[0]["pp_raw"].ToString(), out pp);
                        decimal rank;
                        decimal.TryParse(user.Data[0]["pp_rank"].ToString().Replace(".", ","), out rank);
                        decimal score;
                        decimal.TryParse(user.Data[0]["ranked_score"].ToString().Replace(".", ","), out score);
                        decimal level;
                        decimal.TryParse(user.Data[0]["level"].ToString(), out level);
                        decimal acc;
                        decimal.TryParse(user.Data[0]["accuracy"].ToString(), out acc);
                        string country = user.Data[0]["country"].ToString();

                        messageSink(cmd[1] + "\n\n" + "Rank: #" + rank.ToString("#,#") + "\nPP: " + pp.ToString("F1") + "\nScore: " + score.ToString("#,#") + "\nLevel: " + level.ToString("#,#") + "\nAcc: " + acc.ToString("F3") + "%" + "\n" + country);

                    }

                }
                else
                {
                    messageSink("ERR: " + user.Err);
                }

            }
        }
    }
}