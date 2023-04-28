using System.Collections.Generic;
using Runtime;
using UI.ManageTeam.DragAndDrop;
using UI.Scripts;
using UnityEngine;

namespace UI.ManageTeam
{
    public class GoaliesView: UiComponent
    {
        [SerializeField] private UISlot mainGoalieSlot;
        [SerializeField] private UISlot backupGoalieSlot;
        [SerializeField] private UISlot substitution1GoalieSlot;
        [SerializeField] private UISlot substitution2GoalieSlot;
        [SerializeField] private ManageTeamView manageTeamView;

        public List<UISlot> Goalies => new()
        {
            mainGoalieSlot,
            backupGoalieSlot,
            substitution1GoalieSlot,
            substitution2GoalieSlot
        };
        
        protected override void Initialize()
        {
            //mainGoalieSlot = Scripts.Utils.FindChild<UISlot>(transform, "MainGoalie");
            //backupGoalieSlot = Scripts.Utils.FindChild<UISlot>(transform, "BackupGoalie");
            //substitution1GoalieSlot = Scripts.Utils.FindChild<UISlot>(transform, "GoalieSubstitution1");
            //substitution2GoalieSlot = Scripts.Utils.FindChild<UISlot>(transform, "GoalieSubstitution2");
            //manageTeamView = Scripts.Utils.FindParent<ManageTeamView>(transform, "ManageTeam");
        }
        
        private void InitGoalie(UISlot slot)
        {
            string goalieToken = null;
            if (slot.slotPosition == SlotPositionEnum.MainGoalkeeper
                || slot.slotPosition == SlotPositionEnum.SubstituteGoalkeeper)
            {
                manageTeamView.Team.goalies.TryGetValue(slot.slotPosition.ToString(), out goalieToken);
            }
            else if (slot.slotPosition == SlotPositionEnum.GoalieSubstitution1
                     || slot.slotPosition == SlotPositionEnum.GoalieSubstitution2)
            {
                manageTeamView.Team.goalie_substitutions.TryGetValue(slot.slotPosition.ToString(), out goalieToken);
            }

            if (goalieToken == null)
            {
                return;
            }
            
            var card = manageTeamView.UserNfts.Find(nft => nft.tokenId == goalieToken);
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

        public void InitGoalies()
        {
            gameObject.SetActive(true);
            foreach (UISlot goalieSlot in Goalies)
            {
                InitGoalie(goalieSlot);
            }
            gameObject.SetActive(false);
        }
    }
}