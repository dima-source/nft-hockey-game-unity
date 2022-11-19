using System;
using System.Collections.Generic;
using System.Linq;
using Near.Models.Game.TeamIds;
using Runtime;
using UI.ManageTeam.DragAndDrop;
using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ManageTeam
{
    public class TeamFivesView: UiComponent
    {
        [SerializeField] private Transform forwardsContent;
        [SerializeField] private Transform defendersContent;
        [SerializeField] private ManageTeamView manageTeamView;
        [SerializeField] private TeamView teamView;
        
        protected override void Initialize()
        {
            forwardsContent = Scripts.Utils.FindChild<Transform>(transform, "Forwards");
            defendersContent = Scripts.Utils.FindChild<Transform>(transform, "Defenders");
            manageTeamView = Scripts.Utils.FindParent<ManageTeamView>(transform, "ManageTeam");
            teamView = Scripts.Utils.FindParent<TeamView>(transform, "Team");
        }
        
        public UISlot CreateNewEmptyFiveSlot(SlotPositionEnum position)
        {
            Transform container = position switch
            {
                SlotPositionEnum.LeftWing or SlotPositionEnum.Center or SlotPositionEnum.RightWing => forwardsContent,
                SlotPositionEnum.LeftDefender or SlotPositionEnum.RightDefender => defendersContent
            };
            UISlot slot = Instantiate(Game.AssetRoot.manageTeamAsset.uiSlot, container);
            slot.slotPosition = position;
            return slot;
        }
        
        private void InitTeamPlayer(LineNumbers line, SlotPositionEnum position)
        {
            var slot = teamView.Fives[line][position];
            
            FiveIds data;
            if (manageTeamView.Team.fives.Count == 0)
            {
                return;
            }

            manageTeamView.Team.fives.TryGetValue(line.ToString(), out data);

            string tokenId = data.field_players[position.ToString()];
            var card = manageTeamView.UserNfts.Find(nft => nft.tokenId == tokenId);
            DraggableCard player = Instantiate(Game.AssetRoot.manageTeamAsset.fieldCard, slot.transform);
            player.CardData = card;
            player.SetData(card);
            player.transform.SetParent(slot.transform);
            player.transform.localPosition = Vector3.zero;
            player.rectTransform.sizeDelta = slot.RectTransform.sizeDelta;
            player.rectTransform.localScale = slot.RectTransform.localScale;
                
            slot.draggableCard = player;
            slot.draggableCard.uiSlot = slot;
        }
        
        private void CreateFiveSlots(LineNumbers line)
        {
            var five = new Dictionary<SlotPositionEnum, UISlot>();
            teamView.Fives.Add(line, five);
            UISlot slot;
            if (line != LineNumbers.PenaltyKill1 && line != LineNumbers.PenaltyKill2)
            {
                slot = CreateNewEmptyFiveSlot(SlotPositionEnum.LeftWing);
                five.Add(SlotPositionEnum.LeftWing, slot);
                InitTeamPlayer(line, SlotPositionEnum.LeftWing);
            }
            
            slot = CreateNewEmptyFiveSlot(SlotPositionEnum.Center);
            five.Add(SlotPositionEnum.Center, slot);
            InitTeamPlayer(line, SlotPositionEnum.Center);
            
            slot = CreateNewEmptyFiveSlot(SlotPositionEnum.RightWing);
            five.Add(SlotPositionEnum.RightWing, slot);
            InitTeamPlayer(line, SlotPositionEnum.RightWing);
            
            slot = CreateNewEmptyFiveSlot(SlotPositionEnum.LeftDefender);
            five.Add(SlotPositionEnum.LeftDefender, slot);
            InitTeamPlayer(line, SlotPositionEnum.LeftDefender);
            
            slot = CreateNewEmptyFiveSlot(SlotPositionEnum.RightDefender);
            five.Add(SlotPositionEnum.RightDefender, slot);
            InitTeamPlayer(line, SlotPositionEnum.RightDefender);
            
            five.Values.ToList().ForEach(slot => slot.gameObject.SetActive(false));
        }
        
        public void InitFives()
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
        
        private void ClearFives()
        {
            teamView.Fives.Values.ToList().
                ForEach(
                    dict => dict.Values.ToList().ForEach(slot => Destroy(slot.gameObject))
                );
            teamView.Fives.Clear();
        }
        
        public void ShowFive(LineNumbers number)
        {
            if (number == LineNumbers.Goalie)
            {
                throw new ApplicationException("Cannot show goalie five in TeamFiveViews");
            }
            
            Dictionary<SlotPositionEnum, UISlot> five = teamView.Fives[number];
            five.Values.ToList().ForEach(slot => slot.gameObject.SetActive(true));
            var forwardsHorizontalLayoutGroup = forwardsContent.GetComponent<HorizontalLayoutGroup>();
            if (number == LineNumbers.PenaltyKill1 || number == LineNumbers.PenaltyKill2)
            {
                forwardsHorizontalLayoutGroup.padding.left = 150;
                forwardsHorizontalLayoutGroup.padding.right = 150;
            }
            else
            {
                forwardsHorizontalLayoutGroup.padding.left = 0;
                forwardsHorizontalLayoutGroup.padding.right = 0;
            }
            // TODO: update teamwork
        }
    }
}