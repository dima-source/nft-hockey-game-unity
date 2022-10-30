using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Query.Builder;
using Near;
using Newtonsoft.Json;
using UI.Profile.Models;
using UnityEngine;

namespace UI.Profile
{
    public class IndexerLogoLoader: ILogoLoader
    {
        private const string Url = "https://api.thegraph.com/subgraphs/name/nft-hockey/game";
        
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
        
        public async Task<TeamLogo> LoadLogo()
        {
            var query = new Query<TeamLogo>("teamLogo");
            query.AddArgument("id", NearPersistentManager.Instance.GetAccountId())
                .AddField(p => p.form_name)
                .AddField(p => p.pattern_name)
                .AddField(p => p.first_layer_color_number)
                .AddField(p => p.second_layer_color_number);
            var responseJson = await GetJsonQuery(query.Build().Trim());
            Debug.Log(responseJson);
            return JsonConvert.DeserializeObject<TeamLogo>(responseJson, new TeamLogoConverter());
        }
    }
}