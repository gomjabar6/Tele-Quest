using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBackend.DBContext.Models
{
    public class CallLog
    {
        public CallLog()
        {

        }

        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public string PhoneNumber { get; set; }
        public string UUID { get; set; }
    }
}
