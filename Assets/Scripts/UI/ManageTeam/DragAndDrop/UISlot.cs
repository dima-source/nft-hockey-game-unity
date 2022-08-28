using System;
using Runtime;
using UI.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.ManageTeam.DragAndDrop
{
    public class UISlot : MonoBehaviour, IDropHandler
    {
        public ManageTeamView manageTeamView;
        public RectTransform RectTransform;

        public SlotPositionEnum slotPosition;
        
        public UIPlayer uiPlayer;

        protected void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            manageTeamView = GetComponentInParent<ManageTeamView>();
        }

        private void EndDrop(UIPlayer uiPlayerDropped)
        {
            Transform uiPlayerDroppedTransform = uiPlayerDropped.transform;
            uiPlayerDropped.transform.SetParent(transform); 
            uiPlayerDroppedTransform.localPosition = Vector3.zero;
        }

        private void FinalizeDrop(UIPlayer uiPlayerDropped)
        {
            // destroying slot if it's card moved from bench to five or goalies
            // if (uiPlayerDropped.uiSlot.transform.parent == manageTeamView.fieldPlayersBenchContent.transform ||
            //     uiPlayerDropped.uiSlot.transform.parent == manageTeamView.goaliesBenchContent.transform )
            // {
            //     Destroy(uiPlayerDropped.uiSlot.gameObject);
            // }

            // destroying slot if it's card moved from bench to five
            if (manageTeamView.fieldPlayersBenchContent.Slots.Contains(uiPlayerDropped.uiSlot))
            {
                manageTeamView.fieldPlayersBenchContent.RemoveSlotWithinPlayer(uiPlayerDropped);
                manageTeamView.AddFieldPlayerToTeam(uiPlayerDropped);
            }
            
            // destroying slot if it's card moved from bench to goalies
            if (manageTeamView.goaliesBenchContent.Slots.Contains(uiPlayerDropped.uiSlot))
            {
                manageTeamView.goaliesBenchContent.RemoveSlotWithinPlayer(uiPlayerDropped);
            }
            
            uiPlayer = uiPlayerDropped;

            uiPlayer.uiSlot = this;
            
            uiPlayerDropped.RectTransform.sizeDelta = RectTransform.sizeDelta;
            uiPlayerDropped.RectTransform.localScale = RectTransform.localScale;
            EndDrop(uiPlayerDropped);
        }

        private void ProcessSwap(UIPlayer uiPlayerDropped)
        {
            // TODO: change team data
            UIPlayer previousUIPlayer = uiPlayer;
            UISlot benchSlot = uiPlayerDropped.uiSlot;
            
            benchSlot.uiPlayer = previousUIPlayer;
            Transform previousUIPlayerTransform = previousUIPlayer.transform;
            previousUIPlayerTransform.SetParent(benchSlot.transform);
            previousUIPlayerTransform.localPosition = Vector3.zero;
            previousUIPlayer.RectTransform.sizeDelta = benchSlot.RectTransform.sizeDelta;
            previousUIPlayer.uiSlot = benchSlot;
            
            uiPlayerDropped.RectTransform.sizeDelta = RectTransform.sizeDelta;
            uiPlayerDropped.RectTransform.localScale = RectTransform.localScale;
            EndDrop(uiPlayerDropped);
            uiPlayer = uiPlayerDropped;
            uiPlayer.uiSlot = this;
        }

        public void OnDrop(PointerEventData eventData)
        {
            UIPlayer uiPlayerDropped = eventData.pointerDrag.GetComponent<UIPlayer>();
            
            // moving card back to bench slot if it is moved to another bench slot
            if (transform.parent == manageTeamView.fieldPlayersBenchContent.transform && 
                uiPlayerDropped.uiSlot.transform.parent == manageTeamView.fieldPlayersBenchContent.transform ||
                transform.parent == manageTeamView.goaliesBenchContent.transform && 
                uiPlayerDropped.uiSlot.transform.parent == manageTeamView.goaliesBenchContent.transform
                )
            {
                EndDrop(uiPlayerDropped);
                uiPlayerDropped.transform.SetParent(uiPlayerDropped.uiSlot.transform); 
                return;
            }
            
            // moving card back if FieldPlayer is trying to be moved in goalie slot
            if (slotPosition is SlotPositionEnum.MainGoalkeeper or SlotPositionEnum.SubstituteGoalkeeper
                && uiPlayerDropped.position != CardView.Position.G)
            {
                EndDrop(uiPlayerDropped);
                uiPlayerDropped.transform.SetParent(uiPlayerDropped.uiSlot.transform); 
                return;
            }
            
            // moving card back if Goalie is trying to be moved in FieldPlayer slot
            if ((slotPosition != SlotPositionEnum.MainGoalkeeper && slotPosition != SlotPositionEnum.SubstituteGoalkeeper)
                && uiPlayerDropped.position == CardView.Position.G)
            {
                EndDrop(uiPlayerDropped);
                uiPlayerDropped.transform.SetParent(uiPlayerDropped.uiSlot.transform); 
                return;
            }
            
            // moving card from five to bench
            if (uiPlayerDropped.uiSlot.transform.parent.parent == manageTeamView.teamView &&
                transform.parent == manageTeamView.fieldPlayersBenchContent.transform)
            {
                uiPlayerDropped.uiSlot.uiPlayer = null;
                manageTeamView.fieldPlayersBenchContent.AddPlayer(uiPlayerDropped);
                manageTeamView.RemoveFieldPlayerFromTeam(uiPlayerDropped);
                return;
            }

            // moving card from goalies to bench
            if (uiPlayerDropped.uiSlot.transform.parent.parent.parent == manageTeamView.teamView &&
                transform.parent == manageTeamView.goaliesBenchContent.transform)
            {
                uiPlayerDropped.uiSlot.uiPlayer = null;
                manageTeamView.goaliesBenchContent.AddPlayer(uiPlayerDropped);
                return;
            }
            
            // swap cards inside five or goalies
            if ((uiPlayerDropped.uiSlot.transform.parent.parent == transform.parent.parent || 
                 uiPlayerDropped.uiSlot.transform.parent.parent.parent == transform.parent.parent.parent) &&
                uiPlayer)
            {
                ProcessSwap(uiPlayerDropped);
                return;
            }
            
            // swap from bench to goalies
            if (uiPlayer &&
                uiPlayerDropped.uiSlot.transform.parent ==
                manageTeamView.goaliesBenchContent.transform)
            {
                manageTeamView.goaliesBenchContent.ReplacePlayer(uiPlayerDropped, uiPlayer);
                ProcessSwap(uiPlayerDropped);
                return;
            }

            // swap from bench to five
            if (uiPlayer && uiPlayerDropped.uiSlot.transform.parent ==
                manageTeamView.fieldPlayersBenchContent.transform)
            {
                manageTeamView.RemoveFieldPlayerFromTeam(uiPlayer);
                manageTeamView.fieldPlayersBenchContent.ReplacePlayer(uiPlayerDropped, uiPlayer);
                manageTeamView.AddFieldPlayerToTeam(uiPlayerDropped);
                ProcessSwap(uiPlayerDropped);
                return;
            }
            
            FinalizeDrop(uiPlayerDropped);
        }

    }
}