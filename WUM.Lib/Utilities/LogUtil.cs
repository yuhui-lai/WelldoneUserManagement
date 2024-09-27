using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using WUM.Lib.Models.DB_Context;

namespace WUM.Lib.Utilities
{
    public static class LogUtil
    {
        public static async Task SaveLog<T>(T req, string operatorId, string logSource, string logLevel, ManagementContext dbContext)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            };

            WelldoneLog log = new WelldoneLog
            {
                LogDate = TimeUtil.UtcNow(),
                LogLevel = logLevel,
                LogMsg = JsonSerializer.Serialize(req, options),
                UserId = operatorId,
                LogSource = logSource
            };
            await dbContext.WelldoneLog.AddAsync(log);
            await dbContext.SaveChangesAsync();
        }
    }
}
