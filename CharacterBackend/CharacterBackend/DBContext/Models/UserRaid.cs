using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBackend.DBContext.Models
{
    public class UserRaid
    {
        public UserRaid()
        {

        }

        public Guid Id { get; set; }
        
        public long damage { get; set; }
        public string CallId { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid RaidId { get; set; }
        public Raid Raid { get; set; }
    }
}
