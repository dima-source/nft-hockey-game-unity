using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Near;
using Near.Models.Game.TeamIds;
using Near.Models.Tokens;
using Near.Models.Tokens.Filters;
using Near.Models.Tokens.Players;
using Near.Models.Tokens.Players.FieldPlayer;
using Runtime;
using TMPro;
using UI.ManageTeam.DragAndDrop;
using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ManageTeam
{
    
    public class ManageTeamView : MonoBehaviour
    {
        
        private ManageTeamController _controller;

        public Transform forwardsCanvasContent;
        
        [SerializeField] private List<UISlot> Goalies = new();

        public List<Token> UserNfts;
        
        [SerializeField] public Transform canvasContent;
        [SerializeField] public Bench fieldPlayersBenchContent;
        [SerializeField] public Bench goaliesBenchContent;
        [SerializeField] public Bench powerPlayersBenchContent;
        [SerializeField] public Bench penaltyKillBenchContent;

        [SerializeField] private TMP_Dropdown tactictsDropdown;
        [SerializeField] private Text iceTimePriority;
        [SerializeField] private Slider iceTimePrioritySlider;
        [SerializeField] public TeamView teamView;

        private Dictionary<LineNumbers, string> _fivesTactics = new();
        private Dictionary<LineNumbers, string> _fivesIceTimePriority = new();

        [SerializeField] private TMP_Text _teamworkText;

        public Bench CurrentBench
        {
            get
            {
                var activeBenches = GetComponentsInChildren<Bench>().Where((bench) => bench.gameObject.activeSelf).ToList();
                if (activeBenches.Count() != 1)
                    throw new ApplicationException($"Must be only one active bench. Got {activeBenches.Count()}");
                return activeBenches.First();
            }
        }

        public TeamIds Team;
        private LineNumbers _currentLineNumber;
        
        private void Awake()
        {
            teamView = Scripts.Utils.FindChild<TeamView>(transform, "Team");
            _controller = new ManageTeamController();
        }

        private void InitTopPanels()
        {
            if (Team.fives.Count == 0)
            {
                return;
            }
            foreach (var five in Team.fives.Values)
            {
                _fivesTactics.Add(Utils.Utils.StringToLineNumber(five.number), five.tactic);
                _fivesIceTimePriority.Add(Utils.Utils.StringToLineNumber(five.number), five.ice_time_priority);
            }
        }

        private async void Start()
        {
            PlayerFilter filter = new()
            {
                ownerId = NearPersistentManager.Instance.GetAccountId()
            };
            Pagination pagination = new()
            {
                first = 100
            };
            UserNfts = await _controller.LoadUserNFTs(filter, pagination);
            Team = await _controller.LoadUserTeam();
            teamView.InitFives();
            teamView.InitGoalies();
            InitTopPanels();

            _currentLineNumber = LineNumbers.First;
            teamView.ShowLine(_currentLineNumber);
            InitBenches();
            fieldPlayersBenchContent.gameObject.SetActive(true);
            
            UpdateTeamWork();
        }

        // public void ShowLine(string number)
        // {
        //     if (number == "G")
        //     {
        //         _teamworkText.text = "";
        //         return;
        //     }
        //     LineNumbers parsedLine = Utils.Utils.StringToLineNumber(number);
        //     teamView.ShowLine(parsedLine);
        //
        //     _currentLineNumber = parsedLine;
        //
        //     // TODO: move working with tactics to another place
        //     var alreadySet = _fivesTactics.TryGetValue(_currentLineNumber, out string tactic);
        //     if (alreadySet)
        //     {
        //         var value = tactic switch
        //         {
        //             "Safe" => 1,
        //             "Defensive" => 2,
        //             "Neutral" => 3,
        //             "Offensive" => 4,
        //             "Aggressive" => 5
        //         };
        //         tactictsDropdown.SetValueWithoutNotify(value);
        //     }
        //
        //     alreadySet = _fivesIceTimePriority.TryGetValue(_currentLineNumber, out string priority);
        //     if (alreadySet)
        //     {
        //         
        //         int value = priority switch
        //         {
        //             "SuperLowPriority" => 1,
        //             "LowPriority" => 2,
        //             "Normal" => 3,
        //             "HighPriority" => 4,
        //             "SuperHighPriority" => 5
        //         };
        //         iceTimePrioritySlider.SetValueWithoutNotify(value);
        //         iceTimePriority.text = Utils.Utils.PascalToCapitalized(priority);
        //     }
        //     else
        //     {
        //         _fivesIceTimePriority.Add(_currentLineNumber, Utils.Utils.GetIceTimePriority(3));
        //         iceTimePrioritySlider.SetValueWithoutNotify(3);
        //         iceTimePriority.text = Utils.Utils.PascalToCapitalized(Utils.Utils.GetIceTimePriority(3)); 
        //     }
        //     
        //     // TODO: move working with teamwork to another place
        //     UpdateTeamWork();
        // }
        
        private void InitBenches()
        {
            if (Team.fives.Count == 0)
            {
                List<Token> fieldPlayers = UserNfts.Where(x => x.player_type == "FieldPlayer").ToList();
                List<Token> goalies = UserNfts.Where(x => x.player_type == "Goalie").ToList();
                fieldPlayersBenchContent.Cards = fieldPlayers;
                goaliesBenchContent.Cards = goalies;
                return;
            }
            
            HashSet<string> fieldPlayersTokensInTeam = new();
            foreach (var five in Team.fives.Values)
            {
                foreach (var tokenId in five.field_players.Values.ToList())
                {
                    fieldPlayersTokensInTeam.Add(tokenId);
                }
            }
            
            List<string> goaliesTokensInTeam = new();
            foreach (var tokenId in Team.goalies.Values)
            {
                goaliesTokensInTeam.Add(tokenId);
            }
            foreach (var tokenId in Team.goalie_substitutions.Values)
            {
                goaliesTokensInTeam.Add(tokenId);
            }

            List<string> powerPlayersTokensInTeam = new();
            foreach (var tokenId in Team.fives[LineNumbers.PowerPlay1.ToString()].field_players.Values.ToList())
            {
                powerPlayersTokensInTeam.Add(tokenId);
            }
            foreach (var tokenId in Team.fives[LineNumbers.PowerPlay2.ToString()].field_players.Values.ToList())
            {
                powerPlayersTokensInTeam.Add(tokenId);
            }
            
            List<string> penaltyKillTokensInTeam = new();
            foreach (var tokenId in Team.fives[LineNumbers.PenaltyKill1.ToString()].field_players.Values.ToList())
            {
                penaltyKillTokensInTeam.Add(tokenId);
            }
            foreach (var tokenId in Team.fives[LineNumbers.PenaltyKill2.ToString()].field_players.Values.ToList())
            {
                penaltyKillTokensInTeam.Add(tokenId);
            }
            
            
            List<Token> fieldPlayersBench = UserNfts.Where(x => x.player_type == "FieldPlayer" &&
                                                                 !fieldPlayersTokensInTeam.Contains(x.tokenId) ).ToList();
            List<Token> goaliesBench = UserNfts.Where(x => x.player_type == "Goalie" && !goaliesTokensInTeam.Contains(x.tokenId)).ToList();
            goaliesBench.AddRange(UserNfts.Where(x => fieldPlayersTokensInTeam.Contains(x.tokenId) && 
                                                       !goaliesTokensInTeam.Contains(x.tokenId)));
            
            List<Token> powerPlayersBench = UserNfts.Where(x => fieldPlayersTokensInTeam.Contains(x.tokenId))
                .Where(x => !powerPlayersTokensInTeam.Contains(x.tokenId)).ToList();
            List<Token> penaltyKillBench = UserNfts.Where(x => fieldPlayersTokensInTeam.Contains(x.tokenId))
                .Where(x => !penaltyKillTokensInTeam.Contains(x.tokenId)).ToList();
            
            fieldPlayersBenchContent.Cards = fieldPlayersBench;
            goaliesBenchContent.Cards = goaliesBench;
            powerPlayersBenchContent.Cards = powerPlayersBench;
            penaltyKillBenchContent.Cards = penaltyKillBench;
            
        }

        // updates benches
        public void AddFieldPlayerToTeam(DraggableCard player)
        {
            goaliesBenchContent.Cards.Add(player.CardData);
            powerPlayersBenchContent.Cards.Add(player.CardData);
            penaltyKillBenchContent.Cards.Add(player.CardData);
        }

        // updates benches
        // TODO: move to TeamView
        public void RemoveFieldPlayerFromTeam(DraggableCard player)
        {
            try
            {
                goaliesBenchContent.RemoveSlotWithinPlayer(player);
            }
            catch (ApplicationException e)
            {
                Debug.Log("Field player wasn't in goalies bench");
                Debug.Log(e.Message);
            }
            
            // removing player from goalie slot if it is in it
            foreach (var goalieSlot in Goalies)
            {
                if (!goalieSlot.draggableCard)
                    continue;
                if (goalieSlot.draggableCard.CardData.tokenId == player.CardData.tokenId)
                {
                    // goalieSlot.uiPlayer.gameObject.SetActive(true);
                    Destroy(goalieSlot.draggableCard.gameObject);
                    goalieSlot.draggableCard = null;
                    break;
                }
            }
            
            try
            {
                powerPlayersBenchContent.RemoveSlotWithinPlayer(player);
            }
            catch (ApplicationException e)
            {
                Debug.Log("Field player wasn't in goalies bench");
                Debug.Log(e.Message);
            }
            
            // removing player from PowerPlay slot if it is in it
            var keys = new List<LineNumbers>{LineNumbers.PowerPlay1, LineNumbers.PowerPlay2};
            foreach (var key in keys)
            {
                foreach (var slot in teamView.Fives[key].Values.ToList())
                {
                    if (!slot.draggableCard)
                        continue;
                    if (slot.draggableCard.CardData.tokenId == player.CardData.tokenId)
                    {
                        Destroy(slot.draggableCard.gameObject);
                        slot.draggableCard = null;
                        break;
                    }
                }
            }
            
            try
            {
                penaltyKillBenchContent.RemoveSlotWithinPlayer(player);
            }
            catch (ApplicationException e)
            {
                Debug.Log("Field player wasn't in goalies bench");
                Debug.Log(e.Message);
            }
            
            // removing player from PenaltyKill slot if it is in it
            keys = new List<LineNumbers>{LineNumbers.PenaltyKill1, LineNumbers.PenaltyKill2};
            foreach (var key in keys)
            {
                foreach (var slot in teamView.Fives[key].Values.ToList())
                {
                    if (!slot.draggableCard)
                        continue;
                    if (slot.draggableCard.CardData.tokenId == player.CardData.tokenId)
                    {
                        Destroy(slot.draggableCard.gameObject);
                        slot.draggableCard = null;
                        break;
                    }
                }
            }
        }

        public void OnChangeIceTimePriority()
        {
            string currentPriority = Utils.Utils.GetIceTimePriority((int) iceTimePrioritySlider.value);
            iceTimePriority.text = Utils.Utils.PascalToCapitalized(currentPriority);
            bool added = _fivesIceTimePriority.TryAdd(_currentLineNumber, currentPriority);
            if (!added)
            {
                _fivesIceTimePriority[_currentLineNumber] = currentPriority;
            }
        }

        public void OnChangeTactics()
        {
            string tactics;
            try
            {
                tactics = Utils.Utils.GetTactics(tactictsDropdown.value);
            }
            catch (SwitchExpressionException)
            {
                Debug.Log("Tactics not chosen");
                _fivesTactics.Remove(_currentLineNumber);
                return;
            }
            
            bool added = _fivesTactics.TryAdd(_currentLineNumber, tactics);
            if (!added)
            {
                _fivesTactics[_currentLineNumber] = tactics;
            }
        }

        public void UpdateTeamWork()
        {
            var playersSlots = teamView.Fives[_currentLineNumber].Values;
            if (playersSlots.Any(x => !x.gameObject.activeSelf || !x.draggableCard)) 
            {
                _teamworkText.text = "";
                return;
            }

            var players = playersSlots.Select(slot => slot.draggableCard);
            var playersData = players.Select(player => (FieldPlayer) player.CardData);
            Dictionary<DraggableCard, int> playersPercent = new();
            foreach (var player in players)
                playersPercent.Add(player, 100);

            int percentsSumBefore = playersPercent.Values.Aggregate((i, i1) => i + i1);

            foreach (var player in players)
            {
                var playerData = (FieldPlayer) player.CardData;
                var slot = player.uiSlot;
                if (playersData.Count(d => d.nationality == playerData.nationality) > 1)
                {
                    playersPercent[player] += 5;
                }

                var nativePostition = player.playerCardData.position;
                
                if (nativePostition.ToString() == "LW" &&
                    player.uiSlot.slotPosition.ToString() == "RW" ||
                    nativePostition.ToString() == "RW" &&
                    player.uiSlot.slotPosition.ToString() == "LW" ||
                    nativePostition.ToString() == "RD" &&
                    player.uiSlot.slotPosition.ToString() == "LD" ||
                    nativePostition.ToString() == "LD" &&
                    player.uiSlot.slotPosition.ToString() == "RD")
                {
                    playersPercent[player] -= 5;
                }
                else if (nativePostition.ToString() != "C" && player.uiSlot.slotPosition == SlotPositionEnum.Center)
                {
                    playersPercent[player] -= 25;
                } 
                else if (nativePostition.ToString() != slot.slotPosition.ToString())
                {
                    playersPercent[player] -= 20;
                }

                if (playerData.player_role == "DefensiveDefenseman" &&
                    playersData.Count(d => d.player_role == "OffensiveDefenseman") >= 1 ||
                    playerData.player_role == "OffensiveDefenseman" &&
                    playersData.Count(d => d.player_role == "DefensiveDefenseman") >= 1)
                {
                    playersPercent[player] -= 10;
                }
                
                if (playersData.Count(d => d.player_role is "ToughGuy" or "Enforcer") > 0)
                {
                    if (playerData.player_role is "Playmaker" or "Shooter")
                    {
                        playersPercent[player] += 20;
                    }
                    else
                    {
                        playersPercent[player] -= 10;
                    }
                }

                if (playersData.Count(d => d.player_role is "TryHarder") > 0)
                {
                        playersPercent[player] += 10;
                }
                if (playersData.Count(d => d.player_role is "TwoWay") > 0)
                {
                        playersPercent[player] += 10;
                }

                if (playersData.Count(d => d.player_role is "DefensiveForward") > 0 &&
                    (player.uiSlot.slotPosition == SlotPositionEnum.LeftDefender ||
                     player.uiSlot.slotPosition == SlotPositionEnum.RightDefender))
                {
                    playersPercent[player] += 20;
                }
            }
            int percentsSumAfter = playersPercent.Values.Aggregate((i, i1) => i + i1);
            if (percentsSumAfter < percentsSumBefore)
            {
                _teamworkText.text = "Low teamwork";
            } else if (percentsSumAfter == percentsSumBefore)
            {
                _teamworkText.text = "OK teamwork";
            } else if (percentsSumAfter > percentsSumBefore)
            {
                _teamworkText.text = "Great teamwork";
            }
        }

        public void ShowStatsChanges(DraggableCard player, bool switched = false)
        {
            var slot = player.uiSlot;
            if (Goalies.Contains(slot))
                return;
            
            var currentFive = teamView.Fives[_currentLineNumber].Values.ToList().Where(x => x != slot);
            var currentFiveFiltered = currentFive.Where(x => x.draggableCard);

            // if player moved to bench
            if (slot.slotPosition == SlotPositionEnum.Bench)
            {
                List<DraggableCard> sameNationalityPlayers = new();
                foreach (var uiSlot in teamView.Fives[_currentLineNumber].Values.ToList().Where(x => x != slot))
                {
                    if (!uiSlot.draggableCard)
                    {
                        continue;
                    }
                    if (((Player) uiSlot.draggableCard.CardData).nationality == ((Player) player.CardData).nationality)
                    {
                        sameNationalityPlayers.Add(uiSlot.draggableCard);
                    }
                }

                if (sameNationalityPlayers.Count == 1)
                {
                    sameNationalityPlayers.First().PlayStatsDown(5);
                    UpdateTeamWork();
                }

                return;
            }

            var userPosition = player.playerCardData.position.ToString();
            int percent = 100;
            
            // if player on it's position, doing nothing

            // if player in the other side. RHCP :)
            
            if (userPosition == "LW" && slot.slotPosition == SlotPositionEnum.RightWing ||
                userPosition == "RW" && slot.slotPosition == SlotPositionEnum.LeftWing ||
                userPosition == "RD" && slot.slotPosition == SlotPositionEnum.LeftDefender ||
                userPosition == "LD" && slot.slotPosition == SlotPositionEnum.RightDefender)
            {
                percent = 95;
            }
            // if not central player is in center
            else if (userPosition != "C" && slot.slotPosition == SlotPositionEnum.Center)
            {
                percent = 75;
            }
            // if player is just on another position
            else if (userPosition != String.Concat(slot.slotPosition.ToString().Select(c => Char.IsUpper(c))))
            {
                percent = 80;
            }
            
            Dictionary<DraggableCard, int> playersPercent = new();

            if (!switched)
            {
                int isDefensemanPair = 0;
                bool sameNationalityInFive = false;
                // natianality
                foreach (var uiSlot in currentFive)
                {
                    if (!uiSlot.draggableCard)
                    {
                        continue;
                    }
                    if (((Player) uiSlot.draggableCard.CardData).nationality == ((Player) player.CardData).nationality)
                    {
                        sameNationalityInFive = true;
                        playersPercent.Add(uiSlot.draggableCard, 5);
                        if (playersPercent.ContainsKey(uiSlot.draggableCard))
                            playersPercent[uiSlot.draggableCard] = 5;
                        else playersPercent.Add(uiSlot.draggableCard, 5);
                    }
                }
                
                // role 
                string player_role = ((Player) player.CardData).player_role;
                foreach (var uiSlot in currentFive)
                {
                    if (!uiSlot.draggableCard)
                    {
                        continue;
                    }
                    string uiPlayerRole = ((Player) uiSlot.draggableCard.CardData).player_role;
                    if (uiPlayerRole == "DefensiveDefenseman" 
                        && player_role == "OffensiveDefenseman" ||
                        uiPlayerRole == "OffensiveDefenseman" 
                        && player_role == "DefensiveDefenseman")
                    {
                        if (isDefensemanPair == 1)
                        {
                            isDefensemanPair = 2;
                            continue;
                        }

                        if (isDefensemanPair == 2)
                        {
                            continue;
                        }
                        isDefensemanPair = 1;
                        continue;
                    }
                    
                    if (isDefensemanPair == 1)
                    {
                        if (playersPercent.ContainsKey(uiSlot.draggableCard))
                            playersPercent[uiSlot.draggableCard] += 10;
                        else playersPercent.Add(uiSlot.draggableCard, 10);
                    }


                    if (player_role is "ToughGuy" or "Enforcer" && uiSlot.draggableCard)
                    {
                        if (uiPlayerRole is "Playmaker" or "Shooter")
                        {
                            if (playersPercent.ContainsKey(uiSlot.draggableCard))
                                playersPercent[uiSlot.draggableCard] += 20;
                            else playersPercent.Add(uiSlot.draggableCard, 20);
                        }
                        else
                        {
                            if (playersPercent.ContainsKey(uiSlot.draggableCard))
                                playersPercent[uiSlot.draggableCard] -= 10;
                            else playersPercent.Add(uiSlot.draggableCard, -10);
                            
                        }
                    }

                    if (player_role is "TryHarder" or "TwoWay")
                    {
                        if (playersPercent.ContainsKey(uiSlot.draggableCard))
                            playersPercent[uiSlot.draggableCard] += 10;
                        else playersPercent.Add(uiSlot.draggableCard, 10);
                    }

                    if (player_role is "DefensiveForward" && 
                        (uiSlot.slotPosition == SlotPositionEnum.LeftDefender ||
                        uiSlot.slotPosition == SlotPositionEnum.RightDefender))
                    {
                        if (playersPercent.ContainsKey(uiSlot.draggableCard))
                            playersPercent[uiSlot.draggableCard] += 20;
                        else playersPercent.Add(uiSlot.draggableCard, 20);
                    }
                }
                
                if (isDefensemanPair > 0)
                {
                    if (playersPercent.ContainsKey(player))
                        playersPercent[player] += 10;
                    else playersPercent.Add(player, 10);
                }

                if (currentFiveFiltered.Count(x => ((Player) x.draggableCard.CardData).player_role is "ToughGuy" or "Enforcer") > 0)
                {
                    if (((Player) player.CardData).player_role is "Playmaker" or "Shooter")
                    {
                        percent += 20;
                    }
                    else
                    {
                        percent -= 10;
                    }
                }

                int toughGuysAndEnforcers = currentFiveFiltered.Count(x =>
                    ((Player) x.draggableCard.CardData).player_role is "TryHarder" or "TwoWay");
                for (int i = 0; i < toughGuysAndEnforcers; i++)
                {
                        percent += 10;
                }

                if ((player.uiSlot.slotPosition == SlotPositionEnum.LeftDefender ||
                    player.uiSlot.slotPosition == SlotPositionEnum.RightDefender) && 
                    currentFiveFiltered.Count(x => ((Player) x.draggableCard.CardData).player_role is "DefensiveForward") > 0)
                {
                    percent += 20;
                }

                if (sameNationalityInFive)
                    percent += 5;
                
                foreach (var uiPlayer in playersPercent.Keys)
                {
                    int p = playersPercent[uiPlayer];
                    if (p > 0)
                    {
                        uiPlayer.PlayStatsUp(p);
                        UpdateTeamWork();
                    }
                    else if (p < 0)
                    {
                        uiPlayer.PlayStatsDown(p * -1);
                        UpdateTeamWork();
                    }
                }
            }
            
            if (percent > 100)
            {
                player.PlayStatsUp(percent - 100);
                UpdateTeamWork();
            } else if (percent < 100)
            {
                player.PlayStatsDown(100 - percent);
                UpdateTeamWork();
            }
        }

        // TODO: move building teamIds object to TeamView
        public async void SaveTeam()
        {
            List<string> fieldPlayers = new();
            TeamIds teamIds = new();
            foreach (var lineNumber in teamView.Fives.Keys)
            {
                FiveIds fiveIds = new();
                var playersOnPositions = teamView.Fives[lineNumber];
                foreach (var position in playersOnPositions.Keys)
                {

                    if (!playersOnPositions[position].draggableCard) // if ui player not set
                    {
                        OnNotEverythingSet($"{Utils.Utils.PascalToCapitalized(lineNumber.ToString())} line not fully set");
                        return;
                    }
                    fiveIds.field_players.Add(position.ToString(),
                        playersOnPositions[position].draggableCard.CardData.tokenId);
                    fieldPlayers.Add(playersOnPositions[position].draggableCard.CardData.tokenId);
                }

                bool added;
                added = _fivesTactics.TryGetValue(lineNumber, out string tactics);
                if (!added)
                {
                    OnNotEverythingSet($"Tactics not set for line \"{Utils.Utils.PascalToCapitalized(lineNumber.ToString())}\"");
                    return;
                }
                fiveIds.tactic = tactics;
                fiveIds.number = lineNumber.ToString();
                
                added = _fivesIceTimePriority.TryGetValue(lineNumber, out string iceTimePriorityValue);
                if (!added)
                {
                    OnNotEverythingSet($"Ice time priority not set for line \"{Utils.Utils.PascalToCapitalized(lineNumber.ToString())}\"");
                    return;
                }
                fiveIds.ice_time_priority = iceTimePriorityValue;
                
                teamIds.fives.Add(lineNumber.ToString(), fiveIds);
            }
            
            foreach (var goalieSlot in Goalies)
            {
                if (!goalieSlot.draggableCard)
                {
                    OnNotEverythingSet($"{Utils.Utils.PascalToCapitalized(goalieSlot.slotPosition.ToString())} not set");
                    return;
                }
                if (goalieSlot.slotPosition == SlotPositionEnum.MainGoalkeeper 
                    || goalieSlot.slotPosition == SlotPositionEnum.SubstituteGoalkeeper)
                    teamIds.goalies.Add(goalieSlot.slotPosition.ToString(), goalieSlot.draggableCard.CardData.tokenId);
                else if (goalieSlot.slotPosition == SlotPositionEnum.GoalieSubstitution1 
                    || goalieSlot.slotPosition == SlotPositionEnum.GoalieSubstitution2)
                    teamIds.goalie_substitutions.Add(goalieSlot.slotPosition.ToString(), goalieSlot.draggableCard.CardData.tokenId);
                
            }
            OnSaving();
            try
            {
                await Near.MarketplaceContract.ContractMethods.Actions.ManageTeam(teamIds);
            }
            catch (Exception e)
            {
                OnSavingError(e);
            }
            OnSaved();
        }

        private void OnNotEverythingSet(string message)
        {
            Debug.LogError(message);
        }

        private void OnSaving()
        {
            Debug.Log("Calculated"); 
        }

        private void OnSavingError(Exception e)
        {
            Debug.Log($"Got error while saving {e.Message}");
        }

        private void OnSaved()
        {
            Debug.Log("saved");
        }
        
        public void Back()
        {
            Game.LoadMainMenu();
        }
    }
}