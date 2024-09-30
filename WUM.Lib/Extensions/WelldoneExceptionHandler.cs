using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WUM.Lib.Models.Common;
using WUM.Lib.Models.DB_Context;
using WUM.Lib.Utilities;

namespace WUM.Lib.Extensions
{
    public static class WelldoneExceptionHandler
    {
        public static void UseWelldoneExceptHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = async context =>
                {
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    string errorMsg = "";
                    if (exceptionFeature?.Error != null)
                    {
                        var logger = loggerFactory.CreateLogger("WelldoneExceptionHandler");
                        errorMsg = exceptionFeature.Error.Message;
                        logger.LogError(exceptionFeature.Error, errorMsg);
                        var path = exceptionFeature.Path;

                        using var dbContext = context.RequestServices.CreateScope().ServiceProvider.GetService<ManagementContext>();
                        await LogUtil.SaveErr(errorMsg, "WelldoneExceptionHandler", path, exceptionFeature.Error.ToString(), dbContext);
                    }

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 500;
                    var jsonStr = JsonSerializer.Serialize(new CommAPIModel<string>
                    {
                        Success = false,
                        Msg = "系統忙碌中，請稍後重試",
                        Data = errorMsg
                    });
                    await context.Response.WriteAsync(jsonStr);
                }
            });
        }
    }
}
