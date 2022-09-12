using System;
using Near.Models.Tokens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ManageTeam.DragAndDrop
{
    public abstract class UIPlayer : CardView, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        protected void setPlayerName(string name)
        {
            string[] splittedName = name.Split(" ", 2);
            if (splittedName.Length != 2)
            {
                throw new ApplicationException("Name is incorrect. Must be \"Name Surname\", got \"" + name + "\"");
            }

            playerName = splittedName[0];
            playerSurname = splittedName[1];
        }

        private CanvasGroup _canvasGroup;
        private Canvas _mainCanvas;
        public RectTransform RectTransform;
        public ManageTeamView ManageTeamView;

        public Transform canvasContent;

        public Token CardData;
        public UISlot uiSlot;

        protected void Start()
        {
            RectTransform = GetComponent<RectTransform>();
            _mainCanvas = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransform.SetParent(canvasContent);
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var scaleFactor = _mainCanvas.scaleFactor;
            RectTransform.anchoredPosition += eventData.delta / scaleFactor;
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
            ManageTeamView.UpdateTeamWork();
        }

        public abstract void SetData(Token token);

        public static Position StringToPosition(string position)
        {
            Position.TryParse(position, out Position parsedPosition);
            return parsedPosition;
        }

        public static PlayerRole StringToRole(string roleString)
        {
            PlayerRole.TryParse(roleString, out PlayerRole parsedRole);
            return parsedRole;
        }
    }
}