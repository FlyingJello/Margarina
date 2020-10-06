using Microsoft.Azure.Cosmos.Table;

namespace Margarina.Persistence
{
    public interface IStorageTableFactory
    {
        CloudTableClient GetCloudTable();
    }
}
