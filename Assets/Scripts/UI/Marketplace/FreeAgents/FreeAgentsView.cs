using System;
using System.Collections.Generic;
using Near.Models;
using Near.Models.Tokens;
using Runtime;
using UI.Marketplace.NftCardsUI;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Token = Near.Models.Tokens.Token;

namespace UI.Marketplace.FreeAgents
{
    public class FreeAgentsView : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private ViewInteractor viewInteractor;
        [SerializeField] private FreeAgentView freeAgentView;
        
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
            
            List<Token> nFTs = await viewInteractor.MarketplaceController.GetUserNFTsOnSale();

            foreach (Token nft in nFTs)
            {
                NftCardUI card = nft.player_type switch
                {
                    "FieldPlayer" => Instantiate(Game.AssetRoot.marketplaceAsset.fieldPlayerCardTile),
                    "Goalie" => Instantiate(Game.AssetRoot.marketplaceAsset.goalieNftCardUI),
                    "GoaliePos" => Instantiate(Game.AssetRoot.marketplaceAsset.goalieNftCardUI),
                    _ => throw new Exception("Extra type not found")
                };

                card.PrepareNftCard(freeAgentView, nft, content);
            }

            _isLoaded = true;
        }
    }
}