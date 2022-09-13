using UnityEngine;

namespace UI.Scripts.Card
{
    public class CardView : UiComponent
    {

        public PlayerCardData playerCardData;

        private CardViewPrototype viewPrototype;
        public RectTransform rectTransform => GetComponent<RectTransform>();


        protected override void Initialize()
        {
            playerCardData = PlayerCardData.FromDefaultValues();
            viewPrototype = new CardViewPrototype(transform, playerCardData);
        }

        protected override void OnUpdate()
        {
            viewPrototype.UpdateView();
        }


        public void Enable(bool value)
        {
            MonoBehaviour[] comps = GetComponentsInChildren<MonoBehaviour>();
            foreach(MonoBehaviour c in comps)
            {
                c.enabled = value;
            }
        }
        
    }
}
