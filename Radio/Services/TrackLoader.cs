namespace Radio.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Apis.YouTube.v3;
    using Google.Apis.YouTube.v3.Data;

    using Microsoft.Extensions.FileProviders;

    using Newtonsoft.Json;

    using Radio.Models;

    public class TrackLoader : ITrackLoader
    {
        private readonly IFileProvider _fileProvider;

        private readonly HttpClient _httpClient;

        private readonly YouTubeService _youTubeApiService;

        public TrackLoader(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
            _httpClient = new HttpClient();

            //using (Stream clientSecretsStream = _fileProvider.GetFileInfo("Client Secret.json").CreateReadStream())
            //{
            //    _youTubeApiService = new YouTubeService(new BaseClientService.Initializer
            //    {
            //        HttpClientInitializer = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(clientSecretsStream).Secrets,
            //                                                                            new string[]
            //                                                                            {
            //                                                                                YouTubeService.Scope.YoutubeReadonly
            //                                                                            },
            //                                                                            "user",
            //                                                                            CancellationToken.None).Result
            //    });
            //}
        }

        public IEnumerable<Track> LoadAllTracks()
        {
            return new Track[]
            {
                new Track
                {
                    Id = "0yi5e25Fd-g",
                    Length = TimeSpan.Parse("0:4:14"),
                    Title = "[DnB] - Botnek & I See MONSTAS - Deeper Love [Monstercat Release]"
                },
                new Track
                {
                    Id = "etHVz3ybuTc",
                    Length = TimeSpan.Parse("0:4:24"),
                    Title = "[Glitch Hop] - Stonebank - Chokehold (feat. Concept) [Monstercat EP Release]"
                },
                new Track
                {
                    Id = "1wwHkaO9_Gk",
                    Length = TimeSpan.Parse("0:2:33"),
                    Title = "Topi - Backup (Tisoki Remix) [Monstercat FREE Release]"
                }
            };

            // return LoadAllTracksAsync().GetAwaiter().GetResult();
        }

        private async Task<IEnumerable<Track>> LoadAllTracksAsync()
        {
            string[] trackNames = File.ReadAllLines(_fileProvider.GetFileInfo("Tracks").PhysicalPath);
            int length = Math.Max(trackNames.Length, 5);

            Track[] tracks = new Track[length];

            for (int trackIndex = 0; trackIndex < length; ++trackIndex)
            {
                tracks[trackIndex] = await QueryVideoByName(trackNames[trackIndex]);
            }

            return tracks.Where(track => track != null);
        }

        private async Task<Track> QueryVideoByName(string name)
        {
            SearchResource.ListRequest searchRequest = _youTubeApiService.Search.List("snippet");
            searchRequest.Q = name;
            searchRequest.MaxResults = 1;

            SearchListResponse searchResponse = await searchRequest.ExecuteAsync();

            SearchResult result = searchResponse.Items.FirstOrDefault(item => item.Id.Kind.Contains("video"));

            if (result == null)
            {
                return null;
            }

            return new Track
            {
                Id = result.Id.VideoId,
                Title = result.Snippet.Title,
                Length = await QueryVideoDuration(result.Id.VideoId)
            };
        }

        private async Task<TimeSpan> QueryVideoDuration(string id)
        {
            string contentDetails = await _httpClient.GetStringAsync($"https://www.googleapis.com/youtube/v3/videos?id={id}&key=&part=contentDetails");

            string durationString = JsonConvert.DeserializeObject<VideoListResponse>(contentDetails)
                                                      .Items
                                                      .Single()
                                                      .ContentDetails
                                                      .Duration;

            return XmlConvert.ToTimeSpan(durationString);
        }
    }
}