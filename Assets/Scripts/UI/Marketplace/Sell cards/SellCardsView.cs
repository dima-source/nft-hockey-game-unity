using System;
using System.Collections.Generic;
using Near.Models.Tokens;
using Near.Models.Tokens.Filters;
using Runtime;
using UI.Marketplace.NftCardsUI;
using UnityEngine;

namespace UI.Marketplace.Sell_cards
{
    public class SellCardsView : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private ViewInteractor viewInteractor;
        [SerializeField] private SellCardView sellNftCardView;
        
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

            List<Token> nFTs = await viewInteractor.MarketplaceController.GetUserNFTs(new PlayerFiler(), new Pagination());

            foreach (Token nft in nFTs)
            {
                NftCardUI card = nft.player_type switch
                {
                    "FieldPlayer" => Instantiate(Game.AssetRoot.marketplaceAsset.fieldPlayerCardTile),
                    "Goalie" => Instantiate(Game.AssetRoot.marketplaceAsset.goalieNftCardUI),
                    _ => throw new Exception("Extra type not found")
                };

                card.PrepareNftCard(sellNftCardView, nft, content);
            }
            _isLoaded = true;
        }
    }
}