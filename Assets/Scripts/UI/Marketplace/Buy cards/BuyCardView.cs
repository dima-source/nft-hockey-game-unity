using System.Collections.Generic;
using Near;
using Near.Models;
using Near.Models.Game;
using Near.Models.Game.Bid;
using Near.Models.Tokens;
using NearClientUnity.Utilities;
using Runtime;
using UI.Marketplace.Buy_cards.UIPopups;
using UI.Marketplace.NftCardsUI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Marketplace.Buy_cards
{
    public class BuyCardView : MonoBehaviour, ICardLoader
    {
        [SerializeField] private UIPopupPurchasedCard uiPopupPurchasedCard;
        [SerializeField] private UIPopupSetBid uiPopupSetBid;

        [SerializeField] private Transform popupContent;
        
        [SerializeField] private Transform setBidView;
        [SerializeField] private Transform buyImmediatelyView;
        
        [SerializeField] private Transform bidContent;
            
        [SerializeField] private Image cardImage;
        [SerializeField] private Text price;
        
        [SerializeField] private Button buyButton;
        [SerializeField] private Text buyButtonText;
        
        [SerializeField] private Transform cardDescriptionContent;
        [SerializeField] private ViewInteractor viewInteractor;
        
        [SerializeField] private InputField bid;

        private List<BidText> _bidTexts;

        private NftCardUI _cardTile;
        private NftCardDescriptionUI _cardDescription;
        private Token _token;

        private string _price;

        public void LoadCard(ICardRenderer cardRenderer, Token token)
        {
            viewInteractor.ChangeView(gameObject.transform);

            if (_bidTexts != null)
            {
                foreach (BidText bidText in _bidTexts)
                {
                    Destroy(bidText.gameObject);
                }
            }
            
            _bidTexts = new List<BidText>();

            if (_cardTile != null)
            {
                Destroy(_cardTile.gameObject);
            }
            
            if (_cardDescription != null)
            {
                Destroy(_cardDescription.gameObject);
            }

            StartCoroutine(Utils.Utils.LoadImage(cardImage, token.media));
            
            _cardDescription = cardRenderer.RenderCardDescription(cardDescriptionContent);
            _token = token;
            
            if (token.ownerId == NearPersistentManager.Instance.GetAccountId())
            {
                buyButton.gameObject.SetActive(false);
                return;
            }

            if (token.marketplace_data == null)
            {
                return;
            }
            
            if (!token.marketplace_data.isAuction)
            {
                buyImmediatelyView.gameObject.SetActive(true);
                setBidView.gameObject.SetActive(false);
                buyButtonText.text = "Buy";

                _price = NearUtils.FormatNearAmount(UInt128.Parse(token.marketplace_data.price)).ToString();
                price.text = "Price: " + _price;
            }
            else
            {
                buyImmediatelyView.gameObject.SetActive(false);
                setBidView.gameObject.SetActive(true);
                buyButtonText.text = "Set";
                
                BidText saleConditionText = Instantiate(Game.AssetRoot.marketplaceAsset.bid, bidContent);
                
                saleConditionText.bid.text = "Sale conditions" + ":  " + NearUtils.FormatNearAmount(UInt128.Parse(token.marketplace_data.price));
                
                _bidTexts.Add(saleConditionText);
                
                foreach (Offer offer in token.marketplace_data.offers)
                {
                    BidText bidText = Instantiate(Game.AssetRoot.marketplaceAsset.bid, bidContent);

                    string ownerId = offer.user.id != NearPersistentManager.Instance.GetAccountId() ? offer.user.id : "Your bid";
                    bidText.bid.text = ownerId + ":  " + NearUtils.FormatNearAmount(UInt128.Parse(offer.price));
                    
                    _bidTexts.Add(bidText);
                }
            }
        }

        public void BuyCard()
        {
            if (_token.marketplace_data.isAuction)
            {
                _price = bid.text;
                Application.deepLinkActivated += OnSetBid;
            }
            else
            {
                Application.deepLinkActivated += OnBuyCard;
            }
            
            viewInteractor.MarketplaceController.Offer(_token.tokenId, "near", _price);
        }

        private void OnBuyCard(string url)
        {
            Application.deepLinkActivated -= OnBuyCard;
            uiPopupPurchasedCard.Show();
            uiPopupPurchasedCard.SetData(_token, popupContent);
        }

        private void OnSetBid(string url)
        {
            Application.deepLinkActivated -= OnSetBid;

            uiPopupSetBid.SetData(bid.text);
            uiPopupSetBid.Show();
        }
    }
}