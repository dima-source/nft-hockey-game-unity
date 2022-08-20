using System;
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

        public void OnDrop(PointerEventData eventData)
        {
            UIPlayer uiPlayerDropped = eventData.pointerDrag.GetComponent<UIPlayer>();
            
            if (transform.parent == manageTeamView.benchContent.transform)
            {
                EndDrop(uiPlayerDropped);
                uiPlayerDropped.transform.SetParent(uiPlayerDropped.uiSlot.transform); 
                return;
            }
            if (uiPlayer)
            {
                EndDrop(uiPlayerDropped);
                return;
            }
            
            if (uiPlayerDropped.uiSlot.transform.parent == manageTeamView.benchContent.transform)
            {
                Destroy(uiPlayerDropped.uiSlot.gameObject);
            }


            uiPlayer = uiPlayerDropped;

            uiPlayerDropped.uiSlot = this;
            
            // if (uiPlayerDropped == null || uiPlayer == null)
            // {
            //     return;
            // }
            
            // Transform uiPlayerTransform = uiPlayer.transform;
            // uiPlayerTransform.SetParent(uiPlayerDropped.uiSlot.transform);
            // uiPlayerTransform.localPosition = Vector3.zero;

            uiPlayerDropped.RectTransform.sizeDelta = RectTransform.sizeDelta;
            EndDrop(uiPlayerDropped);
            // manageTeamView.SwapCards(uiPlayer, uiPlayerDropped);
        }
    }
}