using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTAmHackCore.Models
{

    public class RoomRepository : IRoomRepository
    {
        private static ConcurrentDictionary<string, Room> _rooms =
            new ConcurrentDictionary<string, Room>();

        public RoomRepository()
        {
            Add(new Room() {Name = "Room1"});
        }

        public IEnumerable<Room> GetAll()
        {
            return _rooms.Values;
        }

        public void Add(Room item)
        {
            item.Key = Guid.NewGuid().ToString();
            _rooms[item.Key] = item;
        }

        public Room Find(string key)
        {
            Room item;
            _rooms.TryGetValue(key, out item);
            return item;
        }

        public Room Remove(string key)
        {
            Room item;
            _rooms.TryGetValue(key, out item);
            _rooms.TryRemove(key, out item);
            return item;
        }

        public void Update(Room item)
        {
            _rooms[item.Key] = item;
        }
    }

}
