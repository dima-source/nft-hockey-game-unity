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
        private NFT _nft;

        private string _price;

        public void LoadCard(ICardRenderer cardRenderer, NFT nft)
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

            StartCoroutine(Utils.Utils.LoadImage(cardImage, nft.Media));
            
            _cardDescription = cardRenderer.RenderCardDescription(cardDescriptionContent);
            _nft = nft;
            
            if (nft.OwnerId == NearPersistentManager.Instance.GetAccountId())
            {
                buyButton.gameObject.SetActive(false);
                return;
            }

            if (nft.MarketplaceData == null)
            {
                return;
            }
            
            if (!nft.MarketplaceData.IsAuction)
            {
                buyImmediatelyView.gameObject.SetActive(true);
                setBidView.gameObject.SetActive(false);
                buyButtonText.text = "Buy";

                _price = NearUtils.FormatNearAmount(UInt128.Parse(nft.MarketplaceData.Price)).ToString();
                price.text = "Price: " + _price;
            }
            else
            {
                buyImmediatelyView.gameObject.SetActive(false);
                setBidView.gameObject.SetActive(true);
                buyButtonText.text = "Set";
                
                BidText saleConditionText = Instantiate(Game.AssetRoot.marketplaceAsset.bid, bidContent);
                
                saleConditionText.bid.text = "Sale conditions" + ":  " + NearUtils.FormatNearAmount(UInt128.Parse(nft.MarketplaceData.Price));
                
                _bidTexts.Add(saleConditionText);
                
                foreach (Offer offer in nft.MarketplaceData.Offers)
                {
                    BidText bidText = Instantiate(Game.AssetRoot.marketplaceAsset.bid, bidContent);

                    string ownerId = offer.User.Id != NearPersistentManager.Instance.GetAccountId() ? offer.User.Id : "Your bid";
                    bidText.bid.text = ownerId + ":  " + NearUtils.FormatNearAmount(UInt128.Parse(offer.Price));
                    
                    _bidTexts.Add(bidText);
                }
            }
        }

        public void BuyCard()
        {
            if (_nft.MarketplaceData.IsAuction)
            {
                _price = bid.text;
                Application.deepLinkActivated += OnSetBid;
            }
            else
            {
                Application.deepLinkActivated += OnBuyCard;
            }
            
            viewInteractor.MarketplaceController.Offer(_nft.TokenId, "near", _price);
        }

        private void OnBuyCard(string url)
        {
            Application.deepLinkActivated -= OnBuyCard;
            uiPopupPurchasedCard.Show();
            uiPopupPurchasedCard.SetData(_nft, popupContent);
        }

        private void OnSetBid(string url)
        {
            Application.deepLinkActivated -= OnSetBid;

            uiPopupSetBid.SetData(bid.text);
            uiPopupSetBid.Show();
        }
    }
}