using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kash.Elector.Data;
using Microsoft.AspNetCore.Mvc;

namespace Kash.Elector.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DummyController : ControllerBase
    {
        public IDistrictRepository DistrictRepository { get; set; }

        public DummyController(IDistrictRepository districtRepository)
        {
            DistrictRepository = districtRepository;
        }

        // POST api/values
        [HttpGet("{id}")]
        public async Task<ActionResult<Elector>> GetElector(string id)
        {
            var random = new Random();

            var elector = new Elector(id, DistrictRepository.Get(DummyDistrictRepository.election, random.Next(1, 4)));

            return await Task.FromResult(elector);
        }
    }
}
