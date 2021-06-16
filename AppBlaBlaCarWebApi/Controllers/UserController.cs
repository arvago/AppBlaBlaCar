using AppBlaBlaCarWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppBlaBlaCarWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public ResponseModel Get()
        {
            return new UserModel().GetAll();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public ResponseModel Get(int id)
        {
            return new UserModel().Get(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public ResponseModel Post([FromBody] UserModel user)
        {
            return user.Insert();

        }

        // PUT api/<UserController>/5
        [HttpPut]
        public ResponseModel Put([FromBody] UserModel user)
        {
            return user.Update();

        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public ResponseModel Delete(int id)
        {
            return new UserModel().Delete(id);

        }
    }
}
