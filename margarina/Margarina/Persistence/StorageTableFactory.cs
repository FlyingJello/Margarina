using System;
using Margarina.Configuration;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace Margarina.Persistence
{
    public class StorageTableFactory : IStorageTableFactory
    {
        private readonly StorageConfig _config;

        public StorageTableFactory(IOptions<StorageConfig> config)
        {
            _config = config.Value;
        }

        public CloudTableClient GetCloudTable()
        {
            var client = CloudStorageAccount.Parse(_config.ConnectionString).CreateCloudTableClient();
            client.DefaultRequestOptions = new TableRequestOptions
            {
                PayloadFormat = TablePayloadFormat.Json,
                MaximumExecutionTime = TimeSpan.FromSeconds(10),
                LocationMode = LocationMode.PrimaryOnly
            };

            return client;
        }
    }
}