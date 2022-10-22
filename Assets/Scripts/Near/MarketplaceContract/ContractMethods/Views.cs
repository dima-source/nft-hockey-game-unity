using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Query.Builder;
using Near.MarketplaceContract.Parsers;
using Near.Models.Game;
using Near.Models.Game.Team;
using Near.Models.Game.TeamIds;
using Near.Models.Tokens;
using Near.Models.Tokens.Filters;
using Near.Models.Tokens.Players;
using Newtonsoft.Json;
using User = Near.Models.Game.User;


namespace Near.MarketplaceContract.ContractMethods
{
    public static class Views
    {
        private const string Url = "https://api.thegraph.com/subgraphs/name/nft-hockey/marketplace";

        private static async Task<string> GetJSONQuery(string json)
        {
            json = "{\"query\": \"{" + json.Replace("\"", "\\\"").Replace("\n", "\\n") + "}\"}";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response;
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await client.PostAsync(Url, content);
             
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<TeamIds> GetTeam(string accountId)
        {
            var query = $@"team(id: ""{accountId}"") {{id fives {{ id field_players {{id token_id position}} number ice_time_priority tactic }} goalies {{id number token_id}} goalie_substitutions {{ id number token_id}}}}";
            var responseJson = await GetJSONQuery(query.Trim());
            
            var teamIds = JsonConvert.DeserializeObject<TeamIds>(responseJson, new TeamIdsConverter());
            
            return teamIds;
        }

        public static async Task<List<Token>> GetTokens(PlayerFilter filter, Pagination pagination = null)
        {
            var query = new Query<Player>("tokens")
                .AddArguments(new {where = filter})
                .AddField(p => p.title)
                .AddField(p => p.nationality)
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
                .AddField(p => p.stats)
                .AddField(p => p.marketplace_data,
                    sq => sq
                        .AddField(p => p.id)
                        .AddField(p => p.price)
                        .AddField(p => p.isAuction)
                        .AddField(p => p.offers,
                            sqOffer => sqOffer 
                                .AddField(o => o.price)
                                .AddField(o => o.user, 
                                    sqUser => sqUser
                                        .AddField(u => u.id))));

            if (pagination != null)
            {
                query.AddArguments(pagination);
            }
            
            var responseJson = await GetJSONQuery(query.Build());
            
            var tokens = JsonConvert.DeserializeObject<List<Token>>(responseJson, new TokensConverter());
            
            return tokens ?? new List<Token>();
        }

        public static async Task<List<User>> GetUser(UserFilter filter)
        {
            var query = new Query<User>("users")
                .AddArguments(new { where = filter })
                .AddField(p => p.id)
                .AddField(p => p.tokens);

            var responseJson = await GetJSONQuery(query.Build());
            
            var getUsers = JsonConvert.DeserializeObject<List<User>>(responseJson, new UserConverter());
            
            return getUsers ?? new List<User>();
        }
    }
}