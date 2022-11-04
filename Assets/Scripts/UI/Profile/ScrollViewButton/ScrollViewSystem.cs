using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Profile.ScrollViewButton
{
    public class ScrollViewSystem : MonoBehaviour
    {
        private ScrollRect _scrollRect;
        [SerializeField] private ScrollButton _leftButton;
        [SerializeField] private ScrollButton _rightButton;
        [SerializeField] private ScrollButton _bottomButton;
        [SerializeField] private ScrollButton _topButton;
        [SerializeField] private float scrollSpeed = 0.01f;

        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

        private void Update()
        {
            if (_leftButton != null)
            {
                if (_leftButton.isDown)
                {
                    ScrollLeft();
                }
            }
            if (_rightButton != null)
            {
                if (_rightButton.isDown)
                {
                    ScrollRight();
                }
            }
            if (_bottomButton != null)
            {
                if (_bottomButton.isDown)
                {
                    ScrollBottom();
                }
            }
            if (_topButton != null)
            {
                if (_topButton.isDown)
                {
                    ScrollTop();
                }
            }
        }

        private void ScrollLeft()
        {
            if (_scrollRect != null)
            {
                if (_scrollRect.horizontalNormalizedPosition >= 0f)
                {
                    _scrollRect.horizontalNormalizedPosition -= scrollSpeed;
                }
            }
        }
        private void ScrollRight()
        {
            if (_scrollRect != null)
            {
                if (_scrollRect.horizontalNormalizedPosition <= 1f)
                {
                    _scrollRect.horizontalNormalizedPosition += scrollSpeed;
                }
            }
        }
        private void ScrollBottom()
        {
            if (_scrollRect != null)
            {
                if (_scrollRect.verticalNormalizedPosition <= 1f)
                {
                    _scrollRect.verticalNormalizedPosition += scrollSpeed;
                }
            }
        }
        private void ScrollTop()
        {
            if (_scrollRect != null)
            {
                if (_scrollRect.verticalNormalizedPosition >= 0f)
                {
                    _scrollRect.verticalNormalizedPosition -= scrollSpeed;
                }
            }
            
        }
    }
}