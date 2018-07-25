using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTAmHackCore.Models
{
    public class Room
    {

        public string Key { get; set; }
        public string Name { get; set; }
        public bool IsFree { get; set; }
        public int Timestamp { get; set; }
        public bool Activity { get; set; }
    }

    public class RoomViewModel
    {
        public List<Room> Rooms { get; set; }
    }
}
