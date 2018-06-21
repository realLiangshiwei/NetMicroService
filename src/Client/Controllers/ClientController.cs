using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        public async Task<IActionResult> GetAllService()
        {
            using(var client = new ConsulClient(opt=>{
                opt.Address = new Uri("http://localhost:8500");
                opt.Datacenter = "dc1";
            })){
               var services=await client.Agent.Services();
              return Ok(services.Response.Select(x=>new 
              {
                  ServiceName = x.Value.Service,
                  Address = x.Value.Address,
                  Id = x.Value.ID,
                  Port = x.Value.Port}).ToList());
            }
        }
    }
}
