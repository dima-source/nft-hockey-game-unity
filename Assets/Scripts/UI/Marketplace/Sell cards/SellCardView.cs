using System.Collections.Generic;
using Near.Models.Tokens;
using NearClientUnity.Utilities;
using UI.Marketplace.NftCardsUI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Marketplace.Sell_cards
{
    public class SellCardView : MonoBehaviour, ICardLoader
    {
        [SerializeField] private UIPopupOnSell uiPopupOnSell;
        
        [SerializeField] private Transform sellView;
        [SerializeField] private Transform marketStoragePaidView;
        
        [SerializeField] private Image cardImage;
        [SerializeField] private Transform cardDescriptionContent;
        [SerializeField] private ViewInteractor viewInteractor;

        private NftCardUI _cardTile;
        private NftCardDescriptionUI _cardDescription;
        private Token _token;
        private string _marketStoragePaid;
        
        [SerializeField] private Toggle isAuction;
        [SerializeField] private InputField price;

        public void LoadCard(ICardRenderer cardRenderer, Token token)
        {
            viewInteractor.ChangeView(gameObject.transform);

            if (_cardTile != null)
            {
                Destroy(_cardTile.gameObject);
            }

            if (_cardDescription != null)
            {
                Destroy(_cardDescription.gameObject);
            }

            _token = token;

            StartCoroutine(Utils.Utils.LoadImage(cardImage, token.media));
            
            _cardDescription = cardRenderer.RenderCardDescription(cardDescriptionContent);
        }

        public void UpdatePrice()
        {
            UInt128 nearAmount = Near.NearUtils.ParseNearAmount(price.text);
            Dictionary<string, string> newSaleConditions = new Dictionary<string, string> {{"near", nearAmount.ToString()}};

            Application.deepLinkActivated += OnSellCard;
            
            viewInteractor.MarketplaceController.SaleUpdate(newSaleConditions, _token.tokenId, isAuction.isOn);
        }

        private void OnSellCard(string url)
        {
            Application.deepLinkActivated -= OnSellCard;
            uiPopupOnSell.SetData(price.text, isAuction.isOn);
            uiPopupOnSell.Show();
        }
    }
}