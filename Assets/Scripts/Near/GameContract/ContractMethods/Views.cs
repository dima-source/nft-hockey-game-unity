using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Near.Models.Game.Bid;
using Near.Models.Game.Team;
using Near.Models.Game.TeamIds;
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