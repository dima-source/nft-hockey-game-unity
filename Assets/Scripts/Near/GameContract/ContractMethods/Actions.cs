using System;
using System.Dynamic;
using System.Threading.Tasks;
using Near.Models.Game.Team;
using NearClientUnity;
using UI.Profile.Models;
using UnityEngine;


namespace Near.GameContract.ContractMethods
{
    public static class Actions
    {
        public static async void StartGame(string opponentId, string deposit)
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            args.opponent_id = opponentId;
            
            await gameContract.Change("start_game", args,
                NearUtils.GasMakeAvailable,
                NearUtils.ParseNearAmount(deposit));
        }
        
        public static async Task<dynamic> MakeAvailable(string bid)
        {
            var gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            args.config = new object();

            var result = await gameContract.Change("make_available", args, 
                NearUtils.GasMakeAvailable,
                NearUtils.ParseNearAmount(bid));

            if (result == "null")
            {
                throw new Exception("ManageTeam");
            }

            return result;
        }

        public static async Task<bool> MakeUnavailable(string bid)
        {
            var gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            args.bid = bid + "000000000000000000000000";
            
            try
            {
                await gameContract.Change("make_unavailable", args);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }

            return true;
        }

        // TODO: fix usage of TeamLogo from Profile module. Here must be used either custom TeamLogo that
        // TODO: duplicates one from Profile or TeamLogo from Profile's must be declared in another module 
        public static async Task<bool> SetTeamLogo(TeamLogo teamLogo)
        {
            var gameContract = await NearPersistentManager.Instance.GetGameContract(); 
            dynamic args = new ExpandoObject();
            args.logo_json = Newtonsoft.Json.JsonConvert.SerializeObject(teamLogo);
            try
            {
                await gameContract.Change("set_team_logo", args);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }

            return true;
        }

        public static async Task GenerateEvent(int gameId)
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            args.game_id = gameId;

            try
            {
                await gameContract.Change("generate_event", args, NearUtils.GasMove);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static async Task TakeTO(int gameId)
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            //args.game_id = gameId;
            
            await gameContract.Change("take_to", args, NearUtils.GasMove);
        }
        
        public static async Task CoachSpeech(int gameId)
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            //args.game_id = gameId;
            
            await gameContract.Change("coach_speech", args, NearUtils.GasMove);
        }
        
        public static async Task GoalieOut(int gameId)
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            //args.game_id = gameId;
            
            await gameContract.Change("coach_speech", args, NearUtils.GasMove);
        } 
        
        public static async Task GoalieBack(int gameId)
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            //args.game_id = gameId;
            
            await gameContract.Change("goalie_back", args, NearUtils.GasMove);
        } 
        
        public static async void ChangeLineups(Team team)
        {
            ContractNear nftContract = await NearPersistentManager.Instance.GetNftContract();

            /*
            List<dynamic> fives = ConvertFives(team.Fives);

            if (fives.Count != 0)
            {
                dynamic args = new ExpandoObject();
                args.fives = fives;
                
                await nftContract.Change("insert_nft_field_players", args, NearUtils.GasMakeAvailable);
            }

            List<List<string>> goalies = ConvertGoalies(team.Goalies);

            if (goalies.Count != 0)
            {
                dynamic args = new ExpandoObject();
                args.goalies = goalies;
                
                await nftContract.Change("insert_nft_goalies", args, NearUtils.GasMakeAvailable);
            }
            */
        }

        public static async Task<bool> RegisterAccount()
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();

            try
            {
                await gameContract.Change("register_account", args, NearUtils.GasMove);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }

            return true;
        }

        /// <param name="friendId">
        /// "send_friend_request", "accept_friend_request", "decline_friend_request", "remove_friend"
        /// </param>
        public static async Task SendFriendRequest(string friendId, string methodName)
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            args.friend_id = friendId;

            await gameContract.Change(methodName, args, NearUtils.GasMove);
        }

        public static async Task SendRequestPlay(string friendId, string deposit)
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            args.friend_id = friendId;

            await gameContract.Change("send_request_play", args, NearUtils.GasMove,
                NearUtils.ParseNearAmount(deposit));
        }

        public static async Task AcceptRequestPlay(string friendId, string deposit)
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            args.friend_id = friendId;
            
            await gameContract.Change(
                "accept_request_play",
                args,
                NearUtils.GasMove,
                NearUtils.ParseNearAmount(deposit)
            );
        }
        
        public static async Task DeclineRequestPlay(string friendId, string deposit)
        {
            ContractNear gameContract = await NearPersistentManager.Instance.GetGameContract();
                
            dynamic args = new ExpandoObject();
            args.friend_id = friendId;

             gameContract.Change(
                 "decline_request_play", 
                 args, 
                 NearUtils.GasMove
             );
        } 
    }
}