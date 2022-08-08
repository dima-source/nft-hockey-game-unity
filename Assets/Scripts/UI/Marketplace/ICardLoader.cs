using Near.Models.Tokens;

namespace UI.Marketplace
{
    public interface ICardLoader
    {
        public void LoadCard(ICardRenderer cardRenderer, NFT nft);
    }
}