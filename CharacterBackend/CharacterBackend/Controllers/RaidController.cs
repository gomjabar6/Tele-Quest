using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBackend.DBContext;
using CharacterBackend.DBContext.Models;
using CharacterBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CharacterBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RaidController : ControllerBase
    {
        private readonly APIDazeService _apiServie;
        private readonly TeleQuestContext _context;

        public RaidController(TeleQuestContext context, APIDazeService apiServie)
        {
            _context = context;
            _apiServie = apiServie;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRaid(Raid raid)
        {
            var dbRaid = new Raid();

            dbRaid.Id = Guid.NewGuid();
            dbRaid.Date = raid.Date;
            dbRaid.XpLevel = raid.XpLevel;
            dbRaid.XpPenalty = raid.XpPenalty;
            dbRaid.XpReward = raid.XpReward;
            dbRaid.Name = raid.Name;

            _context.Raid.Add(dbRaid);
            await _context.SaveChangesAsync();

            return Ok(dbRaid);
        }

        [HttpGet]
        public async Task<ActionResult> GetRaids()
        {
            var raids = await _context.Raid.Where(r => r.Date >= DateTime.UtcNow).OrderBy(r => r.Date).Take(10).ToListAsync();
            return Ok(raids);
        }

        [HttpGet]
        public async Task<ActionResult> BeginNextRaid()
        {
            await _apiServie.BeginRaid();
            return Ok();
        }



    }
}