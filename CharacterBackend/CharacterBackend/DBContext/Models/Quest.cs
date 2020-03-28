using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBackend.DBContext.Models
{
    public class Quest
    {
        public Quest()
        {

        }

        public Guid Id { get; set; }
        public DateTime Date {get;set;}
        public string Name { get; set; }
        public bool Success { get; set; }
        public long ExpEarned { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
