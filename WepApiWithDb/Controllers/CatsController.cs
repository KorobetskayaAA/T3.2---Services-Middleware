using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatsWepApiWithDb.BL;
using CatsWepApiWithDb.BL.Model;

namespace CatsWepApiWithDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : MurcatControllerBase
    {
        private readonly CatsService _catsService;

        public CatsController(CatsService catsService)
        {
            _catsService = catsService;
        }

        [HttpGet("{ownerId}")]
        public async Task<ActionResult<IEnumerable<ViewCat>>> GetCats(int ownerId)
        {
            var result = await _catsService.GetCats(ownerId);
            return MapResult(result);
        }

        [HttpGet("{ownerId}/{id}")]
        public async Task<ActionResult<ViewCat>> GetCat(int ownerId, string id)
        {
            var result = await _catsService.GetCat(ownerId, id);
            return MapResult(result);
        }

        [HttpPut("{ownerId}/{id}")]
        public async Task<IActionResult> PutCat(int ownerId, string id, PostedCat cat)
        {
            var result = await _catsService.UpdateCat(ownerId, id, cat);
            if (result.Status == MurcatResultStatus.Ok)
            {
                return NoContent();
            }
            return MapResult(result.Status);
        }

        [HttpPost("{ownerId}")]
        public async Task<ActionResult<ViewCat>> PostCat(int ownerId, PostedCat cat)
        {
            var result = await _catsService.CreateCat(ownerId, cat);
            if (result.Status == MurcatResultStatus.Ok)
            {
                return CreatedAtAction("GetCat",
                    new { ownerId = cat.OwnerId, id = cat.Id },
                    result.Value);
            }
            return MapResult(result.Status);
        }

        [HttpDelete("{ownerId}/{id}")]
        public async Task<ActionResult<ViewCat>> DeleteCat(int ownerId, string id)
        {
            var result = await _catsService.DeleteCat(ownerId, id);
            return MapResult(result);
        }
    }
}
