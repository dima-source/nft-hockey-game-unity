using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Profile.ScrollViewButton
{
    public class ScrollButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool isDown = false;
        public void OnPointerDown(PointerEventData eventData)
        {
            isDown = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isDown = false;
        }
    }
}