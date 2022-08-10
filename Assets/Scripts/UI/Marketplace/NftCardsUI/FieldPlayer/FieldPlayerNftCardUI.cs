using Near;
using Near.Models;
using Near.Models.Tokens;
using NearClientUnity.Utilities;
using Runtime;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI.Marketplace.NftCardsUI.FieldPlayer
{
    class FieldPlayerRenderer : ICardRenderer
    {
        private readonly Token _token;
        
        public FieldPlayerRenderer(Token token)
        {
            _token = token;
        }
        
        public NftCardUI RenderCardTile(Transform content)
        {
            FieldPlayerNftCardUI fieldPlayerNftCardUI =
                Object.Instantiate(Game.AssetRoot.marketplaceAsset.fieldPlayerCardTile, content);
            
            fieldPlayerNftCardUI.Name.text = _token.title;
            fieldPlayerNftCardUI.OwnerId.text = _token.ownerId!= NearPersistentManager.Instance.GetAccountId() 
                ? "Owner: " + _token.ownerId: "You are the owner";
            
            if (_token.marketplace_data == null)
            {
                fieldPlayerNftCardUI.Price.gameObject.SetActive(false);
            } 
            else if (_token.marketplace_data.offers != null && _token.marketplace_data.isAuction)
            {
                fieldPlayerNftCardUI.Cost.text = "Auction";
            }
            else 
            {
                fieldPlayerNftCardUI.Cost.text = "Price: " + NearUtils.FormatNearAmount(UInt128.Parse(_token.marketplace_data.price));
            }

            Near.Models.Tokens.Players.FieldPlayer.FieldPlayer fieldPlayer =
                (Near.Models.Tokens.Players.FieldPlayer.FieldPlayer)_token;
            

            fieldPlayerNftCardUI.LoadImage(_token.media);
            fieldPlayerNftCardUI.Number.text = fieldPlayer.number.ToString();
            /*
            fieldPlayer.Position.text = Utils.Utils.ConvertPosition(extra.Position);
            fieldPlayer.Role.text = extra.Role;
            
            fieldPlayer.Skating.text = extra.Stats.Skating.ToString();
            fieldPlayer.Shooting.text = extra.Stats.Shooting.ToString();
            fieldPlayer.Strength.text = extra.Stats.Strength.ToString();
            fieldPlayer.Iq.text = extra.Stats.IQ.ToString();
            */
            fieldPlayerNftCardUI.Morale.text = fieldPlayer.Stats.Morale.ToString();
            
            return fieldPlayerNftCardUI;
        }

        public NftCardDescriptionUI RenderCardDescription(Transform content)
        {
            FieldPlayerDescriptionUI cardDescription =
                Object.Instantiate(Game.AssetRoot.marketplaceAsset.fieldPlayerCardDescription, content);

            Near.Models.Tokens.Players.FieldPlayer.FieldPlayer fieldPlayer =
                (Near.Models.Tokens.Players.FieldPlayer.FieldPlayer)_token;
            
            cardDescription.OwnerId.text =  _token.ownerId != NearPersistentManager.Instance.GetAccountId() 
                ? "Owner: " + _token.ownerId : "You are the owner";
            
            // TODO: parse royalty 
            /*
            foreach (var royalty in _nftSaleInfo.NFT.royalty)
            {
                cardDescription.Royalty.text = royalty.Value / 100 + "% - " + royalty.Key;
            }
            */
            
            cardDescription.Name.text = _token.title;
            /*
            cardDescription.Position.text = "Position: " + extra.Position;
            cardDescription.Role.text = "Role: " + extra.Role;

            cardDescription.Skating.text = "skating: " + extra.Stats.Skating;
            cardDescription.Shooting.text = "shooting: " + extra.Stats.Shooting;
            cardDescription.Strength.text = "strength: " + extra.Stats.Shooting;
            cardDescription.Iq.text = "IQ: " + extra.Stats.IQ;
            */
            cardDescription.Morale.text = "morale: " + fieldPlayer.Stats.Morale;

            return cardDescription;
        }
    }
    
    public class FieldPlayerNftCardUI : NftCardUI
    {
        [SerializeField] private Text number;
        [SerializeField] private Text position;
        [SerializeField] private Text role;
        
        [SerializeField] private Text skating;
        [SerializeField] private Text shooting;
        [SerializeField] private Text strength;
        [SerializeField] private Text iq;
        [SerializeField] private Text morale;

        public Text Number => number;
        public Text Position => position;
        public Text Role => role;

        public Text Skating => skating;
        public Text Shooting => shooting;
        public Text Strength => strength;
        public Text Iq => iq;
        public Text Morale => morale;
        
        protected override ICardRenderer GetCardRenderer()
        {
            return new FieldPlayerRenderer(token);
        }
    }
}