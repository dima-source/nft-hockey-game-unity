using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using NearClientUnity;
using NearClientUnity.Utilities;
using Newtonsoft.Json;
using UnityEngine;

namespace Near.MarketplaceContract.ContractMethods
{
    public static class Actions
    {
        public static async void SaleUpdate(Dictionary<string, string> newSaleConditions, string tokenId, bool isAuction)
        {
            ContractNear marketContract = await NearPersistentManager.Instance.GetMarketplaceContract();

            dynamic saleArgs = new ExpandoObject();
            saleArgs.nft_contract_token = NearPersistentManager.Instance.nftContactId + "||" + tokenId;

            dynamic sale = await marketContract.View("get_sale", saleArgs);

            // TODO: Implement for other fts
            if (sale.result.ToString() != "null")
            {
                dynamic updatePriceArgs = new ExpandoObject();
                updatePriceArgs.nft_contract_id = NearPersistentManager.Instance.nftContactId;
                updatePriceArgs.token_id = tokenId;
                updatePriceArgs.ft_token_id = "near";
                updatePriceArgs.price = newSaleConditions["near"];

                UInt128 deposit = 1;
                    
                await marketContract.Change("update_price", updatePriceArgs, NearUtils.Gas, deposit);
            }
            else
            {
                dynamic nftApproveArgs = new ExpandoObject();
                nftApproveArgs.token_id = tokenId;
                nftApproveArgs.account_id = NearPersistentManager.Instance.MarketplaceContactId;

                dynamic msg = new ExpandoObject();
                msg.is_auction = isAuction;
                msg.sale_conditions = newSaleConditions;

                nftApproveArgs.msg = JsonConvert.SerializeObject(msg);

                UInt128 deposit = NearUtils.ParseNearAmount("1") / 100;

                ContractNear nftContract = await NearPersistentManager.Instance.GetNftContract();

                await nftContract.Change("nft_approve", nftApproveArgs, NearUtils.Gas, deposit);
            }
        }

        public static async void Offer(string tokenId, string offerToken, string price)
        {
            if (offerToken != "near")
            {
                throw new Exception("currently only accepting NEAR offers");
            }
            
            ContractNear marketContract = await NearPersistentManager.Instance.GetMarketplaceContract();

            dynamic offerArgs = new ExpandoObject();
            offerArgs.nft_contract_id = NearPersistentManager.Instance.nftContactId;
            offerArgs.token_id = tokenId;

            await marketContract.Change("offer", offerArgs, NearUtils.Gas, NearUtils.ParseNearAmount(price));
        }

        public static async void AcceptOffer(string tokenId)
        {
            ContractNear marketContract = await NearPersistentManager.Instance.GetMarketplaceContract();
            
            dynamic acceptOfferArgs = new ExpandoObject();
            acceptOfferArgs.nft_contract_id = NearPersistentManager.Instance.nftContactId;
            acceptOfferArgs.token_id = tokenId;
            acceptOfferArgs.ft_token_id = "near";

            await marketContract.Change("accept_offer", acceptOfferArgs, NearUtils.Gas);
        }

        public static async void RemoveSale(string tokenId)
        {
            ContractNear marketContract = await NearPersistentManager.Instance.GetMarketplaceContract();

            dynamic saleArgs = new ExpandoObject();
            saleArgs.nft_contract_token = NearPersistentManager.Instance.nftContactId + "||" + tokenId;

            dynamic sale = await marketContract.View("get_sale", saleArgs);
            
            if (sale.result.ToString() != "null")
            {
                dynamic removeSaleArgs = new ExpandoObject();

                removeSaleArgs.nft_contract_id = NearPersistentManager.Instance.nftContactId;
                removeSaleArgs.token_id = tokenId;
                
                UInt128 deposit = 1;

                await marketContract.Change("remove_sale", removeSaleArgs, NearUtils.Gas, deposit);
            }
        }
        
        /// <summary>
        /// Register an account and give a free pack
        /// </summary>
        public static async void RegisterAccount()
        {
            ContractNear nftContract = await NearPersistentManager.Instance.GetNftContract();
            var accountId = NearPersistentManager.Instance.GetAccountId();

            dynamic args = new ExpandoObject();
            args.receiver_id = accountId;
            
            var result = await nftContract.Change("nft_register_account", args, NearUtils.Gas);
            int x = 0;
        }

        public static async void BuyPack(string cost)
        {
            ContractNear nftContract = await NearPersistentManager.Instance.GetNftContract();
            var accountId = NearPersistentManager.Instance.GetAccountId();

            dynamic args = new ExpandoObject();
            args.receiver_id = accountId;
            UInt128 deposit = NearUtils.ParseNearAmount(cost);

            
            var result = await nftContract.Change("nft_buy_pack", args, NearUtils.Gas, deposit);
            int x = 0;
        }
    }
}