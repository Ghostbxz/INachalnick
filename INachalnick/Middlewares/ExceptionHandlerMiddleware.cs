using ConsoleColorsExtension;
using Microsoft.AspNetCore.Http;
using Microsoft.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using INachalnickTinyRestApi.Models;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace INachalnickTinyRestApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ExceptionHandler(ex, context);
            }
        }

        private async Task ExceptionHandler(Exception ex, HttpContext context)
        {
            try
            {
                int statusCode = (int)HttpStatusCode.InternalServerError;
                object? extraInfo = null;
                statusCode = (int)GetStatusCodeByException(ex);

                if (ex is HttpOperationException operationException)
                {
                    extraInfo = operationException.Response.Content;
                    try
                    {
                        if (extraInfo is string str)
                        {
                            extraInfo = JToken.Parse(str);
                        }
                    }
                    catch { }
                }
                if (ex is not UnauthorizedAccessException)
                {
                    //Logger.Log(new LogMessage(LogType.Exception, $"Error {extraInfo ?? ""} {ex}", ex));
                }
                var errResponse = new
                {
                    Message = $"{ex?.Message}\n{ex?.InnerException?.Message}",
                    StatusCode = statusCode,
                    extraInfo,
                    ExtraData = (ex as ExceptionWithData)?.ExtraData,
                };
                var settings = new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                var jsonRes = JsonConvert.SerializeObject(errResponse, settings);
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(jsonRes);
                try
                {
                    context.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
                }
                catch { }
            }
            catch (Exception eex)
            {
                //Logger.Log(new LogMessage(LogType.Exception, $"ExceptionHandler Error {eex}", eex));
            }
        }

        private static HttpStatusCode GetStatusCodeByException(Exception ex)
        {
            if (ex is UnauthorizedAccessException) return HttpStatusCode.Unauthorized;
            if (ex is ArgumentException) return HttpStatusCode.NotFound;

            return HttpStatusCode.InternalServerError;
        }
    }
}
