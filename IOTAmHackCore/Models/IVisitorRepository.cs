using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTAmHackCore.Models
{
    public interface IVisitorRepository
    {
        void Add(Visitor Visitor);
        IEnumerable<Visitor> GetAll();
        Visitor Find(string key);
        Visitor Remove(string key);
        void Update(Visitor Visitor);
    }

}
