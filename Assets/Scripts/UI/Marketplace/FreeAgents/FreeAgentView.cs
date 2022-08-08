using System.Collections.Generic;
using Near;
using Near.Models.Game.Bid;
using Near.Models.Tokens;
using NearClientUnity.Utilities;
using Runtime;
using UI.Marketplace.Buy_cards;
using UI.Marketplace.FreeAgents.UIPopups;
using UI.Marketplace.NftCardsUI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Marketplace.FreeAgents
{
    public class FreeAgentView : MonoBehaviour, ICardLoader
    {
        [SerializeField] private UIPopupOnAccept uiPopupOnAccept;
        [SerializeField] private UIPopupOnRemoveSale uiPopupOnRemoveSale;
        [SerializeField] private UIPopupOnUpdatePrice uiPopupOnUpdatePrice;
        
        [SerializeField] private Transform bidsView;
        [SerializeField] private Transform buyImmediatelyView;
        
        [SerializeField] private Transform bidContent;
            
        [SerializeField] private Image cardImage;
        [SerializeField] private Text price;
        
        [SerializeField] private Transform cardDescriptionContent;
        [SerializeField] private ViewInteractor viewInteractor;

        [SerializeField] private Transform setNewPriceView;

        [SerializeField] private Text currentSaleConditions;
        [SerializeField] private InputField newSaleConditions;

        private List<BidText> _bidTexts;

        private NftCardUI _cardTile;
        private NftCardDescriptionUI _cardDescription;
        private NFT _nft;

        private string _price;
        private string _bidToAccept;
        
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

            if (nft.MarketplaceData == null)
            {
                return;
            }
            
            if (nft.MarketplaceData.Offers == null || nft.MarketplaceData.Offers.Count == 0)
            {
                buyImmediatelyView.gameObject.SetActive(true);
                bidsView.gameObject.SetActive(false);

                _price = NearUtils.FormatNearAmount(UInt128.Parse(nft.MarketplaceData.Price)).ToString();
                price.text = "Price: " + _price;
                currentSaleConditions.text = _price;
            }
            else
            {
                buyImmediatelyView.gameObject.SetActive(false);
                bidsView.gameObject.SetActive(true);
                
                BidText saleConditionText = Instantiate(Game.AssetRoot.marketplaceAsset.bid, bidContent);

                saleConditionText.bid.text = "Sale conditions" + ":  " + NearUtils.FormatNearAmount(UInt128.Parse(_nft.MarketplaceData.Price));

                currentSaleConditions.text = saleConditionText.bid.text;

                _bidTexts.Add(saleConditionText);

                // TODO: 
                //_bidToAccept = nftSaleInfo.Sale.bids["near"][0].price;
                
                foreach (Offer offer in _nft.MarketplaceData.Offers)
                {
                    BidText bidText = Instantiate(Game.AssetRoot.marketplaceAsset.bid, bidContent);

                    string ownerId = offer.User.Id != NearPersistentManager.Instance.GetAccountId() ? offer.User.Id: "Your bid";
                    bidText.bid.text = ownerId + ":  " + NearUtils.FormatNearAmount(UInt128.Parse(offer.Price));
                    
                    _bidTexts.Add(bidText);
                }
            }
        }

        public void AcceptOffer()
        {
            Application.deepLinkActivated += OnAcceptOffer;
            viewInteractor.MarketplaceController.AcceptOffer(_nft.TokenId);
        }

        private void OnAcceptOffer(string url)
        {
            Application.deepLinkActivated -= OnAcceptOffer;
            
            uiPopupOnAccept.SetData(_bidToAccept); 
            uiPopupOnAccept.Show();
        }

        public void ShowSetNewPriceView()
        {
            setNewPriceView.gameObject.SetActive(true);
            setNewPriceView.transform.SetAsLastSibling();
        }

        public void CloseSetNewPriceView()
        {
            setNewPriceView.gameObject.SetActive(false);
        }

        public void UpdatePrice()
        {
            UInt128 nearAmount = NearUtils.ParseNearAmount(newSaleConditions.text);

            Dictionary<string, string> newSale = new Dictionary<string, string> {{"near", nearAmount.ToString()}};

            Application.deepLinkActivated += OnUpdatePrice;
            
            viewInteractor.MarketplaceController.SaleUpdate(newSale, _nft.TokenId, _nft.MarketplaceData.IsAuction);
            
            CloseSetNewPriceView();
        }

        private void OnUpdatePrice(string url)
        {
            Application.deepLinkActivated -= OnUpdatePrice;
            
            uiPopupOnUpdatePrice.SetData(newSaleConditions.text);
            uiPopupOnUpdatePrice.Show();
        }

        public void RemoveSale()
        {
            Application.deepLinkActivated += OnRemoveSale;
            viewInteractor.MarketplaceController.RemoveSale(_nft.TokenId);
        }

        private void OnRemoveSale(string url)
        {
            Application.deepLinkActivated -= OnRemoveSale;
            uiPopupOnRemoveSale.Show();
        }
    }
}