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
            // destroying slot if it's card moved from bench to five or goalies
            if (draggableCardDropped.uiSlot.transform.parent == manageTeamView.fieldPlayersBenchContent.transform ||
                draggableCardDropped.uiSlot.transform.parent == manageTeamView.goaliesBenchContent.transform )
            {
                Destroy(draggableCardDropped.uiSlot.gameObject);
            }
            
            draggableCardDropped.uiSlot.draggableCard = null;
            draggableCard = draggableCardDropped;

            draggableCard.uiSlot = this;
            
            draggableCardDropped.rectTransform.sizeDelta = RectTransform.sizeDelta;
            draggableCardDropped.rectTransform.localScale = RectTransform.localScale;
            EndDrop(draggableCardDropped);
        }

        private void ProcessSwap(DraggableCard draggableCardDropped)
        {
            // TODO: change team data
            DraggableCard previousDraggableCard = draggableCard;
            UISlot benchSlot = draggableCardDropped.uiSlot;
            
            benchSlot.draggableCard = previousDraggableCard;
            Transform previousUIPlayerTransform = previousDraggableCard.transform;
            previousUIPlayerTransform.SetParent(benchSlot.transform);
            previousUIPlayerTransform.localPosition = Vector3.zero;
            previousDraggableCard.rectTransform.sizeDelta = benchSlot.RectTransform.sizeDelta;
            previousDraggableCard.uiSlot = benchSlot;
            
            draggableCardDropped.rectTransform.sizeDelta = RectTransform.sizeDelta;
            draggableCardDropped.rectTransform.localScale = RectTransform.localScale;
            EndDrop(draggableCardDropped);
            draggableCard = draggableCardDropped;
            draggableCard.uiSlot = this;
        }

        public void OnDrop(PointerEventData eventData)
        {
            DraggableCard draggableCardDropped = eventData.pointerDrag.GetComponent<DraggableCard>();
            
            // moving card back to bench slot if it is moved to another bench slot
            if (transform.parent == manageTeamView.fieldPlayersBenchContent.transform && 
                draggableCardDropped.uiSlot.transform.parent == manageTeamView.fieldPlayersBenchContent.transform ||
                transform.parent == manageTeamView.goaliesBenchContent.transform && 
                draggableCardDropped.uiSlot.transform.parent == manageTeamView.goaliesBenchContent.transform
                )
            {
                EndDrop(draggableCardDropped);
                draggableCardDropped.transform.SetParent(draggableCardDropped.uiSlot.transform); 
                return;
            }
            
            // moving card from five to bench
            if (draggableCardDropped.uiSlot.transform.parent.parent == manageTeamView.teamView &&
                transform.parent == manageTeamView.fieldPlayersBenchContent.transform)
            {
                draggableCardDropped.uiSlot.draggableCard = null;
                // TODO
                //manageTeamView.CreateNewBenchSlotWithPlayer(manageTeamView.fieldPlayersBenchContent, draggableCardDropped);
                return;
            }

            // moving card from goalies to bench
            if (draggableCardDropped.uiSlot.transform.parent.parent.parent == manageTeamView.teamView &&
                transform.parent == manageTeamView.goaliesBenchContent.transform)
            {
                draggableCardDropped.uiSlot.draggableCard = null;
                // TODO
                //manageTeamView.CreateNewBenchSlotWithPlayer(manageTeamView.goaliesBenchContent, draggableCardDropped);
                return;
                
            }
            
            // swap cards inside five or goalies
            if ((draggableCardDropped.uiSlot.transform.parent.parent == transform.parent.parent || 
                 draggableCardDropped.uiSlot.transform.parent.parent.parent == transform.parent.parent.parent) &&
                draggableCard)
            {
                ProcessSwap(draggableCardDropped);
                return;
            }
            
            // swap from bench to five or goalies
            if (draggableCard &&
                (draggableCardDropped.uiSlot.transform.parent == manageTeamView.fieldPlayersBenchContent.transform ||
                draggableCardDropped.uiSlot.transform.parent == manageTeamView.goaliesBenchContent.transform)
                )
            {
                ProcessSwap(draggableCardDropped);
                return;
            }
            FinalizeDrop(draggableCardDropped);
        }

    }
}