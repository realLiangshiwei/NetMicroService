using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
    {
        [Route ("api/[controller]/[action]")]
        [ApiController]
        public class ClientController : ControllerBase
        {
            private readonly ConsulClient _consulClient;

            public ClientController(ConsulClient consulClient)
            {
                _consulClient = consulClient;
            }

            public async Task<IActionResult> GetAllService ()
            {
               
                    var services = await _consulClient.Agent.Services ();
                    return Ok (services.Response.Select (x => new
                    {
                        ServiceName = x.Value.Service,
                            Address = x.Value.Address,
                            Id = x.Value.ID,
                            Port = x.Value.Port
                    }).ToList ());
                
            }

            public async Task<string> GetUserService()
            {
                
                    var services = await _consulClient.Agent.Services ();

                    var service = services.Response.FirstOrDefault ().Value;

                    var httpclient = new HttpClient ();

                    return await httpclient.GetStringAsync ($"https://{service.Address}:{service.Port}/api/User");

                
            }
        }
    }