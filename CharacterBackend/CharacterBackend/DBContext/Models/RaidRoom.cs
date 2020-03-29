using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBackend.DBContext.Models
{
    public class RaidRoom
    {
        public RaidRoom()
        {

        }
        public Guid Id { get; set; }
        public string CallUUID { get; set; }
        public Guid UserId { get; set; }
    }
}
