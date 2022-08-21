using System;
using Runtime;
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
        public int slotId;
        
        public UIPlayer uiPlayer;

        protected void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        private void EndDrop(UIPlayer uiPlayerDropped)
        {
            Transform uiPlayerDroppedTransform = uiPlayerDropped.transform;
            uiPlayerDropped.transform.SetParent(transform); 
            uiPlayerDroppedTransform.localPosition = Vector3.zero;
        }

        private void FinalizeDrop(UIPlayer uiPlayerDropped)
        {
            // destroying slot if it's card moved from bench to team
            if (uiPlayerDropped.uiSlot.transform.parent == manageTeamView.benchContent.transform)
            {
                Destroy(uiPlayerDropped.uiSlot.gameObject);
            }
            
            uiPlayerDropped.uiSlot.uiPlayer = null;
            uiPlayer = uiPlayerDropped;

            uiPlayer.uiSlot = this;
            
            uiPlayerDropped.RectTransform.sizeDelta = RectTransform.sizeDelta;
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
            EndDrop(uiPlayerDropped);
            uiPlayer = uiPlayerDropped;
            uiPlayer.uiSlot = this;
        }

        public void OnDrop(PointerEventData eventData)
        {
            UIPlayer uiPlayerDropped = eventData.pointerDrag.GetComponent<UIPlayer>();
            
            // moving card back to bench slot if it is moved to another bench slot
            if (transform.parent == manageTeamView.benchContent.transform && uiPlayerDropped.uiSlot.transform.parent == manageTeamView.benchContent.transform)
            {
                EndDrop(uiPlayerDropped);
                uiPlayerDropped.transform.SetParent(uiPlayerDropped.uiSlot.transform); 
                return;
            }
            
            // moving card from five to bench
            if (uiPlayerDropped.uiSlot.transform.parent.parent == manageTeamView.teamView &&
                transform.parent == manageTeamView.benchContent.transform)
            {
                uiPlayerDropped.uiSlot.uiPlayer = null;
                manageTeamView.CreateNewBenchSlotWithPlayer(uiPlayerDropped);
                // FinalizeDrop(uiPlayerDropped);
                return;
            }
            
            // swap cards inside team
            if (uiPlayerDropped.uiSlot.transform.parent.parent == manageTeamView.teamView &&
                transform.parent.parent == manageTeamView.teamView && uiPlayer)
            {
                ProcessSwap(uiPlayerDropped);
                return;
            }
            
            // swap from bench to five
            if (uiPlayer &&
                uiPlayerDropped.uiSlot.transform.parent == manageTeamView.benchContent.transform)
            {
                ProcessSwap(uiPlayerDropped);
                return;
            }
            FinalizeDrop(uiPlayerDropped);
        }

    }
}