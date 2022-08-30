using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Near;
using Near.Models.Game.Team;
using Near.Models.Game.TeamIds;
using Near.Models.Tokens;
using Near.Models.Tokens.Filters;
using Near.Models.Tokens.Players;
using Near.Models.Tokens.Players.FieldPlayer;
using Near.Models.Tokens.Players.Goalie;
using Runtime;
using TMPro;
using UI.ManageTeam.DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ManageTeam
{
    
    public class ManageTeamView : MonoBehaviour
    {
        public enum LineNumbers
        {
            First,
            Second,
            Third,
            Fourth,
            PowerPlay1,
            PowerPlay2,
            PenaltyKill1,
            PenaltyKill2,
            Goalie
        }
        // TODO: make iceTimePriority and tactics different for every five 
        private ManageTeamController _controller;

        public Transform forwardsCanvasContent;
        public Transform defendersCanvasContent;
        
        private Dictionary<LineNumbers, Dictionary<SlotPositionEnum, UISlot>> fives = new();
        [SerializeField] private List<UISlot> goalies = new();

        private List<Token> _userNFTs;
        
        [SerializeField] public Transform canvasContent;
        [SerializeField] public Bench fieldPlayersBenchContent;
        [SerializeField] public Bench goaliesBenchContent;
        [SerializeField] public Bench powerPlayersBenchContent;
        [SerializeField] public Bench penaltyKillBenchContent;

        [SerializeField] private TMP_Dropdown tactictsDropdown;
        [SerializeField] private Text iceTimePriority;
        [SerializeField] private Slider iceTimePrioritySlider;

        private Dictionary<LineNumbers, string> _fivesTactics = new();
        private Dictionary<LineNumbers, string> _fivesIceTimePriority = new();

        [SerializeField] public Transform teamView;

        public Bench CurrentBench
        {
            get
            {
                var benches = new List<Bench> {fieldPlayersBenchContent, goaliesBenchContent, 
                    powerPlayersBenchContent, penaltyKillBenchContent};
                foreach (var bench in benches)
                {
                    if (bench.gameObject.activeSelf)
                    {
                        return bench;
                    }
                }
                
                throw new ApplicationException("No active bench");
            }
            set {}
        }

        private TeamIds _team;
        private LineNumbers _currentLineNumber;

        private void CreateFiveSlots(LineNumbers line)
        {
            var five = new Dictionary<SlotPositionEnum, UISlot>();
            UISlot slot;
            if (line != LineNumbers.PenaltyKill1 && line != LineNumbers.PenaltyKill2)
            {
                slot = CreateNewEmptySlot(forwardsCanvasContent, SlotPositionEnum.LeftWing);
                five.Add(SlotPositionEnum.LeftWing, slot);
            }
            slot = CreateNewEmptySlot(forwardsCanvasContent, SlotPositionEnum.Center);
            five.Add(SlotPositionEnum.Center, slot);
            slot = CreateNewEmptySlot(forwardsCanvasContent, SlotPositionEnum.RightWing);
            five.Add(SlotPositionEnum.RightWing, slot);
            slot = CreateNewEmptySlot(defendersCanvasContent, SlotPositionEnum.LeftDefender);
            five.Add(SlotPositionEnum.LeftDefender, slot);
            slot = CreateNewEmptySlot(defendersCanvasContent, SlotPositionEnum.RightDefender);
            five.Add(SlotPositionEnum.RightDefender, slot);
            five.Values.ToList().ForEach(slot => slot.gameObject.SetActive(false));
            fives.Add(line, five);
        }

        private void ClearFives()
        {
            fives.Values.ToList().
                ForEach(
                    dict => dict.Values.ToList().ForEach(slot => Destroy(slot.gameObject))
                );
            fives.Clear();
        }

        private void InitFives()
        {
            ClearFives();
            CreateFiveSlots(LineNumbers.First);
            CreateFiveSlots(LineNumbers.Second);
            CreateFiveSlots(LineNumbers.Third);
            CreateFiveSlots(LineNumbers.Fourth);
            CreateFiveSlots(LineNumbers.PowerPlay1);
            CreateFiveSlots(LineNumbers.PowerPlay2);
            CreateFiveSlots(LineNumbers.PenaltyKill1);
            CreateFiveSlots(LineNumbers.PenaltyKill2);
            
        }

        private void Awake()
        {
            _controller = new ManageTeamController();
            InitFives();
        }

        private async void Start()
        {
            _team = await _controller.LoadUserTeam();
            PlayerFilter filter = new()
            {
                ownerId = NearPersistentManager.Instance.GetAccountId()
            };
            Pagination pagination = new()
            {
                first = 100
            };
            _userNFTs = await _controller.LoadUserNFTs(filter, pagination);
            _currentLineNumber = LineNumbers.First;

            ShowFive(_currentLineNumber.ToString());
            InitBenches();
            fieldPlayersBenchContent.gameObject.SetActive(true);
        }

        public void HideCurrentFive()
        {
            Dictionary<SlotPositionEnum, UISlot> five = fives[_currentLineNumber];
            five.Values.ToList().ForEach(slot => slot.gameObject.SetActive(false));
            iceTimePrioritySlider.SetValueWithoutNotify(0f);
            iceTimePriority.text = "Select ice time priority";
            tactictsDropdown.SetValueWithoutNotify(0);
        }

        private LineNumbers StringToLineNumber(string line)
        {
            bool parsed = LineNumbers.TryParse(line, out LineNumbers parsedLine);
            if (!parsed)
            {
                throw new ApplicationException($"Cannot parse value {line} to LineNumbers");
            }
            return parsedLine;
        }
        
        public void ShowFive(string number)
        {
            LineNumbers parsedLine = StringToLineNumber(number);
            Dictionary<SlotPositionEnum, UISlot> five = fives[parsedLine];
            five.Values.ToList().ForEach(slot => slot.gameObject.SetActive(true));
            var forwardsHorizontalLayoutGroup = forwardsCanvasContent.GetComponent<HorizontalLayoutGroup>();
            if (parsedLine == LineNumbers.PenaltyKill1 || parsedLine == LineNumbers.PenaltyKill2)
            {
                forwardsHorizontalLayoutGroup.padding.left = 150;
                forwardsHorizontalLayoutGroup.padding.right = 150;
            }
            else
            {
                forwardsHorizontalLayoutGroup.padding.left = 0;
                forwardsHorizontalLayoutGroup.padding.right = 0;
            }

            _currentLineNumber = parsedLine;
            Debug.Log(number);

            bool alreadySet = _fivesTactics.TryGetValue(_currentLineNumber, out string tactic);
            if (alreadySet)
            {
                int value = tactic switch
                {
                    "Safe" => 1,
                    "Defensive" => 2,
                    "Neutral" => 3,
                    "Offensive" => 4,
                    "Aggressive" => 5
                };
                tactictsDropdown.SetValueWithoutNotify(value);
            }

            alreadySet = _fivesIceTimePriority.TryGetValue(_currentLineNumber, out string priority);
            if (alreadySet)
            {
                
                int value = priority switch
                {
                    "SuperLowPriority" => 1,
                    "LowPriority" => 2,
                    "Normal" => 3,
                    "HighPriority" => 4,
                    "SuperHighPriority" => 5
                };
                iceTimePrioritySlider.SetValueWithoutNotify(value );
                iceTimePriority.text = PascalToCapitalized(priority);
            }
        }

        public UISlot CreateNewEmptySlot(Transform container, SlotPositionEnum position)
        {
            UISlot slot = Instantiate(Game.AssetRoot.manageTeamAsset.uiSlot, container);
            slot.slotPosition = position;
            return slot;
        }
        
        private void InitBenches()
        {
            List<Token> fieldPlayersBench = _userNFTs.Where(x => x.player_type == "FieldPlayer").ToList();
            List<Token> goaliesBench = _userNFTs.Where(x => x.player_type == "Goalie").ToList();
            fieldPlayersBenchContent.Cards = fieldPlayersBench;
            goaliesBenchContent.Cards = goaliesBench;
        }

        private string PascalToCapitalized(string value)
        {
            var result = value.SelectMany((c, i) => i != 0 && char.IsUpper(c) && !char.IsUpper(value[i - 1]) ? new char[] { ' ', c } : new char[] { c });
            return new String(result.ToArray());
        }
        
        // updates benches
        public void AddFieldPlayerToTeam(UIPlayer player)
        {
            goaliesBenchContent.Cards.Add(player.CardData);
            powerPlayersBenchContent.Cards.Add(player.CardData);
            penaltyKillBenchContent.Cards.Add(player.CardData);
        }

        // updates benches
        public void RemoveFieldPlayerFromTeam(UIPlayer player)
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
            foreach (var goalieSlot in goalies)
            {
                if (!goalieSlot.uiPlayer)
                    continue;
                if (goalieSlot.uiPlayer.CardData.tokenId == player.CardData.tokenId)
                {
                    // goalieSlot.uiPlayer.gameObject.SetActive(true);
                    Destroy(goalieSlot.uiPlayer.gameObject);
                    goalieSlot.uiPlayer = null;
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
                foreach (var slot in fives[key].Values.ToList())
                {
                    if (!slot.uiPlayer)
                        continue;
                    if (slot.uiPlayer.CardData.tokenId == player.CardData.tokenId)
                    {
                        Destroy(slot.uiPlayer.gameObject);
                        slot.uiPlayer = null;
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
                foreach (var slot in fives[key].Values.ToList())
                {
                    if (!slot.uiPlayer)
                        continue;
                    if (slot.uiPlayer.CardData.tokenId == player.CardData.tokenId)
                    {
                        Destroy(slot.uiPlayer.gameObject);
                        slot.uiPlayer = null;
                        break;
                    }
                }
            }
        }

        public void OnChangeIceTimePriority()
        {
            string currentPriority = Utils.Utils.GetIceTimePriority((int) iceTimePrioritySlider.value);
            iceTimePriority.text = PascalToCapitalized(currentPriority);
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

        public void SaveTeam()
        {
            List<string> fieldPlayers = new();
            TeamIds teamIds = new();
            foreach (var lineNumber in fives.Keys)
            {
                FiveIds fiveIds = new();
                var playersOnPositions = fives[lineNumber];
                foreach (var position in playersOnPositions.Keys)
                {

                    if (!playersOnPositions[position].uiPlayer) // if ui player not set
                    {
                        Debug.LogError($"{lineNumber.ToString()} line not fully set");
                        return;
                    }
                    fiveIds.field_players.Add(position.ToString(),
                        playersOnPositions[position].uiPlayer.CardData.tokenId);
                    fieldPlayers.Add(playersOnPositions[position].uiPlayer.CardData.tokenId);
                }

                bool added;
                added = _fivesTactics.TryGetValue(lineNumber, out string tactics);
                if (!added)
                    throw new ApplicationException($"Tactics not set for line \"{lineNumber.ToString()}\"");
                fiveIds.tactic = tactics;
                fiveIds.number = lineNumber.ToString();
                
                added = _fivesIceTimePriority.TryGetValue(lineNumber, out string iceTimePriorityValue);
                if (!added)
                    throw new ApplicationException($"Ice time priority not set for line \"{lineNumber.ToString()}\"");
                fiveIds.ice_time_priority = iceTimePriorityValue;
                
                teamIds.fives.Add(lineNumber.ToString(), fiveIds);
            }
            
            foreach (var goalieSlot in goalies)
            {
                if (!goalieSlot.uiPlayer)
                {
                    Debug.LogError($"{goalieSlot.slotPosition.ToString()} not set");
                    return;
                }
                if (goalieSlot.slotPosition == SlotPositionEnum.MainGoalkeeper 
                    || goalieSlot.slotPosition == SlotPositionEnum.SubstituteGoalkeeper)
                    teamIds.goalies.Add(goalieSlot.slotPosition.ToString(), goalieSlot.uiPlayer.CardData.tokenId);
                else if (goalieSlot.slotPosition == SlotPositionEnum.GoalieSubstitution1 
                    || goalieSlot.slotPosition == SlotPositionEnum.GoalieSubstitution2)
                    teamIds.goalie_substitutions.Add(goalieSlot.slotPosition.ToString(), goalieSlot.uiPlayer.CardData.tokenId);
                
            }
            Debug.Log("Calculated");
            Near.MarketplaceContract.ContractMethods.Actions.ManageTeam(teamIds);
            Debug.Log("saved");
        }
        
        public void Cancel()
        {
            Start();
        }
        
        public void Back()
        {
            Game.LoadMainMenu();
        }
    }
}