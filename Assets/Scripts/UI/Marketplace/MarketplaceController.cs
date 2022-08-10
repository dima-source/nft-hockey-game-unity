using System.Collections.Generic;
using System.Threading.Tasks;
using Near.MarketplaceContract.ContractMethods;
using Near.Models.Tokens;
using Near.Models.Tokens.Filters;

namespace UI.Marketplace
{
    public class MarketplaceController
    {
        private List<Token> _nftSalesInfo;
        private List<Token> _userNFTs;

        public async Task<List<Token>> GetUserNFTsToBuy()
        {
            _nftSalesInfo ??= await Views.GetNFTsToBuy();
            
            return _nftSalesInfo;
        }

        public async Task<List<Token>> GetUserNFTs(PlayerFiler filer, Pagination pagination)
        {
            
            return await Views.GetTokens(filer, pagination);
        }

        public async Task<List<Token>> GetUserNFTsOnSale()
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