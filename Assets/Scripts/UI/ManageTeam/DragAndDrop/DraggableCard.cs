using System;
using Near.Models.Tokens;
using UI.Scripts.Card;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ManageTeam.DragAndDrop
{
    public abstract class DraggableCard : CardView, IDragHandler, IBeginDragHandler, IEndDragHandler
    {

        public Image playerImg;

        private CanvasGroup _canvasGroup;
        private Canvas _mainCanvas;

        public Transform canvasContent;

        public Token CardData;
        public UISlot uiSlot;

        protected void Start()
        {
            _mainCanvas = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            rectTransform.SetParent(canvasContent);
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var scaleFactor = _mainCanvas.scaleFactor;
            rectTransform.anchoredPosition += eventData.delta / scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Transform itemTransform = eventData.pointerDrag.transform;
            if (itemTransform.parent == canvasContent)
            {
                uiSlot.OnDrop(eventData);
            }

            transform.localPosition = Vector3.zero;
            _canvasGroup.blocksRaycasts = true;
        }

        public abstract void SetData(Token token);
        
    }
}