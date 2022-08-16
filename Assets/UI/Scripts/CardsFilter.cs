using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class CardsFilter : UiComponent
    {
        
        private class LayoutSettings
        {
            private Vector2 _cellSize;
            private Vector2 _spacing;

            public LayoutSettings(Vector2 cellSize, Vector2 spacing)
            {
                _cellSize = cellSize;
                _spacing = spacing;
            }

            public void CopyValues(GridLayoutGroup layout)
            {
                layout.spacing = _spacing;
                layout.cellSize = _cellSize;
            }
        }

        private Transform _layoutContainer, _togglesContainer;
        private GridLayoutGroup _layout;

        private static readonly LayoutSettings Settings3x3 = 
            new LayoutSettings(new Vector2(400, 640), 
                new Vector2(20, 20));
        
        private static readonly LayoutSettings Settings2x2 = 
            new LayoutSettings(new Vector2(500, 800), 
                new Vector2(50, 50));
        
        
        protected override void Initialize()
        {
            _layoutContainer = Utils.FindChild<Transform>(transform, "Layout");
            _layout = Utils.FindChild<GridLayoutGroup>(_layoutContainer, "Content");
            Transform temp = Utils.FindChild<Transform>(transform, "FilterMenu");
            _togglesContainer = Utils.FindChild<Transform>(temp, "Content");

            foreach (Transform child in _togglesContainer)
            {
                child.GetComponent<ToggleGroup>().onChange = OnInputChanged;
            }
        }

        public void OnGrid3x3Click()
        {
            PlaySound();
            Settings3x3.CopyValues(_layout);
        }
        
        public void OnGrid2x2Click()
        {
            PlaySound();
            Settings2x2.CopyValues(_layout);
        }
        
        public void OnLinesButtonClick()
        {
            PlaySound();
        }

        public void OnSearchChanged()
        {
            PlaySound();
            OnInputChanged();
        }

        private void OnInputChanged()
        {
            // Rebuild content here 
        }
        

        private void PlaySound()
        {
            AudioController.LoadClip(Configurations.DefaultButtonSoundPath);
            AudioController.source.Play();
        }
        
    }
}
