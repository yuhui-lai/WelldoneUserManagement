using Azure;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WUM.Lib.Interfaces;
using WUM.Lib.Utilities;
using System.Text.Json;
using WUM.Lib.Models.LogModel;

namespace WUM.Lib.Filters
{
    public class ApiLogFilter : IActionFilter
    {
        private readonly IConfiguration config;
        private readonly ISqlConnectionFactory dapper;
        private WelldoneLog log;

        public ApiLogFilter(IConfiguration config, ISqlConnectionFactory dapper)
        {
            this.config = config;
            this.dapper = dapper;
            log = new WelldoneLog();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            /*var request = context.HttpContext.Request;
            var rIpAddr = context.HttpContext.Connection.RemoteIpAddress;
            log.ip = rIpAddr==null ? "" : rIpAddr.ToString();
            log.api = $"{Assembly.GetExecutingAssembly().GetName().Name}" + request.Path.ToUriComponent();
            log.request = JsonSerializer.Serialize(context.ActionArguments.Values);
            log.LogDate = TimeUtil.UtcNow();*/
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            /*if (context.Result is ObjectResult x)
            {
                log.response = JsonSerializer.Serialize(x.Value);
            }
            var except = context.Exception;
            log.Exception = except==null ? "" : except.Message;

            string logSql = " INSERT INTO welldone_api_log (ip, api, request, response, exception, create_datetime) " +
                " VALUES(@ip, @api, @request, @response, @exception, @create_datetime) ";

            var conn = dapper.CreateConnection();
            conn.Execute(logSql, log);*/
        }


    }
}
