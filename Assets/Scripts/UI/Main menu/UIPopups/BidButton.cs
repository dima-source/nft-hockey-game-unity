using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_menu.UIPopups
{
    public class BidButton : MonoBehaviour
    {
        [SerializeField] private UIPopupSetBid uiPopupSetBid;

        public Image image;
        
        public string bid;
        
        public Sprite activeSprite;
        public Sprite defaultSprite;
        public Color activeColor;
        public Color defaultColor;
        
        public void ChangeActiveButton()
        {
            uiPopupSetBid.ChangeActiveButton(this);
        }
    }
}