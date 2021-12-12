using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatsWepApiWithDb.BL;
using CatsWepApiWithDb.BL.Model;

namespace CatsWepApiWithDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogService _catalogService;

        public CatalogController(CatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewCat>>> GetCats()
        {
            return Ok(await _catalogService.GetCats());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ViewCat>> GetCat(string id)
        {
            var cat = await _catalogService.GetCat(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        [HttpPost("{id}/vote/{cuteness}")]
        public async Task<ActionResult<float>> PostVote(string id, float cuteness)
        {
            var cat = await _catalogService.Vote(id, cuteness);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat.Cuteness);
        }
    }
}
