using Near;
using Near.Models.Tokens;
using NearClientUnity.Utilities;
using Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Marketplace.NftCardsUI.Goalie
{
    class GoalieRenderer : ICardRenderer
    {
        private readonly NFT _nft;
        
        public GoalieRenderer(NFT nft)
        {
            _nft = nft;
        }
        
        public NftCardUI RenderCardTile(Transform content)
        {
            GoalieNftCardUI goalieNftCardUI =
                Object.Instantiate(Game.AssetRoot.marketplaceAsset.goalieNftCardUI, content);

            goalieNftCardUI.Name.text = _nft.Name;
            
            goalieNftCardUI.OwnerId.text = _nft.OwnerId != NearPersistentManager.Instance.GetAccountId() ? "Owner: " + _nft.Owner.Id : "You are the owner";


            if (_nft.MarketplaceData == null)
            {
                goalieNftCardUI.Price.gameObject.SetActive(false);
            } 
            else if (_nft.MarketplaceData.Offers != null && _nft.MarketplaceData.IsAuction)
            {
                goalieNftCardUI.Cost.text = "Auction";
            }
            else 
            {
                goalieNftCardUI.Cost.text = "Price: " + NearUtils.FormatNearAmount(UInt128.Parse(_nft.MarketplaceData.Price));
            }

            Near.Models.Tokens.Players.Goalie.Goalie goalie = (Near.Models.Tokens.Players.Goalie.Goalie)_nft;
            
            goalieNftCardUI.Number.text = goalie.Number.ToString();
            goalieNftCardUI.LoadImage(_nft.Media);
            /*
            goalie.Position.text = Utils.Utils.ConvertPosition(extra.Position);
            goalie.Role.text = extra.Role;

            goalie.GloveAndBlocker.text = extra.Stats.GloveAndBlocker.ToString();
            goalie.Pads.text = extra.Stats.Pads.ToString();
            goalie.Stretch.text = extra.Stats.Stretch.ToString();
            goalie.Stand.text = extra.Stats.Stand.ToString();
            goalie.Morale.text = extra.Stats.Morale.ToString();
            */
            
            return goalieNftCardUI;
        }

        public NftCardDescriptionUI RenderCardDescription(Transform content)
        {
            GoalieDescriptionUI cardDescription =
                Object.Instantiate(Game.AssetRoot.marketplaceAsset.goalieDescriptionUI, content);
            
            cardDescription.OwnerId.text =  _nft.OwnerId != NearPersistentManager.Instance.GetAccountId() 
                ? "Owner: " + _nft.OwnerId : "You are the owner";

            cardDescription.Name.text = _nft.Name;

            // TODO: parse royalties
            /*
            foreach (var royalty in _nft.PerpetualRoyalties)
            {
                cardDescription.Royalty.text = royalty.Value / 100 + "% - " + royalty.Key;
            }
            */

            Near.Models.Tokens.Players.Goalie.Goalie goalie = (Near.Models.Tokens.Players.Goalie.Goalie)_nft;

            cardDescription.Name.text = goalie.Name;
            /*
            cardDescription.Position.text = "Position: " + extra.Position;
            cardDescription.Role.text = "Role: " + extra.Role;
            
            cardDescription.GloveAndBlocker.text = "agility: " +  extra.Stats.GloveAndBlocker;
            cardDescription.Pads.text = "pads: " + extra.Stats.Pads;
            cardDescription.Stretch.text = "stretch: " + extra.Stats.Stretch;
            cardDescription.Stand.text = "stand: " + extra.Stats.Stand;
            */
            cardDescription.Morale.text = "morale: " + goalie.Stats.Morale;
            
            return cardDescription;
        }
    }
    
    public class GoalieNftCardUI : NftCardUI
    {
        [SerializeField] private Text number;
        [SerializeField] private Text position;
        [SerializeField] private Text role;
        
        [SerializeField] private Text gloveAndBlocker;
        [SerializeField] private Text pads;
        [SerializeField] private Text stand;
        [SerializeField] private Text stretch;
        [SerializeField] private Text morale;

        public Text Number => number;
        public Text Position => position;
        public Text Role => role;

        public Text GloveAndBlocker => gloveAndBlocker;
        public Text Pads => pads;
        public Text Stand => stand;
        public Text Stretch => stretch;
        public Text Morale => morale;
        
        protected override ICardRenderer GetCardRenderer()
        {
            return new GoalieRenderer(NFT);
        }
    }
}