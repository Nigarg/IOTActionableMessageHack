using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTAmHackCore.Models
{
    public interface IRoomRepository
    {
        void Add(Room room);
        IEnumerable<Room> GetAll();
        Room Find(string key);
        Room Remove(string key);
        void Update(Room room);
    }

}
