using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace INachalnickTinyRestApi.Middlewares
{
    public class ReqResMiddleware
    {
        private readonly RequestDelegate _next;

        public ReqResMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "OPTIONS") { return; }
            var path = context.Request.Path.HasValue ? context.Request.Path.Value : "";
            if (string.IsNullOrEmpty(path) || path.Contains("/health") || path == "/")
            {
                await _next(context);
                return;
            }
            var showLogs = !path.Contains("/api/swagger/");
            if (!showLogs)
            {
                await _next(context);
                return;
            }
            try
            {
                //Logger.Log(new LogMessage(LogType.Debug, $"Request - {path} {context.Request.QueryString}"));
            }
            catch (Exception ex)
            {
                //Logger.Log(new LogMessage(LogType.Exception, $"ReqResMiddleware Error {ex.Message}"));
                //Logger.Log(new LogMessage(LogType.Exception, ex.Message, ex));
            }
            await _next(context);
            //Logger.Log(new LogMessage(LogType.Debug, $"Response - {context.Response.StatusCode} - {path}"));
        }
    }
}