using Eka.Web.MusicBrainz;
using Eka.Web.Wikipedia;
using Eka.Web.YouTube;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatBot.MessageSpanHandlers
{
    public class YouTubeUrlSpanHandler : IMessageSpanHandler
    {
        private string[] YouTubeUrls = new string[]
        {
            "https?://(www\\.)?youtube\\.com/watch\\?[a-zA-Z0-9_\\-&=#]*v=([a-zA-Z0-9_\\-]+)",
            "https?://(www\\.)?youtu\\.be/([a-zA-Z0-9_\\-]+)",
            "https?://(www\\.)?infinitelooper.com/\\?v=([a-zA-Z0-9_\\-]+)"
        };

        public void IdentifyActionSpans(ActionSpanSink actionSpanSink, string message)
        {
            foreach (string youTubeUrlPattern in this.YouTubeUrls)
            {
                foreach (Match match in new Regex(youTubeUrlPattern, RegexOptions.IgnoreCase).Matches(message))
                {
                    actionSpanSink(match, match.Groups[2].ToString());
                }
            }
        }

        public void HandleSpan(MessageSink messageSink, MessageActionSpan actionSpan)
        {
            string videoId = actionSpan.Data;

            AlbumInfobox albumInfo = null;
            SingleInfobox singleInfo = null;

            VideoInfo videoInfo = new VideoInfo(videoId);

            string formattedDuration = null;
            if (videoInfo.Duration.TotalHours < 1)
            {
                formattedDuration = videoInfo.Duration.Minutes.ToString("D2") + ":" + videoInfo.Duration.Seconds.ToString("D2");
            }
            else
            {
                formattedDuration = ((int)videoInfo.Duration.TotalHours).ToString("D2") + ":" + videoInfo.Duration.Minutes.ToString("D2") + ":" + videoInfo.Duration.Seconds.ToString("D2");
            }

            // Music lookup
            Match match = new Regex("([^\\-]+?)\\-([^\\(\\){\\[]+)").Match(videoInfo.Title);
            if (match.Success)
            {
                RecordingSearch recordingSearch = new RecordingSearch(match.Groups[2].ToString().Trim());
                recordingSearch.Artist = match.Groups[1].ToString().Trim();

                string normalizedSongName = new Regex("[^ a-zA-Z0-9]").Replace(recordingSearch.SearchString, "").ToLower();
                string normalizedArtistName = new Regex("[^ a-zA-Z0-9]").Replace(recordingSearch.Artist, "").ToLower();

                string artistName = null;
                string albumName = null;
                string releaseType = null;
                DateTime releaseDate = DateTime.Now;
                RecordingSearchResults searchResults = recordingSearch.GetResults();
                foreach (RecordingSearchResult result in searchResults)
                {
                    string resultTitle = result.Title;
                    string resultArtistName = result.Artist;

                    if (resultTitle.IndexOf('(') >= 0)
                    {
                        resultTitle = resultTitle.Substring(0, resultTitle.IndexOf('(')).Trim();
                    }

                    resultTitle = new Regex("[^ a-zA-Z0-9]").Replace(resultTitle.Normalize(NormalizationForm.FormD), "");
                    resultArtistName = new Regex("[^ a-zA-Z0-9]").Replace(resultArtistName.Normalize(NormalizationForm.FormD), "");

                    if (resultTitle.ToLower().Contains(normalizedSongName) &&
                        resultArtistName.ToLower().Contains(normalizedArtistName))
                    {
                        artistName = result.Artist;

                        foreach (ReleaseResult release in result.Releases)
                        {
                            if (release.Type != "Single" && release.Type != "Album") { continue; }
                            if (release.Date > releaseDate) { continue; }
                            if (!release.HasDate) { continue; }

                            albumName = release.Title;
                            releaseType = release.Type;
                            releaseDate = release.Date;
                        }
                    }
                }

                if (albumName != null)
                {
                    albumName = albumName.Replace('’', '\'');

                    Page page = null;

                    if (releaseType == "Album")
                    {
                        page = new Page(albumName + " (" + artistName + " album)");
                        if (!page.Exists) { page = new Page(albumName + " (album)"); }
                        if (!page.Exists) { page = new Page(albumName); }
                        if (!page.Exists) { page = new Page(albumName + " (UK series)"); }
                    }
                    else if (releaseType == "Single")
                    {
                        page = new Page(albumName + " (" + artistName + " song)");
                        if (!page.Exists) { page = new Page(albumName + " (song)"); }
                        if (!page.Exists) { page = new Page(albumName); }
                    }

                    if (page.Exists)
                    {
                        albumInfo = page.AlbumInfobox;
                        singleInfo = page.SingleInfobox;
                    }
                }
            }

            string videoInfoMessage = "YouTube: " + videoInfo.Title + " [" + formattedDuration + "]";

            List<string> blockedCountries = new List<string>();
            if (!videoInfo.CountryRestriction.IsCountryAllowed("BR"))
            {
                blockedCountries.Add("Brazil");
            }
            if (!videoInfo.CountryRestriction.IsCountryAllowed("DE"))
            {
                blockedCountries.Add("Germany");
            }
            if (!videoInfo.CountryRestriction.IsCountryAllowed("FI"))
            {
                blockedCountries.Add("Finland");
            }
            if (!videoInfo.CountryRestriction.IsCountryAllowed("JP"))
            {
                blockedCountries.Add("Japan");
            }
            if (!videoInfo.CountryRestriction.IsCountryAllowed("KP"))
            {
                blockedCountries.Add("North Korea");
            }
            if (!videoInfo.CountryRestriction.IsCountryAllowed("RU"))
            {
                blockedCountries.Add("Russia");
            }
            if (!videoInfo.CountryRestriction.IsCountryAllowed("SK"))
            {
                blockedCountries.Add("South Korea");
            }
            if (!videoInfo.CountryRestriction.IsCountryAllowed("GB"))
            {
                blockedCountries.Add("the United Kingdom");
            }
            if (!videoInfo.CountryRestriction.IsCountryAllowed("US"))
            {
                blockedCountries.Add("the United States");
            }

            if (blockedCountries.Count == 1)
            {
                videoInfoMessage += "\n        Blocked in " + blockedCountries[0] + ".";
            }
            else if (blockedCountries.Count > 1)
            {
                videoInfoMessage += "\n        Blocked in ";
                for (int i = 0; i < blockedCountries.Count - 1; i++)
                {
                    if (i > 0)
                    {
                        videoInfoMessage += ", ";
                    }
                    videoInfoMessage += blockedCountries[i];
                }
                videoInfoMessage += " and " + blockedCountries[blockedCountries.Count - 1] + ".";
            }

            if (albumInfo != null)
            {
                videoInfoMessage += "\n        Album: " + albumInfo.Name;
                videoInfoMessage += "\n        Genre: " + albumInfo.Genre;
                videoInfoMessage += "\n        Released: " + albumInfo.ReleaseDate.Year;
            }
            else if (singleInfo != null)
            {
                videoInfoMessage += "\n        Genre: " + singleInfo.Genre;
                videoInfoMessage += "\n        Released: " + singleInfo.ReleaseDate.Year;
            }

            messageSink(videoInfoMessage);
        }
    }
}