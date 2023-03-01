using UI.Scripts;
using UnityEngine;

namespace UI
{
    public class CustomSlider : UiComponent
    {
        private Transform _handleSlideArea;
        private const string UnselectedValuePath = "/Assets/Resources/Prefabs/Slider/UnselectedValueImg";
        
        protected override void Initialize()
        {
            _handleSlideArea = UiUtils.FindChild<Transform>(transform, "Handle Slide Area");
            
        }
    }
}