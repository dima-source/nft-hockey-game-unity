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
        [SerializeField] public Transform benchesContent;

        [SerializeField] private TMP_Dropdown tactictsDropdown;
        [SerializeField] private Text iceTimePriority;
        [SerializeField] private Slider iceTimePrioritySlider;

        private Dictionary<LineNumbers, string> _fivesTactics = new();
        private Dictionary<LineNumbers, string> _fivesIceTimePriority = new();

        [SerializeField] public Transform teamView;
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

        private TeamIds _team;
        private LineNumbers _currentLineNumber;

        private void InitTeamPlayer(LineNumbers line, SlotPositionEnum position)
        {
            var slot = fives[line][position];
            
            FiveIds data;
            if (_team.fives.Count == 0)
            {
                return;
            }

            _team.fives.TryGetValue(line.ToString(), out data);

            string tokenId = data.field_players[position.ToString()];
            var card = _userNFTs.Find(nft => nft.tokenId == tokenId);
            DraggableCard player = Instantiate(Game.AssetRoot.manageTeamAsset.fieldCard, slot.transform);
            player.CardData = card;
            player.SetData(card);
            player.canvasContent = canvasContent;
            player.transform.SetParent(slot.transform);
            player.transform.localPosition = Vector3.zero;
            player.rectTransform.sizeDelta = slot.RectTransform.sizeDelta;
            player.rectTransform.localScale = slot.RectTransform.localScale;
            player.ManageTeamView = this;
                
            slot.draggableCard = player;
            slot.draggableCard.uiSlot = slot;
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
            DraggableCard player = Instantiate(Game.AssetRoot.manageTeamAsset.fieldCard, slot.transform);
            player.CardData = card;
            player.SetData(card);
            player.canvasContent = canvasContent;
            player.transform.SetParent(slot.transform);
            player.transform.localPosition = Vector3.zero;
            player.rectTransform.sizeDelta = slot.RectTransform.sizeDelta;
            player.rectTransform.localScale = slot.RectTransform.localScale;
            player.ManageTeamView = this;
                
            slot.draggableCard = player;
            slot.draggableCard.uiSlot = slot; 
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

        private void InitTopPanels()
        {
            if (_team.fives.Count == 0)
            {
                return;
            }
            foreach (var five in _team.fives.Values)
            {
                _fivesTactics.Add(StringToLineNumber(five.number), five.tactic);
                _fivesIceTimePriority.Add(StringToLineNumber(five.number), five.ice_time_priority);
            }
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
            InitTopPanels();

            _currentLineNumber = LineNumbers.First;
            ShowFive(_currentLineNumber.ToString());
            InitBenches();
            fieldPlayersBenchContent.gameObject.SetActive(true);
            
            UpdateTeamWork();
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
            if (number == "G")
            {
                _teamworkText.text = "";
                return;
            }
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

            var alreadySet = _fivesTactics.TryGetValue(_currentLineNumber, out string tactic);
            if (alreadySet)
            {
                var value = tactic switch
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
                iceTimePrioritySlider.SetValueWithoutNotify(value);
                iceTimePriority.text = Utils.Utils.PascalToCapitalized(priority);
            }
            else
            {
                _fivesIceTimePriority.Add(_currentLineNumber, Utils.Utils.GetIceTimePriority(3));
                iceTimePrioritySlider.SetValueWithoutNotify(3);
                iceTimePriority.text = Utils.Utils.PascalToCapitalized(Utils.Utils.GetIceTimePriority(3)); 
            }
            UpdateTeamWork();
        }

        public UISlot CreateNewEmptySlot(Transform container, SlotPositionEnum position)
        {
            UISlot slot = Instantiate(Game.AssetRoot.manageTeamAsset.uiSlot, container);
            slot.slotPosition = position;
            slot.manageTeamView = this;
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
            foreach (var goalieSlot in goalies)
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
                foreach (var slot in fives[key].Values.ToList())
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
                foreach (var slot in fives[key].Values.ToList())
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
            var playersSlots = fives[_currentLineNumber].Values;
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
            if (goalies.Contains(slot))
                return;
            
            var currentFive = fives[_currentLineNumber].Values.ToList().Where(x => x != slot);

            // if player moved to bench
            if (slot.slotPosition == SlotPositionEnum.Bench)
            {
                List<DraggableCard> sameNationalityPlayers = new();
                foreach (var uiSlot in fives[_currentLineNumber].Values.ToList().Where(x => x != slot))
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

                if (currentFive.Where(x => x.draggableCard)
                        .Count(x => ((Player) x.draggableCard.CardData).player_role is "ToughGuy" or "Enforcer") > 0)
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

                int toughGuysAndEnforcers = currentFive.Where(x => x.draggableCard).Count(x =>
                    ((Player) x.draggableCard.CardData).player_role is "TryHarder" or "TwoWay");
                for (int i = 0; i < toughGuysAndEnforcers; i++)
                {
                        percent += 10;
                }

                if ((player.uiSlot.slotPosition == SlotPositionEnum.LeftDefender ||
                    player.uiSlot.slotPosition == SlotPositionEnum.RightDefender) && 
                    currentFive.Count(x => ((Player) x.draggableCard.CardData).player_role is "DefensiveForward") > 0)
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

        public async void SaveTeam()
        {
            List<string> fieldPlayers = new();
            TeamIds teamIds = new();
            foreach (var lineNumber in fives.Keys)
            {
                FiveIds fiveIds = new();
                var playersOnPositions = fives[lineNumber];
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
            
            foreach (var goalieSlot in goalies)
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