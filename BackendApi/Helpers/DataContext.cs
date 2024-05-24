using System.Data;
using MySql.Data.MySqlClient;

namespace Grocery.Helpers{
    public class DataContext{
        public IDbConnection CreateConnection()
		{
			var connectionString = $"Server=localhost;Port=3306;Database=grocery;Uid=root;Pwd=123456;";
			return new MySqlConnection(connectionString);
		}
    }
}