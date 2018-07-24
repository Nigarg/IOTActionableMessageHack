using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTAmHackCore.Models
{

    public class VisitorRepository : IVisitorRepository
    {
        private static ConcurrentDictionary<string, Visitor> _Visitors =
            new ConcurrentDictionary<string, Visitor>();

        public VisitorRepository()
        {
            Add(new Visitor() { Name = "Visitor1" });
        }

        public IEnumerable<Visitor> GetAll()
        {
            return _Visitors.Values;
        }

        public void Add(Visitor item)
        {
            item.Key = Guid.NewGuid().ToString();
            _Visitors[item.Key] = item;
        }

        public Visitor Find(string key)
        {
            Visitor item;
            _Visitors.TryGetValue(key, out item);
            return item;
        }

        public Visitor Remove(string key)
        {
            Visitor item;
            _Visitors.TryGetValue(key, out item);
            _Visitors.TryRemove(key, out item);
            return item;
        }

        public void Update(Visitor item)
        {
            _Visitors[item.Key] = item;
        }
    }

}
