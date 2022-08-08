using System.Collections.Generic;
using System.Threading.Tasks;
using Near.MarketplaceContract.ContractMethods;
using Near.Models.Tokens;

namespace UI.Marketplace
{
    public class MarketplaceController
    {
        private List<NFT> _nftSalesInfo;
        private List<NFT> _userNFTs;

        public async Task<List<NFT>> GetUserNFTsToBuy()
        {
            _nftSalesInfo ??= await Views.GetNFTsToBuy();
            
            return _nftSalesInfo;
        }

        public async Task<List<NFT>> GetUserNFTs()
        {
            return await Views.GetUserNFTs();
        }

        public async Task<List<NFT>> GetUserNFTsOnSale()
        {
            return await Views.GetUserNFTsOnSale();
        }

        public void SaleUpdate(Dictionary<string, string> newSaleConditions, string tokenId, bool isAuction)
        {
            Actions.SaleUpdate(newSaleConditions, tokenId, isAuction);
        }

        public void RemoveSale(string tokenId)
        {
            Actions.RemoveSale(tokenId);
        } 

        public void Offer(string tokenId, string offerToken, string price)
        {
            Actions.Offer(tokenId, offerToken, price);
        }

        public void AcceptOffer(string tokenId)
        {
            Actions.AcceptOffer(tokenId);
        }
    }
}