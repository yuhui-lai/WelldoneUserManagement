using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WUM.Lib.Interfaces;

namespace WUM.Lib.Configs
{
    public static class DbSetting
    {
        public static void SetDapper(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //pass custom IDbConnectionFactory to repositories which is used to create connection inside using statement.
            //In this case repository itself can be singleton to reduce allocations on heap.
            //https://stackoverflow.com/questions/42937942/dapper-with-net-core-injected-sqlconnection-lifetime-scope
            //https://ithelp.ithome.com.tw/articles/10226575
            serviceCollection.AddSingleton<ISqlConnectionFactory>(serviceProvider =>
            {
                //指派連線字串
                return new DapperFactory(() => configuration.GetConnectionString("YuDB"));
            });
        }
    }
}
