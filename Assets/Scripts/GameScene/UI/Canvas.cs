using UI.Scripts;
using UnityEngine;

namespace GameScene.UI
{
    public class Canvas : UiComponent
    {
        protected override void Initialize()
        {
            var phoneLayout = global::UI.Scripts.UiUtils.FindChild<GameView>(transform, "PhoneLayout");
            var tabletLayout = global::UI.Scripts.UiUtils.FindChild<GameView>(transform, "TabletLayout");

            var rectTransform = GetComponent<RectTransform>();
            var rect = rectTransform.rect;
            
            var aspectRatio = rect.width / rect.height;
            if (aspectRatio >= 2)
            {
                phoneLayout.gameObject.SetActive(true);
                tabletLayout.gameObject.SetActive(false);
            }
            else
            {
                phoneLayout.gameObject.SetActive(false);
                tabletLayout.gameObject.SetActive(true);
                
                var tabletBottom = global::UI.Scripts.UiUtils.FindChild<Transform>(transform, "BottomPanelTablet");
                var pcBottom = global::UI.Scripts.UiUtils.FindChild<Transform>(transform, "BottomPanelPC");
                
                if (aspectRatio >= 1.6)
                {
                    pcBottom.gameObject.SetActive(true);    
                    tabletBottom.gameObject.SetActive(false);
                }
                else
                {
                    pcBottom.gameObject.SetActive(false);    
                    tabletBottom.gameObject.SetActive(true);
                }
            }
        }
    }
}