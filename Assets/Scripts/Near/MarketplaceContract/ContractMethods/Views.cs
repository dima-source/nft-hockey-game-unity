using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Query.Builder;
using Near.Models.Tokens;
using Near.Models.Tokens.Filters;
using Near.Models.Tokens.Players;
using Near.Models.Tokens.Players.FieldPlayer;
using Newtonsoft.Json;
using UnityEngine;


namespace Near.MarketplaceContract.ContractMethods
{
    public static class Views
    {
        public const string Url = "https://api.thegraph.com/subgraphs/name/nft-hockey/marketplace";
        
        public static async Task<string> GetJSONQuery(string json)
        {
            json = "{\"query\": \"{" + json.Replace("\"", "\\\"") + "}\"}";
            
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

        public static async Task<List<Token>> GetUserNFTs()
        {
            // TODO: fixed request
            // string accountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
            string accountId = "parh.testnet";

            PlayerFiler filer = new PlayerFiler();
            filer.ownerId = "parh.testnet";
            filer.player_type = "FieldPlayer";
            filer.hand = "L";
            filer.rarity_in = new List<string> { "super rare", "exclusive"};

            IQuery<Player> query = new Query<Player>("tokens", new QueryOptions())
                .AddArguments(new { where = filer })
                .AddField(p => p.title)
                .AddField(p => p.player_type)
                .AddField(p => p.media)
                .AddField(p => p.rarity)
                .AddField(p => p.issued_at)
                .AddField(p => p.tokenId)
                .AddField(p => p.owner,
                    sq => sq
                        .AddField(p => p.id)
                        .AddField(p => p.tokens))
                .AddField(p => p.ownerId)
                .AddField(p => p.perpetual_royalties)
                .AddField(p => p.reality)
                .AddField(p => p.number)
                .AddField(p => p.hand)
                .AddField(p => p.player_role)
                .AddField(p => p.native_position)
                .AddField(p => p.birthday)
                .AddField(p => p.stats);

            string responseJson = await GetJSONQuery(query.Build());
            
            Debug.Log(responseJson);
            
            var tokens = JsonConvert.DeserializeObject<List<Token>>(responseJson, new TokensConverter());
            
            if (tokens == null)
            {
                return new List<Token>();
            }

            return tokens;
        }

        public static async Task<List<Token>> GetNFTsToBuy()
        {
            string accountId = NearPersistentManager.Instance.WalletAccount.GetAccountId();
            string json = "{\"query\": \"{marketplaceTokens(where: {token_:{ownerId_not: "+"\""+accountId+"\""+"}})" +
                        "{id price token {id media title extra issued_at perpetual_royalties tokenId owner { id } }" +
                        " isAuction offers { price user { id} }}}\"}";
            string responseJson = await GetJSONQuery(json);
            // TODO: parse response
            
            return new List<Token>();
        }

        public static async Task<List<Token>> GetUserNFTsOnSale()
        {
            throw new System.NotImplementedException();
        }
    }
}