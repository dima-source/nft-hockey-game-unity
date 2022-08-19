using System;
using System.IO;
using Near.Models.Tokens;
using UI.Scripts;
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

        public Image playerImg;

        private CanvasGroup _canvasGroup;
        private Canvas _mainCanvas;
        public RectTransform RectTransform;

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
                itemTransform.SetParent(uiSlot.transform);
                itemTransform.localPosition = Vector3.zero;
            }
            
            transform.localPosition = Vector3.zero;
            _canvasGroup.blocksRaycasts = true;
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
            // return roleString switch
            // {
            //     "Playmaker" => PlayerRole.Playmaker,
            //     "Enforcer" => PlayerRole.Enforcer,
            //     "Shooter" => PlayerRole.Shooter,
            //     "Try-harder" => PlayerRole.TryHarder,
            //     "Defensive forward" => PlayerRole.DefensiveForward,
            //     "Grinder" => PlayerRole.Grinder,
            //     "Defensive defenceman" => PlayerRole.DefensiveDefenceman,
            //     "Offensive defenceman" => PlayerRole.OffensiveDefenceman,
            //     "Two-way defencemen" => PlayerRole.TwoWayDefencemen,
            //     "Tough guy" => PlayerRole.ToughGuy,
            //     "Standup" => PlayerRole.StandUp,
            //     "Butterfly" => PlayerRole.Butterfly,
            //     "Hybrid" => PlayerRole.Hybrid,
            //     _ => throw new ApplicationException("Unsupported role " + roleString)
            // };
        }
    }
}