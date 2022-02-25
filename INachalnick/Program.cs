using ConsoleColorsExtension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using INachalnickTinyRestApi.Controllers;
using INachalnickUtilities.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace INachalnick
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                MongoHelper.SetGlobalSettings();
                //Logger.Debug("Init Main");
                //Logger.Debug("Start Building Host");
                Console.WriteLine("Start Building Host");
                _ = new DefaultController { }; // to init Loaded static field
                var host =  Host.CreateDefaultBuilder(args)
                                .ConfigureWebHost(config =>
                                {
                                    config = config.SuppressStatusMessages(true)
                                    .UseUrls()
                                    .ConfigureLogging((hostingContext, logging) => logging.SetMinimumLevel(LogLevel.Error))
                                    .UseKestrel(options => options.Limits.MaxRequestBodySize = null) // dont limit body by code
                                    .ConfigureKestrel((context, options) => options.Listen(IPAddress.Any, 5000))
                                    .UseStartup<Startup>();
                                })
                                .Build();
                LogHostDetails();
                host.Run();
            }
            catch (Exception ex)
            {
                //Logger.Log(new LogMessage(LogType.Exception, "Stopped program because of exception ", ex));
                throw;
            }
        }

        public static void LogHostDetails()
        {
            string lan = GetLanIp();
            var logHostDetails = $@"
{"################ Server Is Running ##################".Green()}
    {"Local Address".Cyan()}
        http://localhost:5000
        http://localhost:5000/api/swagger/index.html?url=./swagger.json
    {"Lan Address".Cyan()}
        http://{lan}:5000
        http://{lan}:5000/api/swagger/index.html?url=./swagger.json
{"#####################################################".Green()}";
            //Logger.Debug(logHostDetails);
            Console.WriteLine(logHostDetails);
        }

        public static string GetLanIp()
        {
            var addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork && x.ToString() != "127.0.0.1").ToList();
            return addresses.Count > 0 ? addresses[0].ToString() : "localhost";
        }
    }
}
