using System;
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

        public void OnDrop(PointerEventData eventData)
        {
            UIPlayer uiPlayerDropped = eventData.pointerDrag.GetComponent<UIPlayer>();
            
            // if (uiPlayerDropped == null || uiPlayer == null)
            // {
            //     return;
            // }
            
            // Transform uiPlayerTransform = uiPlayer.transform;
            // uiPlayerTransform.SetParent(uiPlayerDropped.uiSlot.transform);
            // uiPlayerTransform.localPosition = Vector3.zero;

            Transform uiPlayerDroppedTransform = uiPlayerDropped.transform;
            uiPlayerDroppedTransform.SetParent(transform); 
            uiPlayerDroppedTransform.localPosition = Vector3.zero;
            uiPlayerDropped.RectTransform.sizeDelta = RectTransform.sizeDelta;

            // manageTeamView.SwapCards(uiPlayer, uiPlayerDropped);
        }
    }
}