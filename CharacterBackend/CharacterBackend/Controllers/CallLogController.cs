using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBackend.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CharacterBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallLogController : ControllerBase
    {

        public TeleQuestContext _context { get; }

        public CallLogController(TeleQuestContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetLastCalls()
        {
            var logs = await _context.CallLog.Take(100).OrderByDescending(l => l.Time).ToListAsync();
            return Ok(logs);
        }


    }
}