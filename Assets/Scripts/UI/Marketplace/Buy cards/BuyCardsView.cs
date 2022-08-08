using System;
using System.Collections.Generic;
using Near.Models.Tokens;
using Runtime;
using UI.Marketplace.NftCardsUI;
using UnityEngine;

namespace UI.Marketplace.Buy_cards
{
    public class BuyCardsView : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private ViewInteractor viewInteractor;
        [SerializeField] private BuyCardView buyNftCardView;
        
        private bool _isLoaded;

        private void Awake()
        {
            _isLoaded = false;
        }

        public async void LoadNftCards()
        {
            viewInteractor.ChangeView(gameObject.transform);
            
            if (_isLoaded)
            {
                return;
            }
            
            List<NFT> nFTs = await viewInteractor.MarketplaceController.GetUserNFTsToBuy();

            foreach (NFT nft in nFTs)
            {
                NftCardUI card = nft.TokenType switch
                {
                    "FieldPlayer" => Instantiate(Game.AssetRoot.marketplaceAsset.fieldPlayerCardTile),
                    "Goalie" => Instantiate(Game.AssetRoot.marketplaceAsset.goalieNftCardUI),
                    "GoaliePos" => Instantiate(Game.AssetRoot.marketplaceAsset.goalieNftCardUI),
                    _ => throw new Exception("Extra type not found")
                };

                card.PrepareNftCard(buyNftCardView, nft, content);
            }

            _isLoaded = true;
        }
    }
}