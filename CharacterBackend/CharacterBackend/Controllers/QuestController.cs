using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBackend.DBContext;
using CharacterBackend.DBContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CharacterBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        public TeleQuestContext _context { get; }


        public QuestController(TeleQuestContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registers a quest for a user
        /// </summary>
        /// <param name="quest">The quest object. Only need to define Name (string), Success (boolean), and the EXPEarned (long, can be negative)</param>
        /// <param name="PhoneNumber">The Phone Number of the user</param>
        /// <returns></returns>
        [HttpPost("{PhoneNumber}")]
        public async Task<ActionResult> RecordQuest(Quest quest, string PhoneNumber)
        {
            var User = await _context.Users.Where(u => u.PhoneNumber == PhoneNumber).FirstOrDefaultAsync();

            if (User == null)
            {
                return NotFound("User Not Found");
            }

            quest.UserId = User.Id;
            quest.Date = DateTime.Now;

            _context.Quests.Add(quest);

            User.ExpPoints += quest.ExpEarned;

            await _context.SaveChangesAsync();

            quest.User = null;

            return Ok(quest);
        }

    }
}