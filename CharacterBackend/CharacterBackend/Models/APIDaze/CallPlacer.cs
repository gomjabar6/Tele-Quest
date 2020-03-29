using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBackend.Models.APIDaze
{
    public class CallPlacer
    {
        public CallPlacer(string number)
        {
            type = "number";
            callerid = "19702894651";
            origin = number;
            destination = number;
        }

        public string callerid { get; set; }
        public string origin { get; set; }
        public string type { get; set; }
        public string destination { get; set; }
    }
}
