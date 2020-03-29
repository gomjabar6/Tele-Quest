using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBackend.DBContext.Models
{
    public class User
    {
        public User()
        {


        }
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }

        private long _expPoints { get; set; }
        public long ExpPoints
        {
            get
            {

                return _expPoints;
            }
            set
            {
                if (value < 100)
                {
                    _expPoints = 100;
                } else
                {
                    _expPoints = value;
                }
            }
        }

        public List<Quest> Quests { get; set; }
        public List<UserRaid> UserRaids { get; set; }
    }
}
