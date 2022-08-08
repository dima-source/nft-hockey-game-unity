using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Near.Models.Tokens;
using Near.Models.Tokens.Players.FieldPlayer;
using Newtonsoft.Json;


namespace Near.MarketplaceContract.ContractMethods
{
    public static class Views
    {
        public const string Url = "https://api.thegraph.com/subgraphs/name/nft-hockey/marketplace";
        
        public static async Task<string> GetJSONQuery(string json)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(Url, content);
             
                return await response.Content.ReadAsStringAsync(); 
            }
        }

        public static async Task<List<NFT>> GetUserNFTs()
        {
            // TODO: fixed request
            string accountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
            string json = "{\"query\": \"{tokens" +
                          "{title media reality stats nationality birthday number hand " + 
                          "player_role native_position player_type rarity issued_at tokenId owner ownerId perpetual_royalties " +
                          "marketplace_data{price token isAuction offers}}}\"}";
            string responseJson = await GetJSONQuery(json);
            var tokens = JsonConvert.DeserializeObject<List<NFT>>(responseJson, new TokensConverter());
            
            if (tokens == null)
            {
                return new List<NFT>();
            }

            return tokens;
        }

        public static async Task<List<NFT>> GetNFTsToBuy()
        {
            string accountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
            string json = "{\"query\": \"{marketplaceTokens(where: {token_:{ownerId_not: "+"\""+accountId+"\""+"}})" +
                        "{id price token {id media title extra issued_at perpetual_royalties tokenId owner { id } }" +
                        " isAuction offers { price user { id} }}}\"}";
            string responseJson = await GetJSONQuery(json);
            // TODO: parse response
            
            return new List<NFT>();
        }

        public static async Task<List<NFT>> GetUserNFTsOnSale()
        {
            throw new System.NotImplementedException();
        }
    }
}