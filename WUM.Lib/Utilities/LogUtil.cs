using Dapper;
using System.Data;
using WUM.Lib.Models.LogModel;

namespace WUM.Lib.Utilities
{
    public static class LogUtil
    {
        public static string InsertLogSql()
        {
            return " INSERT INTO WelldoneLog ( LogDate, LogLevel, LogMsg, UserId, LogSource, Exception) " +
                " VALUES(@LogDate, @LogLevel, @LogMsg, @UserId, @LogSource, @Exception) ";
        }

        public static async Task Save(IDbConnection conn, WelldoneLog log)
        {
            await conn.ExecuteAsync(InsertLogSql(), log);
        }
    }
}
