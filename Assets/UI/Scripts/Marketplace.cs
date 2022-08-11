using System;
using UnityEngine;

namespace UI.Scripts
{
    public class Marketplace : UiComponent
    {
        private Popup _popup;
        private Transform _pagesContainer;
        [SerializeField]
        private Transform[] _pages;

        private TopBar _topBar;
        
        protected override void Initialize()
        {
            _popup = Utils.FindChild<Popup>(transform, "Popup");
            _pagesContainer = Utils.FindChild<Transform>(transform, "Main");
            _topBar = Utils.FindChild<TopBar>(transform, "TopBar");
            InitializePages();
        }

        private void InitializePages()
        {
            _pages = new[]
            {
                Utils.FindChild<Transform>(_pagesContainer, "BuyPacks"),
                Utils.FindChild<Transform>(_pagesContainer, "Statistics")
            };
        }
    }
}
