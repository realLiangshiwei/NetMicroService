using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) 
        {
           var conf= new ConfigurationBuilder().AddCommandLine(args).Build();
           var build = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(conf);

            if(conf["httpsport"]!=null)
            {
               build = build.UseKestrel(options=>{
                   options.Listen(IPAddress.Loopback, Convert.ToInt32(conf["httpsport"]),https=>{
                       https.UseHttps();
                   });
                    options.Listen(IPAddress.Loopback,Convert.ToInt32(conf["httpport"]));
                });
            }
              return  build.UseStartup<Startup>();
        }
    }
}
