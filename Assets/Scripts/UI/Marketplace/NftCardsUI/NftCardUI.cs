using Near.Models;
using Near.Models.Tokens;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Marketplace.NftCardsUI
{
    public abstract class NftCardUI : MonoBehaviour
    {
        [SerializeField] protected Image image;
        [SerializeField] protected Text nameNftCard;
        [SerializeField] protected Text cost;
        [SerializeField] protected Text ownerId;
        [SerializeField] protected Transform price;
        [SerializeField] protected Button chooseButton;

        protected NFT NFT;
        
        private ICardLoader _cardLoader;
        
        public Text Name => nameNftCard;
        public Text Cost => cost;
        public Transform Price => price;
        public Text OwnerId => ownerId;
        
        protected abstract ICardRenderer GetCardRenderer();
        
        public void PrepareNftCard(ICardLoader cardLoader, NFT nft, Transform content)
        {
            _cardLoader = cardLoader;
            
            NFT = nft;
            
            ICardRenderer cardRenderer = GetCardRenderer();
            var cardTile = cardRenderer.RenderCardTile(content);
            
            cardTile.chooseButton.onClick.AddListener(OnClick);
        }
        
        private void OnClick()
        {
            _cardLoader.LoadCard(GetCardRenderer(), NFT);
        }
        
        public void LoadImage(string url)
        {
            StartCoroutine(Utils.Utils.LoadImage(image, url));
        }
    }
}