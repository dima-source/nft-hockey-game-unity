using System;
using System.Collections;
using Near.Models.Tokens;
using TMPro;
using UI.Scripts.Card;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ManageTeam.DragAndDrop
{
    public abstract class DraggableCard : CardView, IDragHandler, IBeginDragHandler, IEndDragHandler
    {

        [SerializeField] private Animation _statsChange;
        [SerializeField] private TMP_Text _statsPercentText;
        
        public Image playerImg;
        public ManageTeamView ManageTeamView;

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

        private void OnEnable()
        {
            _statsChange.gameObject.SetActive(false);
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
        
        public void PlayStatsUp(int percent)
        {
            _statsPercentText.text = $"{percent.ToString()}%";
            StartCoroutine(AnimationPlaying("StatsUp"));
        }

        public void PlayStatsDown(int percent)
        {
            _statsPercentText.text = $"{percent.ToString()}%";
            StartCoroutine(AnimationPlaying("StatsDown"));
        }
        
        private IEnumerator AnimationPlaying(string animationType)
        {
            _statsChange.gameObject.SetActive(true);
            _statsChange.Play(animationType);
            while (_statsChange.isPlaying)
            {
                yield return new WaitForSeconds(0.5f);
            }
            
            _statsChange.gameObject.SetActive(false);
        }

        public abstract void SetData(Token token);
        
    }
}