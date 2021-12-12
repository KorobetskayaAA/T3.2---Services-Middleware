using CatsWepApiWithDb.BL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatsWepApiWithDb.Controllers
{
    public class MurcatControllerBase : ControllerBase
    {
        protected ActionResult MapResult(MurcatResultStatus status)
        {
            switch (status)
            {
                case MurcatResultStatus.NotFound: return NotFound();
                case MurcatResultStatus.AlreadyExists: return Conflict();
                case MurcatResultStatus.DataSaveFailed: return StatusCode(500);
                case MurcatResultStatus.WrongInput: return BadRequest();
            }
            return Ok();
        }

        protected ActionResult<T> MapResult<T>(MurcatResult<T> result) where T: class
        {
            if (result.Status == MurcatResultStatus.Ok)
            {
                return Ok(result.Value);
            }
            return MapResult(result.Status);
        }
    }
}
