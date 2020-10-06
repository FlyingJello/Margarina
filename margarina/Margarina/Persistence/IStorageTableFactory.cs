using Microsoft.Azure.Cosmos.Table;

namespace Margarina.Persistence
{
    interface IStorageTableFactory
    {
        CloudTableClient GetCloudTable();
    }
}
