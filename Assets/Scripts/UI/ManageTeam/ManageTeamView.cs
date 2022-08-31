using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Xsl;
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
        [SerializeField] public Transform goaliesContent;

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

        private void InitTeamPlayer(LineNumbers line, SlotPositionEnum position)
        {
            var slot = fives[line][position];
            
            FiveIds data;
            if (_team.fives.Count == 0)
            {
                data = new();
            }
            else
            {
                _team.fives.TryGetValue(line.ToString(), out data);
            }

            if (data == null)
            {
                return;
            }

            string tokenId = data.field_players[position.ToString()];
            var card = _userNFTs.Find(nft => nft.tokenId == tokenId);
            UIPlayer player = Instantiate(Game.AssetRoot.manageTeamAsset.fieldPlayer, slot.transform);
            player.CardData = card;
            player.SetData(card);
            player.canvasContent = canvasContent;
            player.transform.SetParent(slot.transform);
            player.transform.localPosition = Vector3.zero;
            player.RectTransform.sizeDelta = slot.RectTransform.sizeDelta;
            player.RectTransform.localScale = slot.RectTransform.localScale;
                
            slot.uiPlayer = player;
            slot.uiPlayer.uiSlot = slot;
        }

        private void InitGoalie(UISlot slot)
        {
            string goalieToken = null;
            if (slot.slotPosition == SlotPositionEnum.MainGoalkeeper
                || slot.slotPosition == SlotPositionEnum.SubstituteGoalkeeper)
            {
                _team.goalies.TryGetValue(slot.slotPosition.ToString(), out goalieToken);
            }
            else if (slot.slotPosition == SlotPositionEnum.GoalieSubstitution1
                     || slot.slotPosition == SlotPositionEnum.GoalieSubstitution2)
            {
                _team.goalie_substitutions.TryGetValue(slot.slotPosition.ToString(), out goalieToken);
            }

            if (goalieToken == null)
            {
                return;
            }
            
            var card = _userNFTs.Find(nft => nft.tokenId == goalieToken);
            UIPlayer player = Instantiate(Game.AssetRoot.manageTeamAsset.fieldPlayer, slot.transform);
            player.CardData = card;
            player.SetData(card);
            player.canvasContent = canvasContent;
            player.transform.SetParent(slot.transform);
            player.transform.localPosition = Vector3.zero;
            player.RectTransform.sizeDelta = slot.RectTransform.sizeDelta;
            player.RectTransform.localScale = slot.RectTransform.localScale;
                
            slot.uiPlayer = player;
            slot.uiPlayer.uiSlot = slot; 
        }

        private void CreateFiveSlots(LineNumbers line)
        {
            var five = new Dictionary<SlotPositionEnum, UISlot>();
            fives.Add(line, five);
            UISlot slot;
            if (line != LineNumbers.PenaltyKill1 && line != LineNumbers.PenaltyKill2)
            {
                slot = CreateNewEmptySlot(forwardsCanvasContent, SlotPositionEnum.LeftWing);
                five.Add(SlotPositionEnum.LeftWing, slot);
                InitTeamPlayer(line, SlotPositionEnum.LeftWing);
            }
            
            slot = CreateNewEmptySlot(forwardsCanvasContent, SlotPositionEnum.Center);
            five.Add(SlotPositionEnum.Center, slot);
            InitTeamPlayer(line, SlotPositionEnum.Center);
            
            slot = CreateNewEmptySlot(forwardsCanvasContent, SlotPositionEnum.RightWing);
            five.Add(SlotPositionEnum.RightWing, slot);
            InitTeamPlayer(line, SlotPositionEnum.RightWing);
            
            slot = CreateNewEmptySlot(defendersCanvasContent, SlotPositionEnum.LeftDefender);
            five.Add(SlotPositionEnum.LeftDefender, slot);
            InitTeamPlayer(line, SlotPositionEnum.LeftDefender);
            
            slot = CreateNewEmptySlot(defendersCanvasContent, SlotPositionEnum.RightDefender);
            five.Add(SlotPositionEnum.RightDefender, slot);
            InitTeamPlayer(line, SlotPositionEnum.RightDefender);
            
            five.Values.ToList().ForEach(slot => slot.gameObject.SetActive(false));
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

        private void InitGoalies()
        {
            goaliesContent.gameObject.SetActive(true);
            foreach (UISlot goalieSlot in goalies)
            {
                InitGoalie(goalieSlot);
            }
            goaliesContent.gameObject.SetActive(false);
        }

        private void Awake()
        {
            _controller = new ManageTeamController();
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
            _userNFTs = await _controller.LoadUserNFTs(filter, pagination);
            _team = await _controller.LoadUserTeam();
            InitFives();
            InitGoalies();

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
            if (_team.fives.Count == 0)
            {
                List<Token> fieldPlayers = _userNFTs.Where(x => x.player_type == "FieldPlayer").ToList();
                List<Token> goalies = _userNFTs.Where(x => x.player_type == "Goalie").ToList();
                fieldPlayersBenchContent.Cards = fieldPlayers;
                goaliesBenchContent.Cards = goalies;
                return;
            }
            
            HashSet<string> fieldPlayersTokensInTeam = new();
            foreach (var five in _team.fives.Values)
            {
                foreach (var tokenId in five.field_players.Values.ToList())
                {
                    fieldPlayersTokensInTeam.Add(tokenId);
                }
            }
            
            List<string> goaliesTokensInTeam = new();
            foreach (var tokenId in _team.goalies.Values)
            {
                goaliesTokensInTeam.Add(tokenId);
            }
            foreach (var tokenId in _team.goalie_substitutions.Values)
            {
                goaliesTokensInTeam.Add(tokenId);
            }

            List<string> powerPlayersTokensInTeam = new();
            foreach (var tokenId in _team.fives[LineNumbers.PowerPlay1.ToString()].field_players.Values.ToList())
            {
                powerPlayersTokensInTeam.Add(tokenId);
            }
            foreach (var tokenId in _team.fives[LineNumbers.PowerPlay2.ToString()].field_players.Values.ToList())
            {
                powerPlayersTokensInTeam.Add(tokenId);
            }
            
            List<string> penaltyKillTokensInTeam = new();
            foreach (var tokenId in _team.fives[LineNumbers.PenaltyKill1.ToString()].field_players.Values.ToList())
            {
                penaltyKillTokensInTeam.Add(tokenId);
            }
            foreach (var tokenId in _team.fives[LineNumbers.PenaltyKill2.ToString()].field_players.Values.ToList())
            {
                penaltyKillTokensInTeam.Add(tokenId);
            }
            
            
            List<Token> fieldPlayersBench = _userNFTs.Where(x => x.player_type == "FieldPlayer" &&
                                                                 !fieldPlayersTokensInTeam.Contains(x.tokenId) ).ToList();
            List<Token> goaliesBench = _userNFTs.Where(x => x.player_type == "Goalie" && !goaliesTokensInTeam.Contains(x.tokenId)).ToList();
            goaliesBench.AddRange(_userNFTs.Where(x => fieldPlayersTokensInTeam.Contains(x.tokenId) && 
                                                       !goaliesTokensInTeam.Contains(x.tokenId)));
            
            List<Token> powerPlayersBench = _userNFTs.Where(x => fieldPlayersTokensInTeam.Contains(x.tokenId))
                .Where(x => !powerPlayersTokensInTeam.Contains(x.tokenId)).ToList();
            List<Token> penaltyKillBench = _userNFTs.Where(x => fieldPlayersTokensInTeam.Contains(x.tokenId))
                .Where(x => !penaltyKillTokensInTeam.Contains(x.tokenId)).ToList();
            // List<Token> penaltyKillBench = fieldPlayersBench.Where(x => !penaltyKillTokensInTeam.Contains(x.tokenId)).ToList();
            
            fieldPlayersBenchContent.Cards = fieldPlayersBench;
            goaliesBenchContent.Cards = goaliesBench;
            powerPlayersBenchContent.Cards = powerPlayersBench;
            penaltyKillBenchContent.Cards = penaltyKillBench;
            
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