﻿using NearClientUnity.Utilities;
using System.Threading.Tasks;
using UnityEngine;

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
            var data = $@"{{""newAccountId"": ""{newAccountId}"", ""newAccountPublicKey"": ""{publicKey.ToString().Replace("ed25519:", "")}""}}";
            await Web.FetchJsonAsync(_helperConnection, data);
        }
    }
}