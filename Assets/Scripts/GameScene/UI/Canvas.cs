using UI.Scripts;
using UnityEngine;

namespace GameScene.UI
{
    public class Canvas : UiComponent
    {
        protected override void Initialize()
        {
            var phoneLayout = global::UI.Scripts.Utils.FindChild<GameView>(transform, "PhoneLayout");
            var tabletLayout = global::UI.Scripts.Utils.FindChild<GameView>(transform, "TabletLayout");

            var rectTransform = GetComponent<RectTransform>();

            if (rectTransform.rect.width / rectTransform.rect.height >= 2)
            {
                phoneLayout.gameObject.SetActive(true);
                tabletLayout.gameObject.SetActive(false);
            }
            else
            {
                phoneLayout.gameObject.SetActive(false);
                tabletLayout.gameObject.SetActive(true);
            }
        }
    }
}