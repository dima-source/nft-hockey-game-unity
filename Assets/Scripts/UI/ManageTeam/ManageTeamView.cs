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
        
        // private List<List<UISlot>> fives = new(8);
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

        private Team _team;
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
        }

        // public UISlot CreateNewBenchSlotWithPlayer(Transform container, UIPlayer uiPlayer)
        // {
        //         UISlot benchSlot = CreateNewEmptySlot(container, SlotPositionEnum.Bench);
        //         benchSlot.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
        //         
        //         uiPlayer.transform.SetParent(benchSlot.transform);
        //         uiPlayer.transform.localPosition = Vector3.zero;
        //         uiPlayer.RectTransform.sizeDelta = new Vector2(150, 225);
        //         uiPlayer.RectTransform.localScale = benchSlot.RectTransform.localScale;
        //
        //         benchSlot.uiPlayer = uiPlayer;
        //         uiPlayer.uiSlot = benchSlot;
        //         if (container == fieldPlayersBenchContent)
        //         {
        //             _fieldPlayersBench.Add(benchSlot);
        //         } 
        //         else if (container == goaliesBenchContent)
        //         {
        //             _goaliesBench.Add(benchSlot);
        //         }
        //         return benchSlot;
        // }

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
        
        // updates benches
        public void AddFieldPlayerToTeam(UIPlayer player)
        {
            goaliesBenchContent.Cards.Add(player.CardData);
            // TODO: add to special bench
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

        public void ChangeIceTimePriority()
        {
            iceTimePriority.text = Utils.Utils.GetIceTimePriority((int)iceTimePrioritySlider.value);
        }

        public void SaveTeam()
        {
            string iceTimePriorityValue = Utils.Utils.GetIceTimePriority((int)iceTimePrioritySlider.value);
            string tactics;
            try
            {
                tactics = Utils.Utils.GetTactics(tactictsDropdown.value);
            }
            catch (SwitchExpressionException)
            {
                Debug.LogError("Tactics not chosen");
                return;
            }
            Debug.Log(tactics);
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
                fiveIds.ice_time_priority = iceTimePriorityValue;
                fiveIds.tactic = tactics;
                fiveIds.number = lineNumber.ToString();
                teamIds.fives.Add(lineNumber.ToString(), fiveIds);
            }
            
            foreach (var goalieSlot in goalies)
            {
                if (!goalieSlot.uiPlayer && goalieSlot.slotPosition != SlotPositionEnum.GoalieSubstitution1 
                                         && goalieSlot.slotPosition != SlotPositionEnum.GoalieSubstitution2)
                {
                    Debug.LogError($"{goalieSlot.slotPosition.ToString()} not set");
                    return;
                }
                if (goalieSlot.slotPosition != SlotPositionEnum.GoalieSubstitution1 
                    && goalieSlot.slotPosition != SlotPositionEnum.GoalieSubstitution2)
                    teamIds.goalies.Add(goalieSlot.slotPosition.ToString(), goalieSlot.uiPlayer.CardData.tokenId);
            }
            teamIds.goalie_substitutions.Add(SlotPositionEnum.GoalieSubstitution1.ToString(), fieldPlayers[0]);
            teamIds.goalie_substitutions.Add(SlotPositionEnum.GoalieSubstitution2.ToString(), fieldPlayers[1]);
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