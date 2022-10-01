using NearClientUnity.Utilities;
using System.Threading.Tasks;

namespace NearClientUnity
{
    public class UrlAccountCreator : AccountCreator
    {
        private readonly Connection _connection;
        private readonly ConnectionInfo _helperConnection;

        public UrlAccountCreator(Connection connection, string helperUrl)
        {
            _connection = connection;
            _helperConnection = new ConnectionInfo
            {
                Url = helperUrl
            };
        }

        public override async Task CreateAccountAsync(string newAccountId, PublicKey publicKey)
        {
            await Web.FetchJsonAsync(_helperConnection, $@"newAccountId: {newAccountId},  newAccountPublicKey: {publicKey}");
        }
    }
}