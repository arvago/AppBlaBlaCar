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
    public class RideController : ControllerBase
    {
        // GET: api/<RideController>
        [HttpGet]
        public ResponseModel Get()
        {
            return new RideModel().GetAll();
        }

        // GET api/<RideController>/5
        [HttpGet("{id}")]
        public ResponseModel Get(int id)
        {
            return new RideModel().Get(id);
        }

        // POST api/<RideController>
        [HttpPost]
        public ResponseModel Post([FromBody] RideModel ride)
        {
            return ride.Insert();
        }

        // PUT api/<RideController>/5
        [HttpPut]
        public ResponseModel Put([FromBody] RideModel ride)
        {
            return ride.Update();
        }

        // DELETE api/<RideController>/5
        [HttpDelete("{id}")]
        public ResponseModel Delete(int id)
        {
            return new RideModel().Delete(id);
        }
    }
}
