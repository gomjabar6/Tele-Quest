using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CharacterBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class APIDazeController : ControllerBase
    {

        public APIDazeService ApiServie { get; }

        public APIDazeController(APIDazeService apiServie)
        {
            ApiServie = apiServie;
        }

        [HttpGet]
        public async Task<ActionResult> GetCallList()
        {
            return Ok(await ApiServie.GetActiveCalls());
        }

        [HttpGet("{PhoneNumber}")]
        public async Task<ActionResult> JoinRaid(string PhoneNumber)
        {
            await ApiServie.PlaceRaidCall(PhoneNumber);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> EndConference()
        {
            await ApiServie.EndConference();

            return Ok();

        }


    }
}