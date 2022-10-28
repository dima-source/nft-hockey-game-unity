using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Query.Builder;
using Near;
using Near.MarketplaceContract;
using Near.Models.Game.TeamIds;
using Newtonsoft.Json;
using UI.Profile.Models;
using UnityEngine;

namespace UI.Profile
{
    public class IndexerRewardsRepository : IRewardsRepository
    {
        private const string Url = "https://api.thegraph.com/subgraphs/name/nft-hockey/rewards-indexer";
        
        private static async Task<string> GetJsonQuery(string json)
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
        
        public async Task<RewardsUser> GetUser()
        {
            var query = new Query<RewardsUser>("user");
            query.AddArgument("id", NearPersistentManager.Instance.GetAccountId())
                .AddField(p => p.id)
                .AddField(p => p.points)
                .AddField(p => p.wins)
                .AddField(p => p.wins_in_line)
                .AddField(p => p.games)
                .AddField(p => p.players_sold)
                .AddField(p => p.referrals_count)
                .AddField(p => p.friends_count)
                .AddField(p => p.already_set_team);
            var responseJson = await GetJsonQuery(query.Build().Trim());
            Debug.Log(responseJson);
            RewardsUser user = JsonConvert.DeserializeObject<RewardsUser>(responseJson, new RewardsUserConverter());
            return user;
        }
    }
}