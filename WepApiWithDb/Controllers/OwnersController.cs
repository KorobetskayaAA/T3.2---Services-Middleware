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
    public class OwnersController : MurcatControllerBase
    {
        private readonly OwnersService _ownersService;

        public OwnersController(OwnersService ownersService)
        {
            _ownersService = ownersService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Owner>> GetOwner(int id)
        {
            var result = await _ownersService.GetOwner(id);
            return MapResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOwner(int id, Owner owner)
        {
            var result = await _ownersService.UpdateOwner(id, owner);
            if (result.Status == MurcatResultStatus.Ok)
            {
                return NoContent();
            }
            return MapResult(result.Status);
        }
    }
}
