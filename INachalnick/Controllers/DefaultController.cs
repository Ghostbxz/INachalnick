using CSharpExtensions.OpenSource;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace INachalnickTinyRestApi.Controllers
{
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private static readonly DateTime Loaded = DateTime.UtcNow;
        [HttpGet("/api/healthz")]
        [HttpGet("/api/health")]
        [HttpGet("/api/alive")]
        public async Task<ContentResult> Alive()
        {
            var gitInfo = "";
            try
            {
                gitInfo = await System.IO.File.ReadAllTextAsync("./gitInfo.txt");
            }
            catch { }
            return Content($"uptime {(DateTime.UtcNow - Loaded).ToHumanReadableString()}, start time: {Loaded}\n\n{gitInfo}", "text/plain");
        }
    }
}