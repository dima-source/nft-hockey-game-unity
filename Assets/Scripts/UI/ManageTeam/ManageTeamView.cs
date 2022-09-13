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
        private List<UISlot> _fieldPlayersBench = new();
        private List<UISlot> _goaliesBench = new();

        private List<Token> _userNFTs;
        
        [SerializeField] private Transform canvasContent;
        [SerializeField] public Transform fieldPlayersBenchContent;
        [SerializeField] public Transform goaliesBenchContent;

        [SerializeField] private TMP_Dropdown tactictsDropdown;
        [SerializeField] private Text iceTimePriority;
        [SerializeField] private Slider iceTimePrioritySlider;

        [SerializeField] public Transform teamView;
        [SerializeField] private Transform goaliesView;

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
            PlayerFilter filter = new PlayerFilter();
            Pagination pagination = new Pagination();
            pagination.first = 100;
            filter.ownerId = NearPersistentManager.Instance.GetAccountId();
            _userNFTs = await _controller.LoadUserNFTs(filter, pagination);
            
            _currentLineNumber = LineNumbers.First;

            ShowFive(_currentLineNumber.ToString());
            InitBenches();
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

        public UISlot CreateNewBenchSlotWithPlayer(Transform container, DraggableCard draggableCard)
        {
                UISlot benchSlot = CreateNewEmptySlot(container, SlotPositionEnum.Bench);
                benchSlot.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                
                draggableCard.transform.SetParent(benchSlot.transform);
                draggableCard.transform.localPosition = Vector3.zero;
                draggableCard.rectTransform.sizeDelta = new Vector2(150, 225);
                draggableCard.rectTransform.localScale = benchSlot.RectTransform.localScale;

                benchSlot.draggableCard = draggableCard;
                draggableCard.uiSlot = benchSlot;
                if (container == fieldPlayersBenchContent)
                {
                    _fieldPlayersBench.Add(benchSlot);
                } 
                else if (container == goaliesBenchContent)
                {
                    _goaliesBench.Add(benchSlot);
                }
                return benchSlot;
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

            foreach (Token nft in fieldPlayersBench)
            {

                DraggableCard draggableCard = Instantiate(Game.AssetRoot.manageTeamAsset.fieldCard);
                
                draggableCard.CardData = nft;
                draggableCard.SetData(nft);
                draggableCard.canvasContent = canvasContent;
                CreateNewBenchSlotWithPlayer(fieldPlayersBenchContent, draggableCard);
            }
            
            goaliesBenchContent.gameObject.SetActive(true);
            foreach (Token nft in goaliesBench)
            {
                DraggableCard draggableCard = Instantiate(Game.AssetRoot.manageTeamAsset.fieldCard);
                draggableCard.CardData = nft;
                draggableCard.SetData(nft);
                draggableCard.canvasContent = canvasContent;
                CreateNewBenchSlotWithPlayer(goaliesBenchContent, draggableCard);
            }
            goaliesBenchContent.gameObject.SetActive(false);
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

                    if (!playersOnPositions[position].draggableCard) // if ui player not set
                    {
                        Debug.LogError($"{lineNumber.ToString()} line not fully set");
                        return;
                    }
                    fiveIds.field_players.Add(position.ToString(),
                        playersOnPositions[position].draggableCard.CardData.tokenId);
                    fieldPlayers.Add(playersOnPositions[position].draggableCard.CardData.tokenId);
                }
                fiveIds.ice_time_priority = iceTimePriorityValue;
                fiveIds.tactic = tactics;
                fiveIds.number = lineNumber.ToString();
                teamIds.fives.Add(lineNumber.ToString(), fiveIds);
            }
            
            foreach (var goalieSlot in goalies)
            {
                if (!goalieSlot.draggableCard && goalieSlot.slotPosition != SlotPositionEnum.GoalieSubstitution1 
                                         && goalieSlot.slotPosition != SlotPositionEnum.GoalieSubstitution2)
                {
                    Debug.LogError($"{goalieSlot.slotPosition.ToString()} not set");
                    return;
                }
                if (goalieSlot.slotPosition != SlotPositionEnum.GoalieSubstitution1 
                    && goalieSlot.slotPosition != SlotPositionEnum.GoalieSubstitution2)
                    teamIds.goalies.Add(goalieSlot.slotPosition.ToString(), goalieSlot.draggableCard.CardData.tokenId);
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