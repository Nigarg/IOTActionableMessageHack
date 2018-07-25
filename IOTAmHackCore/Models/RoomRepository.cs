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
            Add(new Room() { Name = "FocusRoom", IsFree = true});
            Add(new Room() { Name = "Garage", IsFree = true });
            Add(new Room() { Name = "ZoneRoom" });
        }

        public IEnumerable<Room> GetAll()
        {
            return _rooms.Values;
        }

        public void Add(Room item)
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            int key = _rdm.Next(_min, _max);
            item.Key = item.Key ?? key.ToString();
            if (!_rooms.Keys.Contains(item.Key))
            {
                item.Timestamp = (Int32) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                _rooms[item.Key] = item;
            }
            else if (item.Activity)
            {
                item.IsFree = false;
                _rooms[item.Key] = item;
            }
            else
            {
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                if (unixTimestamp - _rooms[item.Key].Timestamp > 60)
                {
                    item.IsFree = true;
                    _rooms[item.Key] = item;
                }
            }
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
