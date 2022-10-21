using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Query.Builder;
using Near.MarketplaceContract.Parsers;
using Near.Models.Game;
using Near.Models.Game.Bid;
using Near.Models.Game.Team;
using Near.Models.Game.TeamIds;
using Near.Models.Tokens.Filters;
using Near.Models.Tokens.Players.FieldPlayer;
using NearClientUnity;
using Newtonsoft.Json;
using UnityEngine;
using Event = Near.Models.Game.Event;

namespace Near.GameContract.ContractMethods
{
    public static class Views
    {
        public const string Url = "https://api.thegraph.com/subgraphs/name/nft-hockey/game";
        
        public static async Task<string> GetJSONQuery(string json)
        {
            json = "{\"query\": \"{" + json.Replace("\"", "\\\"") + "}\"}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(Url, content);

                return await response.Content.ReadAsStringAsync();
            }
        }


        /// <returns>If user is not in the game returns null</returns>
        public static async Task<User> GetUserInGame()
        {
            string accountId = NearPersistentManager.Instance.GetAccountId();

            UserFilter filter = new UserFilter()
            {
                id = accountId
            };

            List<User> users = await GetUsers(filter);
            if (users.Count != 1)
            {
                throw new Exception("Can't find ");
            }

            if (users.Count == 0)
            {
                return null;
            }

            return users[0];
        }
        public static async Task<User> GetUser()
        {
            string accountId = NearPersistentManager.Instance.GetAccountId();
            UserFilter filter = new UserFilter()
            {
                id = accountId
            };

            List<User> users = await GetUsers(filter);

            if (users.Count != 1)
            {
                throw new Exception($"User id {accountId} not found");
            }

            return users.First();
        }

        public static async Task<List<User>> GetUsers(UserFilter filter)
        {
            IQuery<User> query = new Query<User>("users")
                .AddArguments(new {where = filter})
                .AddField(p => p.games,
                    sq => sq
                        .AddField(g => g.id)
                        .AddField(g => g.winner_index))
                .AddField(p => p.id)
                .AddField(p => p.is_available)
                .AddField(p => p.deposit)
                .AddField(p => p.friends, sq => sq
                    .AddField(u => u.id))
                .AddField(p => p.sent_friend_requests, sq => sq
                    .AddField(u => u.id))
                .AddField(p => p.friend_requests_received, sq => sq
                    .AddField(u => u.id))
                .AddField(u => u.sent_requests_play, sq => sq
                    .AddField(u => u.from)
                    .AddField(u => u.deposit)
                    .AddField(u => u.to))
                .AddField(p => p.requests_play_received, sq => sq
                    .AddField(u => u.from)
                    .AddField(u => u.deposit)
                    .AddField(u => u.to));

            string responseJson = await GetJSONQuery(query.Build());

            Debug.Log(responseJson);

            var GetUsers = JsonConvert.DeserializeObject<List<User>>(responseJson, new UserGameContractConverter());

            if (GetUsers == null)
            {
                return new List<User>();
            }

            return GetUsers;
        }

        public static async Task<GameData> GetGame(GameDataFilter filter)
        {
            var games = await GetGames(filter);
            if (games.Count == 1)
            {
                return games[0];
            }

            throw new Exception("Can't find game");
        }

        public static async Task<List<GameData>> GetGames(GameDataFilter filter, Pagination pagination = null)
        {
            IQuery<GameData> query = new Query<GameData>("games")
                .AddArguments(new {where = filter})
                .AddField(p => p.id)
                .AddField(p => p.reward)
                .AddField(p => p.winner_index)
                .AddField(p => p.last_event_generation_time)
                .AddField(p => p.turns)
                .AddField(p => p.zone_number)
                .AddField(p => p.events, Event.GetQuery)
                .AddField(p => p.player_with_puck,
                    FieldPlayer.GetQuery)
                .AddField(p => p.user1,
                    UserInGameInfo.GetQuery)
                .AddField(p => p.user2,
                    UserInGameInfo.GetQuery);

            if (pagination != null)
            {
                query.AddArguments(pagination);
            }

            string responseJson = await GetJSONQuery(query.Build());

            Debug.Log(responseJson);

            var gameDatas = JsonConvert.DeserializeObject<List<GameData>>(responseJson, new GameDataConverter());

            if (gameDatas == null)
            {
                return new List<GameData>();
            }

            return gameDatas;
        }
    }
}