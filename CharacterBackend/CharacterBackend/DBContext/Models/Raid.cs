using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBackend.DBContext.Models
{
    public class Raid
    {
        public Raid()
        {

        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public long XpLevel { get; set; }
        public long XpReward { get; set; }
        public long XpPenalty { get; set; }
        public DateTime Date { get; set; }
        public bool Success { get; set; }

        public List<UserRaid> UserRaids { get; set; }
    }
}
