using Microsoft.Data.SqlClient;
using System.Data;
using WUM.Lib.Interfaces;

namespace WUM.Lib.Configs
{
    public class DapperFactory : ISqlConnectionFactory
    {
        private readonly Func<string> getConnectionString;
        public DapperFactory(Func<string> getConnectionString) => this.getConnectionString = getConnectionString;
        public IDbConnection CreateConnection() => new SqlConnection(getConnectionString());
    }
}
