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
        
        private List<List<UISlot>> fives = new List<List<UISlot>>(4);
        private List<UISlot> goalies;
        private List<UISlot> _benchPlayers;

        private List<Token> _userNFTs;

        [SerializeField] private Transform canvasContent;
        [SerializeField] public Transform benchContent;

        // [SerializeField] private Text lineText;
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
            _userNFTs = await _controller.LoadUserNFTs(filter, pagination);
            
            _currentLineNumber = LineNumbers.First;

            ShowFive(_currentLineNumber.ToString());
            ShowBench(_currentLineNumber);
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

        // private void ShowGoalies()
        // {
        //     foreach (UISlot goalie in goalies)
        //     {
        //         if (goalie.uiPlayer != null)
        //         {
        //             Destroy(goalie.uiPlayer.gameObject);
        //         }
        //     }
        //     
        //     // lineText.text = "Goalies";
        //
        //     int goalieNumber = 0;
        //     foreach (var goalieNftMetadata in  _team.Goalies)
        //     {
        //         UISlot goalieSlot = goalies[goalieNumber];
        //         UIPlayer player = Instantiate(Game.AssetRoot.manageTeamAsset.goalie, goalieSlot.transform);
        //         
        //         //player.SetData(goalieNftMetadata.Value);
        //
        //         player.canvasContent = canvasContent;
        //         player.forwardsContent = forwardsCanvasContent;
        //         player.defendersContent = defendersCanvasContent;
        //
        //         goalies[goalieNumber].uiPlayer = player;
        //         player.uiSlot = goalies[goalieNumber];
        //
        //         goalieNumber++;
        //     }
        // }

        public UISlot CreateNewBenchSlotWithPlayer(UIPlayer uiPlayer)
        {
                UISlot benchSlot = CreateNewEmptySlot(benchContent, SlotPositionEnum.Bench);
                benchSlot.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                
                uiPlayer.RectTransform.sizeDelta = new Vector2(150, 225);

                benchSlot.uiPlayer = uiPlayer;
                uiPlayer.uiSlot = benchSlot;
                uiPlayer.transform.localPosition = Vector3.zero;
                
                benchSlot.manageTeamView = this;
                benchSlot.slotId = _benchPlayers.Count;
                
                _benchPlayers.Add(benchSlot);
                return benchSlot;
        }

        public UISlot CreateNewEmptySlot(Transform container, SlotPositionEnum position)
        {
            UISlot slot = Instantiate(Game.AssetRoot.manageTeamAsset.uiSlot, container);
            slot.slotPosition = position;
            slot.manageTeamView = this;
            return slot;
        }
        
        private void ShowBench(LineNumbers line)
        {
            if (_benchPlayers != null)
            {
                foreach (UISlot uiPlayerSlot in _benchPlayers)
                {
                    Destroy(uiPlayerSlot.gameObject);
                }
            }
            _benchPlayers = new List<UISlot>();
            
            string type = line switch
            {
                LineNumbers.Goalie => "FieldPlayer",
                _ => "GoaliePos"
            };

            List<Token> benchPlayers = type switch
            {
                "GoaliePos" => _userNFTs.Where(x => x.player_type != type && x.player_type != "Goalie")
                    .ToList(),
                _ => _userNFTs.Where(x => x.player_type != type).ToList()
            };

            int slotId = 0;
            
            foreach (Token nft in benchPlayers)
            {
                UISlot benchSlot = Instantiate(Game.AssetRoot.manageTeamAsset.uiSlot, benchContent);
                benchSlot.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                benchSlot.slotPosition = SlotPositionEnum.Bench;
                benchSlot.manageTeamView = this;
                
                UIPlayer uiPlayer = nft.player_type switch
                {
                    "FieldPlayer" => Instantiate(Game.AssetRoot.manageTeamAsset.fieldPlayer, benchSlot.transform),
                    "Goalie" => Instantiate(Game.AssetRoot.manageTeamAsset.goalie, benchSlot.transform),
                    _ => throw new Exception("Extra type not found")
                };

                uiPlayer.RectTransform.sizeDelta = new Vector2(150, 225);
                uiPlayer.CardData = nft;

                uiPlayer.uiSlot = benchSlot;
                
                uiPlayer.SetData(nft);
                uiPlayer.transform.localPosition = Vector3.zero;
                uiPlayer.canvasContent = canvasContent;
                uiPlayer.forwardsContent = forwardsCanvasContent;
                uiPlayer.defendersContent = defendersCanvasContent;

                benchSlot.manageTeamView = this;
                benchSlot.slotId = slotId;
                slotId++;
                
                _benchPlayers.Add(benchSlot);
            }
        }

        private void SetLineDataToTeam(string line)
        {
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
        }
        
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