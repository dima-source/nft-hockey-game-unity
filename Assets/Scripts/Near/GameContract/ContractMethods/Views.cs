using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Query.Builder;
using Near.MarketplaceContract.Parsers;
using Near.Models.Game;
using Near.Models.Tokens.Filters;
using Near.Models.Tokens.Players.FieldPlayer;
using Newtonsoft.Json;
using Event = Near.Models.Game.Event;

namespace Near.GameContract.ContractMethods
{
    public static class Views
    {
        private const string Url = "https://api.thegraph.com/subgraphs/name/nft-hockey/game";

        private static async Task<string> GetJSONQuery(string json)
        {
            json = "{\"query\": \"{" + json.Replace("\"", "\\\"") + "}\"}";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(Url, content);

            return await response.Content.ReadAsStringAsync();
        }


        /// <returns>If user is not in the game returns null</returns>
        public static async Task<User> GetUserInGame()
        {
            var accountId = NearPersistentManager.Instance.GetAccountId();

            var filter = new UserFilter()
            {
                id = accountId
            };
            
            var gamePagination = new Pagination
            {
                orderDirection = OrderDirection.desc,
                orderBy = "id"
            };
            
            var users = await GetUsers(filter, gamePagination);
            if (users.Count != 1)
            {
                throw new Exception("Can't find ");
            }

            if (users[0].games[0].winner_index != null)
            {
                return null;
            }
            
            return users.Count == 0 ? null : users[0];
        }
        public static async Task<User> GetUser()
        {
            var accountId = NearPersistentManager.Instance.GetAccountId();
            var filter = new UserFilter()
            {
                id = accountId
            };

            var users = await GetUsers(filter);

            if (users.Count != 1)
            {
                throw new Exception($"User id {accountId} not found");
            }

            return users.First();
        }

        public static async Task<List<User>> GetUsers(UserFilter filter, Pagination gamePagination = null)
        {
            var query = new Query<User>("users")
                .AddArguments(new {where = filter})
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
            
            if (gamePagination == null)
            {
                query.AddField(p => p.games,
                    sq => sq
                        .AddField(g => g.id)
                        .AddField(g => g.winner_index));
            }
            else
            {
                query.AddField(p => p.games,
                    sq => sq
                        .AddArguments(gamePagination)
                        .AddField(g => g.id)
                        .AddField(g => g.winner_index));
            }
            
            var responseJson = await GetJSONQuery(query.Build());

            var getUsers = JsonConvert.DeserializeObject<List<User>>(responseJson, new UserGameContractConverter());

            return getUsers ?? new List<User>();
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

        private static async Task<List<GameData>> GetGames(GameDataFilter filter, Pagination pagination = null)
        {
            var query = new Query<GameData>("games")
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

            var responseJson = await GetJSONQuery(query.Build());

            var gameDatas = JsonConvert.DeserializeObject<List<GameData>>(responseJson, new GameDataConverter());

            return gameDatas ?? new List<GameData>();
        }
    }
}