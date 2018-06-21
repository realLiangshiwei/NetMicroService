using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;

namespace UserService.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static readonly List<UserDto> Users = new List<UserDto>();

        static UserController()
        {
            Users.Add(new UserDto{Name="令狐冲",Profile="剑在手,跟我走"});
            Users.Add(new UserDto{Name="jack",Profile="hello"});
        }
        
        // GET api/values
        [HttpGet]
        public ActionResult<List<UserDto>> Get()
        {
            return Users;
        }
       

        // GET api/values/5
        [HttpGet("{name}")]
        public ActionResult<UserDto> Get(string name)
        {
            return Users.FirstOrDefault(x=>x.Name == name);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] UserDto user)
        {
            Users.Add(user);
        }

        // DELETE api/values/5
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
            Users.RemoveAll(x=>x.Name==name);
        }
    }
}
