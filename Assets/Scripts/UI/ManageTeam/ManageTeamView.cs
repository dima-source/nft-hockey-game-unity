using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Near.Models.Game.Team;
using Near.Models.Tokens;
using Near.Models.Tokens.Filters;
using Near.Models.Tokens.Players;
using Near.Models.Tokens.Players.FieldPlayer;
using Near.Models.Tokens.Players.Goalie;
using Runtime;
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
        
        // private List<List<UISlot>> fives = new(8);
        private Dictionary<LineNumbers, Dictionary<SlotPositionEnum, UISlot>> fives = new();
        private List<UISlot> goalies = new();
        private List<UISlot> _fieldPlayersBench = new();
        private List<UISlot> _goaliesBench = new();

        private List<Token> _userNFTs;

        [SerializeField] private Transform canvasContent;
        [SerializeField] public Transform fieldPlayersBenchContent;
        [SerializeField] public Transform goaliesBenchContent;

        [SerializeField] private Text iceTimePriority;
        [SerializeField] private Slider iceTimePrioritySlider;

        [SerializeField] public Transform teamView;
        [SerializeField] private Transform goaliesView;

        private Team _team;
        private LineNumbers _currentLineNumber;

        private void CreateFiveSlots(LineNumbers line)
        {
            var five = new Dictionary<SlotPositionEnum, UISlot>();
            UISlot slot = CreateNewEmptySlot(forwardsCanvasContent, SlotPositionEnum.LeftWinger);
            five.Add(SlotPositionEnum.LeftWinger, slot);
            slot = CreateNewEmptySlot(forwardsCanvasContent, SlotPositionEnum.Center);
            five.Add(SlotPositionEnum.Center, slot);
            slot = CreateNewEmptySlot(forwardsCanvasContent, SlotPositionEnum.RightWinger);
            five.Add(SlotPositionEnum.RightWinger, slot);
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

            _currentLineNumber = parsedLine;
            Debug.Log(number);
        }

        public UISlot CreateNewBenchSlotWithPlayer(Transform container, UIPlayer uiPlayer)
        {
                UISlot benchSlot = CreateNewEmptySlot(container, SlotPositionEnum.Bench);
                benchSlot.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                
                uiPlayer.RectTransform.sizeDelta = new Vector2(150, 225);

                benchSlot.uiPlayer = uiPlayer;
                uiPlayer.uiSlot = benchSlot;
                uiPlayer.transform.SetParent(benchSlot.transform);
                uiPlayer.transform.localPosition = Vector3.zero;
                
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

                UIPlayer uiPlayer = Instantiate(Game.AssetRoot.manageTeamAsset.fieldPlayer);
                
                uiPlayer.CardData = nft;
                uiPlayer.SetData(nft);
                uiPlayer.canvasContent = canvasContent;
                CreateNewBenchSlotWithPlayer(fieldPlayersBenchContent, uiPlayer);
            }
            
            foreach (Token nft in goaliesBench)
            {

                UIPlayer uiPlayer = Instantiate(Game.AssetRoot.manageTeamAsset.fieldPlayer);
                
                uiPlayer.CardData = nft;
                uiPlayer.SetData(nft);
                uiPlayer.canvasContent = canvasContent;
                CreateNewBenchSlotWithPlayer(goaliesBenchContent, uiPlayer);
            }
        }

        public void ChangeIceTimePriority()
        {
            iceTimePriority.text = Utils.Utils.GetIceTimePriority((int)iceTimePrioritySlider.value);
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