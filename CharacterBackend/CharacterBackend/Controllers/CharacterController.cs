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
    public class CharacterController : ControllerBase
    {
        public TeleQuestContext _context { get; }

        public CharacterController(TeleQuestContext Context)
        {
            this._context = Context;
        }

        /// <summary>
        /// Gets or creates a user details given the phone number
        /// </summary>
        /// <param name="PhoneNumber">Phone number of the user, no spaces please!</param>
        /// <returns></returns>
        [HttpGet("{PhoneNumber}")]
        public async Task<ActionResult> GetCreateUserDetails(string PhoneNumber)
        {
            var user = await _context.Users.Where(u => u.PhoneNumber == PhoneNumber).FirstOrDefaultAsync();

            if (user == null)
            {
                user = new DBContext.Models.User();
                _context.Add(user);
                user.PhoneNumber = PhoneNumber;
                await _context.SaveChangesAsync();
            }

            return Ok(user);
        }

        /// <summary>
        /// Gets top 10 Users, phone numbers are blanked for privacy
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetLeaderbaord()
        {
            var users = await _context.Users.OrderByDescending(u => u.ExpPoints).Take(10).ToListAsync();
            users.ForEach(u => u.PhoneNumber = "");

            return Ok(users);
        }


        /// <summary>
        /// Modifies a user's details given either the DB ID of the user or the phone number
        /// </summary>
        /// <param name="user">The user object, usually used to specify the username. Can't change Exp with this endpoint!</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetCreateUserDetails(User user)
        {

            var dbUser = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            if (dbUser == null)
            {
                dbUser = await _context.Users.Where(u => u.PhoneNumber == user.PhoneNumber).FirstOrDefaultAsync();
            }

            if (dbUser == null)
            {
                return NotFound("User not Found");
            }

            dbUser.PhoneNumber = user.PhoneNumber;
            dbUser.Username = user.Username;

            await _context.SaveChangesAsync();

            return Ok(user);
        }


    }
}