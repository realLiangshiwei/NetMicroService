using System;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConsulController: ControllerBase
    {
        private readonly IApplicationLifetime _applicationLifetime;

        public ConsulController(IApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
        }
        public static string ServiceId = Guid.NewGuid().ToString();
        public static string ServiceName = "userService";
        public IActionResult Health()
        {
            return Ok("ok");
        }

        public async Task<IActionResult> Register()
        {
            using (var client = new ConsulClient (opt =>
            {
                opt.Address = new Uri ("http://localhost:8500");
                opt.Datacenter = "dc1"; 
            }))
            {
                 var address = HttpContext.Connection.LocalIpAddress.ToString();
                 if(address=="::1")
                 {
                     address = "localhost";
                 }
               await client.Agent.ServiceRegister(new AgentServiceRegistration
                {
                   
                    Name = ServiceName,
                        ID = ServiceId.ToString(),
                        Address = address,
                        Port = HttpContext.Connection.LocalPort,
                        Check = new AgentServiceCheck
                        {
                            DeregisterCriticalServiceAfter = TimeSpan.FromSeconds (5),
                                Interval = TimeSpan.FromSeconds (15),
                                HTTP = $"https://{address}:{HttpContext.Connection.LocalPort}/api/Consul/Health",
                                Timeout = TimeSpan.FromSeconds (5)
                        }
                });
            }

             _applicationLifetime.ApplicationStopped.Register (async () =>
            {
                using (var client = new ConsulClient (opt =>
                {
                    opt.Address = new Uri ("http://localhost:8500");
                    opt.Datacenter = "dc1";
                }))
                {
                   await client.Agent.ServiceDeregister(ServiceId);
                }

            });

            return Ok("ok");
        }
    }
}