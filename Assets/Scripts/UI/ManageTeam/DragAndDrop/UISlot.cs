using System.Collections.Generic;
using UI.Scripts.Card;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.ManageTeam.DragAndDrop
{
    public class UISlot : MonoBehaviour, IDropHandler
    {
        public ManageTeamView manageTeamView;
        public RectTransform RectTransform;

        public SlotPositionEnum slotPosition;
        
        public DraggableCard draggableCard;

        protected void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            manageTeamView = GetComponentInParent<ManageTeamView>();
        }

        private void EndDrop(DraggableCard draggableCardDropped)
        {
            Transform uiPlayerDroppedTransform = draggableCardDropped.transform;
            draggableCardDropped.transform.SetParent(transform); 
            uiPlayerDroppedTransform.localPosition = Vector3.zero;
        }

        private void FinalizeDrop(DraggableCard draggableCardDropped)
        {
            // destroying slot if it's card moved from bench to five
            if (manageTeamView.fieldPlayersBenchContent.Slots.Contains(draggableCardDropped.uiSlot))
            {
                manageTeamView.fieldPlayersBenchContent.RemoveSlotWithinPlayer(draggableCardDropped);
                manageTeamView.AddFieldPlayerToTeam(draggableCardDropped);
            }
            else
            {
                // destroying bench slot if it's card moved from any bench excepting FieldPlayersBench to team
                List<Bench> benches = new List<Bench>
                {manageTeamView.goaliesBenchContent, manageTeamView.powerPlayersBenchContent, 
                    manageTeamView.penaltyKillBenchContent};
                foreach (var bench in benches)
                {
                    if (bench.Slots.Contains(draggableCardDropped.uiSlot))
                    {
                        bench.RemoveSlotWithinPlayer(draggableCardDropped);
                    }
                }
            }

            
            draggableCard = draggableCardDropped;

            draggableCardDropped.uiSlot = this;
            
            draggableCardDropped.rectTransform.sizeDelta = RectTransform.sizeDelta;
            draggableCardDropped.rectTransform.localScale = RectTransform.localScale;
            EndDrop(draggableCardDropped);
        }

        private DraggableCard ProcessSwap(DraggableCard draggableCardDropped)
        {
            DraggableCard previousUIPlayer = draggableCard;
            UISlot benchSlot = draggableCardDropped.uiSlot;
            
            benchSlot.draggableCard = previousUIPlayer;
            Transform previousUIPlayerTransform = previousUIPlayer.transform;
            previousUIPlayerTransform.SetParent(benchSlot.transform);
            previousUIPlayerTransform.localPosition = Vector3.zero;
            previousUIPlayer.rectTransform.sizeDelta = benchSlot.RectTransform.sizeDelta;
            previousUIPlayer.uiSlot = benchSlot;
            
            draggableCardDropped.rectTransform.sizeDelta = RectTransform.sizeDelta;
            draggableCardDropped.rectTransform.localScale = RectTransform.localScale;
            EndDrop(draggableCardDropped);
            draggableCard = draggableCardDropped;
            draggableCard.uiSlot = this;
            return previousUIPlayer;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var secondaryBenches = new List<Bench>
            {
                manageTeamView.goaliesBenchContent, manageTeamView.penaltyKillBenchContent,
                manageTeamView.powerPlayersBenchContent
            };
            DraggableCard draggableCardDropped = eventData.pointerDrag.GetComponent<DraggableCard>();
            
            // moving card back to bench slot if it is moved to another bench slot
            if (transform.parent == draggableCardDropped.uiSlot.transform.parent && 
                transform.parent == manageTeamView.CurrentBench.transform)
            {
                EndDrop(draggableCardDropped);
                draggableCardDropped.transform.SetParent(draggableCardDropped.uiSlot.transform); 
                return;
            }
            
            // moving card back if FieldPlayer is trying to be moved in goalie slot
            if (slotPosition is SlotPositionEnum.MainGoalkeeper or SlotPositionEnum.SubstituteGoalkeeper
                && draggableCardDropped.playerCardData.position.ToString() != "G")
            {
                EndDrop(draggableCardDropped);
                draggableCardDropped.transform.SetParent(draggableCardDropped.uiSlot.transform); 
                return;
            }
            
            // moving card back if Goalie is trying to be moved in FieldPlayer slot
            if (slotPosition != SlotPositionEnum.MainGoalkeeper &&
                slotPosition != SlotPositionEnum.SubstituteGoalkeeper &&
                slotPosition != SlotPositionEnum.Bench && 
                draggableCardDropped.playerCardData.position.ToString() == "G")
            {
                EndDrop(draggableCardDropped);
                draggableCardDropped.transform.SetParent(draggableCardDropped.uiSlot.transform); 
                return;
            }
            
            // moving card from five to bench
            // if (draggableCardDropped.uiSlot.transform.parent.parent == manageTeamView.teamView &&
            if (Scripts.Utils.HasChild(manageTeamView.teamView, draggableCardDropped.uiSlot.transform.parent.name) &&
                transform.parent == manageTeamView.CurrentBench.transform)
            {
                draggableCardDropped.uiSlot.draggableCard = null;
                manageTeamView.CurrentBench.AddPlayer(draggableCardDropped);
                if (manageTeamView.CurrentBench.transform == manageTeamView.fieldPlayersBenchContent.transform)
                {
                    manageTeamView.RemoveFieldPlayerFromTeam(draggableCardDropped);
                    manageTeamView.ShowStatsChanges(draggableCardDropped);
                }
                else if (manageTeamView.CurrentBench.transform == manageTeamView.powerPlayersBenchContent.transform ||
                         manageTeamView.CurrentBench.transform == manageTeamView.penaltyKillBenchContent.transform)
                {
                    manageTeamView.ShowStatsChanges(draggableCardDropped);
                } 
                return;
            }

            // // moving card from goalies to bench
            // if (draggableCardDropped.uiSlot.transform.parent.parent.parent == manageTeamView.teamView &&
            //     transform.parent == manageTeamView.goaliesBenchContent.transform)
            // {
            //     draggableCardDropped.uiSlot.draggableCard = null;
            //     manageTeamView.goaliesBenchContent.AddPlayer(draggableCardDropped);
            //     return;
            // }
            
            // moving card from PP to bench
            // if (draggableCardDropped.uiSlot.transform.parent.parent == manageTeamView.teamView &&
            //     transform.parent == manageTeamView.powerPlayersBenchContent.transform)
            // {
            //     draggableCardDropped.uiSlot.draggableCard = null;
            //     manageTeamView.powerPlayersBenchContent.AddPlayer(draggableCardDropped);
            //     manageTeamView.ShowStatsChanges(draggableCardDropped);
            //     return;
            // }
            
            // moving card from PK to bench
            // if (draggableCardDropped.uiSlot.transform.parent.parent == manageTeamView.teamView &&
            //     transform.parent == manageTeamView.penaltyKillBenchContent.transform)
            // {
            //     draggableCardDropped.uiSlot.draggableCard = null;
            //     manageTeamView.penaltyKillBenchContent.AddPlayer(draggableCardDropped);
            //     manageTeamView.ShowStatsChanges(draggableCardDropped);
            //     return;
            // }
            
            // swap cards inside five or goalies
            if ((draggableCardDropped.uiSlot.transform.parent.parent == transform.parent.parent || 
                 draggableCardDropped.uiSlot.transform.parent.parent.parent == transform.parent.parent.parent) &&
                draggableCard)
            {
                var previousPlayer = ProcessSwap(draggableCardDropped);
                if (!(previousPlayer == draggableCardDropped))
                {
                    manageTeamView.ShowStatsChanges(draggableCardDropped, true);
                    manageTeamView.ShowStatsChanges(previousPlayer, true);
                }
                return;
            }
            
            // swap from bench to any line excepting main four lines
            foreach (var bench in secondaryBenches)
            {
                if (draggableCard && draggableCardDropped.uiSlot.transform.parent == bench.transform)
                {
                    bench.ReplacePlayer(draggableCardDropped, draggableCard);
                    ProcessSwap(draggableCardDropped);
                    manageTeamView.ShowStatsChanges(draggableCardDropped);
                    return;
                }
            }

            // swap from bench to five
            if (draggableCard && draggableCardDropped.uiSlot.transform.parent ==
                manageTeamView.fieldPlayersBenchContent.transform)
            {
                manageTeamView.RemoveFieldPlayerFromTeam(draggableCard);
                manageTeamView.fieldPlayersBenchContent.ReplacePlayer(draggableCardDropped, draggableCard);
                manageTeamView.AddFieldPlayerToTeam(draggableCardDropped);
                ProcessSwap(draggableCardDropped);
                manageTeamView.ShowStatsChanges(draggableCardDropped);
                return;
            }
            
            FinalizeDrop(draggableCardDropped);
            manageTeamView.ShowStatsChanges(draggableCardDropped);
        }
    }
}