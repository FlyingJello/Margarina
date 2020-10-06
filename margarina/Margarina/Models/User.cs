using Microsoft.Azure.Cosmos.Table;

namespace Margarina.Models
{
    public class User : TableEntity
    {
        public User() { }

        public User(string username, string password)
        {
            PartitionKey = username;
            RowKey = username;
            Username = username;
            Password = password;
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
