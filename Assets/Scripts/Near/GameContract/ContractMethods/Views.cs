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
        
        /// <returns>If user is not in the game returns -1</returns>
        public static async Task<int> GetGameId()
        {
            AvailableGame userGame = await GetUserGame();

            if (userGame == null)
            {
                return -1;
            }
            
            return userGame.GameId;
        }
        
        public static async Task<AvailableGame> GetUserGame()
        {
            string accountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
            AvailableGame userGame = (await GetAvailableGames())
                .FirstOrDefault(x => x.PlayerIds.Item1 == accountId || x.PlayerIds.Item2 == accountId);

            return userGame;
        }

        private static async Task<List<AvailableGame>> GetAvailableGames()
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();

            dynamic args = new ExpandoObject();
            args.from_index = 0;
            args.limit = 50;

            dynamic dynamicAvailableGames = await gameContract.View("get_available_games", args);

            return ParseAvailableGames(dynamicAvailableGames);
        }

        private static List<AvailableGame> ParseAvailableGames(dynamic dynamicAvailableGames)
        {
            List<dynamic> availableGamesResults = JsonConvert.DeserializeObject<List<dynamic>>(
                dynamicAvailableGames.result.ToString()
            );

            List<AvailableGame> availableGames = new List<AvailableGame>(); 
            foreach (dynamic availableGamesResult in availableGamesResults)
            {
                availableGames.Add(ParseAvailableGame(availableGamesResult));
            }

            return availableGames;
        }

        private static AvailableGame ParseAvailableGame(dynamic availableGameDynamic)
        {
            int gameId = availableGameDynamic[0];
            string playerId1 = availableGameDynamic[1][0].ToString();
            string playerId2 = availableGameDynamic[1][1].ToString();
                
            return new AvailableGame()
            {
                GameId = gameId, 
                PlayerIds = new Tuple<string, string>(playerId1, playerId2)
            }; 
        }
        
        public static async Task<IEnumerable<Opponent>> GetAvailablePlayers()
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();

            dynamic args = new ExpandoObject();
            args.from_index = 0;
            args.limit = 50;

            dynamic opponents = await gameContract.View("get_available_players", args);

            return ParseOpponents(opponents);
        }

        private static IEnumerable<Opponent> ParseOpponents(dynamic dynamicOpponents)
        {
            List<List<object>> opponentObjects = JsonConvert
                .DeserializeObject<List<List<object>>>(dynamicOpponents.result.ToString());
            
            var result = opponentObjects.Select(o => new Opponent
            {
                Name = (string) o[0],
                GameConfig = JsonConvert.DeserializeObject<GameConfig>(o[1].ToString())
            }); 
            
            return result; 
        }
        
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
                        .AddField(p => p.id))
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
                    .AddField(u => u.id)
                    .AddField(u => u.deposit))
                .AddField(p => p.requests_play_received, sq => sq
                    .AddField(u => u.id)
                    .AddField(u => u.deposit));

            string responseJson = await GetJSONQuery(query.Build());
            
            Debug.Log(responseJson);
            
            var GetUsers = JsonConvert.DeserializeObject<List<User>>(responseJson, new UserGameContractConverter());
            
            if (GetUsers == null)
            {
                return new List<User>();
            }

            return GetUsers;
        }
        public static async Task<bool> IsAlreadyInTheList()
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            args.account_id = NearPersistentManager.Instance.WalletAccount.GetAccountId();

            dynamic isInTheList = await gameContract.View("is_already_in_the_waiting_list", args);
            
            return bool.Parse(isInTheList.result);
        }

        public static async Task<GameConfig> GetGameConfig()
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            args.account_id = NearPersistentManager.Instance.WalletAccount.GetAccountId();

            dynamic gameConfig = await gameContract.View("get_game_config", args);
            
            return JsonConvert.DeserializeObject<GameConfig>(gameConfig.result);
        }
        
        public static async Task<List<GameData>> GetGames(GameDataFilter filter)
        {
            // TODO
            IQuery<GameData> query = new Query<GameData>("games")
                .AddArguments(new { where = filter })
                .AddField(p => p.id)
                .AddField(p => p.reward)
                .AddField(p => p.winner_index)
                .AddField(p => p.last_event_generation_time)
                .AddField(p => p.turns)
                .AddField(p => p.zone_number)
                .AddField(p => p.events, Event.GetQuery)
                .AddField(p=>p.player_with_puck,
                    FieldPlayer.GetQuery)
                .AddField(p=>p.user1,
                    UserInGameInfo.GetQuery)
                .AddField(p=>p.user2,
                    UserInGameInfo.GetQuery);
              
            string responseJson = await GetJSONQuery(query.Build());
            
            Debug.Log(responseJson);
            
            var gameDatas = JsonConvert.DeserializeObject<List<GameData>>(responseJson, new GameDataConverter());
            
            if (gameDatas == null)
            {
                return new List<GameData>();
            }

            return gameDatas;
        }
        
        private static string CalculateIdFieldPlayer(TeamIds teamIds, string numberFive, string playerPosition)
        {
            string nftId = "-1"; 
            if (teamIds.fives.ContainsKey(numberFive) &&
                teamIds.fives[numberFive].field_players.ContainsKey(playerPosition))
            {
                nftId = teamIds.fives[numberFive].field_players[playerPosition];
            }

            return nftId;
        }
        
        private static string CalculateIdGoalie(TeamIds teamIds, string number)
        {
            string nftId = "-1"; 
            if (teamIds.goalies.ContainsKey(number))
            {
                nftId = teamIds.goalies[number];
            }

            return nftId;
        }

        public static async Task<Team> LoadUserTeam()
        {
            throw new NotImplementedException();
        }
    }
}