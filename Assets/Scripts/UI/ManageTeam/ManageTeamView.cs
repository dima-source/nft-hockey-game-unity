using System;
using System.Collections.Generic;
using System.Linq;
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
            Goalie
        }
        private ManageTeamController _controller;

        public Transform forwardsCanvasContent;
        public Transform defendersCanvasContent;
        
        private List<List<UISlot>> fives = new(4);
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

        private void InitFives()
        {
            if (fives.Count == 4)
            {
                return;
            }

            fives.Clear();
            for (int i = 0; i < 4; i++)
            {
                List<UISlot> five = new List<UISlot>(5);
                UISlot slot = CreateNewEmptySlot(forwardsCanvasContent, SlotPositionEnum.LeftWinger);
                five.Add(slot);
                slot = CreateNewEmptySlot(forwardsCanvasContent, SlotPositionEnum.Center);
                five.Add(slot);
                slot = CreateNewEmptySlot(forwardsCanvasContent, SlotPositionEnum.RightWinger);
                five.Add(slot);
                slot = CreateNewEmptySlot(defendersCanvasContent, SlotPositionEnum.LeftDefender);
                five.Add(slot);
                slot = CreateNewEmptySlot(defendersCanvasContent, SlotPositionEnum.RightDefender);
                five.Add(slot);
                five.ForEach(slot => slot.gameObject.SetActive(false));
                fives.Add(five);
            }
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
            int fiveIndex = _currentLineNumber switch
            {
                LineNumbers.First=> 0,
                LineNumbers.Second => 1,
                LineNumbers.Third => 2,
                LineNumbers.Fourth => 3,
                _ => throw new ApplicationException($"Unexpected line number {_currentLineNumber}")
            };
            List<UISlot> five = fives[fiveIndex];
            foreach (UISlot uiSlot in five)
            {
                uiSlot.gameObject.SetActive(false);
            }
        }

        private LineNumbers StringToLineNumber(string line)
        {
            LineNumbers.TryParse(line, out LineNumbers parsedLine);
            return parsedLine;
        }
        
        public void ShowFive(string number)
        {
            LineNumbers parsedLine = StringToLineNumber(number);
            int fiveIndex = parsedLine switch
            {
                LineNumbers.First => 0,
                LineNumbers.Second => 1,
                LineNumbers.Third => 2,
                LineNumbers.Fourth => 3,
                _ => throw new ApplicationException($"Unexpected line number {_currentLineNumber}")
            };
            List<UISlot> five = fives[fiveIndex];
            foreach (UISlot uiSlot in five)
            {
                uiSlot.gameObject.SetActive(true);
            }

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
                
                // benchSlot.manageTeamView = this;

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
            // slot.manageTeamView = this;
            return slot;
        }
        
        private void InitBenches()
        {
            // if (_benchPlayers != null)
            // {
            //     foreach (UISlot uiPlayerSlot in _benchPlayers)
            //     {
            //         Destroy(uiPlayerSlot.gameObject);
            //     }
            // }
            // _benchPlayers = new List<UISlot>();

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

        // private void SetLineDataToTeam(string line)
        // {
            /*
            if (line == "Goalies")
            {
                if (goalies[0].uiPlayer == null || goalies[1] == null)
                {
                    return;
                }
                
                _team.Goalies["MainGoalkeeper"] = (Goalie)goalies[0].uiPlayer.CardData ;
                _team.Goalies["SubstituteGoalkeeper"] = (Goalie)goalies[1].uiPlayer.CardData;
            }
            else
            {
                _team.Fives[line].FieldPlayers["LeftWing"] = (FieldPlayer)fives[0].uiPlayer.CardData;
                _team.Fives[line].FieldPlayers["Center"] = (FieldPlayer)fives[1].uiPlayer.CardData;
                _team.Fives[line].FieldPlayers["RightWing"] = (FieldPlayer)fives[2].uiPlayer.CardData;
                _team.Fives[line].FieldPlayers["LeftDefender"] = (FieldPlayer)fives[3].uiPlayer.CardData;
                _team.Fives[line].FieldPlayers["RightDefender"] = (FieldPlayer)fives[4].uiPlayer.CardData;
            }
            */
        // }
        
        // public void SwitchLine(string line)
        // {
        //     SetLineDataToTeam(lineNumber);
        //     
        //     if (line == "Goalies")
        //     {
        //         if (lineNumber == "Goalies")
        //         {
        //             return;
        //         }
        //         
        //         fiveView.gameObject.SetActive(false);
        //         goaliesView.gameObject.SetActive(true);
        //         
        //         ShowGoalies();
        //         ShowBench(line);
        //     }
        //     else
        //     {
        //         fiveView.gameObject.SetActive(true);
        //         goaliesView.gameObject.SetActive(false);
        //         
        //         ShowFive(line);
        //         
        //         if (lineNumber == "Goalies")
        //         {
        //             ShowBench(line);
        //         }
        //     }
        //     
        //     lineNumber = line;
        // }
        
        // private UISlot GetUISlot(SlotPositionEnum slotPositionEnum, int slotId)
        // {
        //     UISlot uiSlot;
        //
        //     switch (slotPositionEnum)
        //     {
        //         case SlotPositionEnum.Bench:
        //             uiSlot = _benchPlayers.FirstOrDefault(x => x.slotId == slotId);
        //             break;
        //         case SlotPositionEnum.MainGoalie or SlotPositionEnum.BackupGoalie:
        //             uiSlot = goalies.FirstOrDefault(x => x.slotPosition == slotPositionEnum);
        //             break;
        //         default:
        //             uiSlot = fives.FirstOrDefault(x => x.slotPosition == slotPositionEnum);
        //             break;
        //     }
        //     
        //     return uiSlot;
        // }

        // public void SwapCards(UIPlayer uiPlayer1, UIPlayer uiPlayer2)
        // {
        //     UISlot uiSlot1 = GetUISlot(uiPlayer1.uiSlot.slotPosition, uiPlayer1.uiSlot.slotId);
        //     UISlot uiSlot2 = GetUISlot(uiPlayer2.uiSlot.slotPosition, uiPlayer2.uiSlot.slotId);
        //
        //     uiSlot1.uiPlayer.uiSlot = uiSlot2;
        //     uiSlot2.uiPlayer.uiSlot = uiSlot1;
        //     
        //     (uiSlot1.uiPlayer, uiSlot2.uiPlayer) = (uiSlot2.uiPlayer, uiSlot1.uiPlayer);
        // }
        
        public void ChangeIceTimePriority()
        {
            iceTimePriority.text = Utils.Utils.GetIceTimePriority((int)iceTimePrioritySlider.value);
        }
        
        // public void Apply()
        // {
        //     SetLineDataToTeam(_currentLineNumber);
        //     
        //     _controller.ChangeLineups(_team);
        // }
        
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