using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Near.MarketplaceContract;
using Near.Models;
using Near.Models.Game.Bid;
using Near.Models.ManageTeam.Team;
using Near.Models.Team.Team;
using Near.Models.Team.TeamIds;
using NearClientUnity;
using Newtonsoft.Json;

namespace Near.GameContract.ContractMethods
{
    public static class Views
    {
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
        
        public static async Task<Team> LoadUserTeam()
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
            ContractNear nftContract = await NearPersistentManager.Instance.GetNftContract();
            
            dynamic args = new ExpandoObject();
            args.account_id = NearPersistentManager.Instance.GetAccountId();
            
            dynamic ownerTeamResult = await gameContract.Change("get_owner_team", args, NearUtils.Gas);
            dynamic ownerTeamIdsResult = await nftContract.View("get_owner_nft_team", args);

            return ParseAndMergeTeamAndTeamIds(ownerTeamResult, ownerTeamIdsResult);
        }

        private static Team ParseAndMergeTeamAndTeamIds(dynamic teamDynamic, dynamic teamIdsDynamic)
        {
            if (teamDynamic == null)
            {
                return new Team();
            }
            
            TeamIds teamIds = JsonConvert.DeserializeObject<TeamIds>(teamIdsDynamic.result);
            
            return new Team()
            {
                Fives = ParseFives(teamDynamic["fives"], teamIds),
                Goalies = ParseGoalies(teamDynamic["goalies"], teamIds)
            };
        }

        private static Dictionary<string, Five> ParseFives(dynamic fivesDynamic, TeamIds teamIds)
        {
            Dictionary<string, Five> fives = new Dictionary<string, Five>();

            foreach (dynamic fiveResult in fivesDynamic)
            {
                Five five = ParseFive(fiveResult, teamIds);
                fives.Add(five.Number, five);
            }

            return fives;
        }
        
        private static Five ParseFive(dynamic fiveDynamic, TeamIds teamIds)
        {
            Five result = new Five();
            
            dynamic fiveChild = fiveDynamic.Children();
                            
            result.Number = fiveDynamic.Name;
            
            foreach (dynamic iceTimePriority in fiveChild["ice_time_priority"])
            {
                result.IceTimePriority = iceTimePriority.ToString();
            }

            result.FieldPlayers = ParseFieldPlayers(fiveDynamic, teamIds);
            return result;
        }

        private static Dictionary<string, NFTMetadata> ParseFieldPlayers(dynamic fiveDynamic, TeamIds teamIds)
        {
            Dictionary<string, NFTMetadata> fieldPlayers = new Dictionary<string, NFTMetadata>(); 
            
            dynamic fiveChild = fiveDynamic.Children();
            foreach (dynamic fieldPlayersResults in fiveChild["field_players"])
            {
                foreach (dynamic fieldPlayerResults in fieldPlayersResults)
                {
                    string playerPosition = fieldPlayerResults.Name;
                    fieldPlayers.Add(playerPosition, ParseFieldPlayer(fiveDynamic, fieldPlayersResults, teamIds));
                }
            }

            return fieldPlayers;
        }

        private static NFTMetadata ParseFieldPlayer(dynamic fiveDynamic, dynamic fieldPlayersDynamicResults, TeamIds teamIds)
        {
            NFTMetadata result = new NFTMetadata();
            
            foreach (dynamic fieldPlayerDynamicResult in fieldPlayersDynamicResults)
            {
                string playerPosition = fieldPlayerDynamicResult.Name;

                foreach (dynamic fieldPlayerDynamic in fieldPlayerDynamicResult)
                {
                    result = new NFTMetadata()
                    {
                        Id = CalculateIdFieldPlayer(teamIds, fiveDynamic.Name, playerPosition),
                        Metadata = ParseMetadata(fieldPlayerDynamic)
                    };
                }
            }

            return result;
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
        
        private static Metadata ParseMetadata(dynamic card)
        {
            return new Metadata()
            {
                title = card["title"].ToString(),
                description = card["description"].ToString(),
                media = card["media"].ToString(),
                media_hash = card["media_hash"].ToString(),
                issued_at = card["issued_at"].ToString(),
                expires_at = card["expires_at"].ToString(),
                starts_at = card["starts_at"].ToString(),
                updated_at = card["updated_at"].ToString(),
               // extra = JsonConvert.DeserializeObject<Extra>(
                 //   card["extra"].ToString(), new ExtraConverter())
            };
        }
        
        private static Dictionary<string, NFTMetadata> ParseGoalies(dynamic goaliesDynamic, TeamIds teamIds) 
        {
            Dictionary<string, NFTMetadata> result  = new Dictionary<string, NFTMetadata>();
            
            foreach (dynamic goalieResults in goaliesDynamic)
            {
                string number = goalieResults.Name;
                result.Add(number, ParseGoalie(goalieResults.Children(), teamIds, number));
            }

            return result;
        }

        private static NFTMetadata ParseGoalie(dynamic goalieDynamic, TeamIds teamIds, string number)
        {
            NFTMetadata result = new NFTMetadata();
            
            foreach (dynamic goalieResult in goalieDynamic)
            {
                result = new NFTMetadata()
                {
                    Id = CalculateIdGoalie(teamIds, number),
                    Metadata = ParseMetadata(goalieResult)
                };
            }

            return result;
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
    }
}