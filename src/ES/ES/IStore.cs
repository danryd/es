using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ES
{
    public interface IStore
    {
        Task Save(Guid entityId, object @event);
        Task<IEnumerable<T>> Events<T>(Guid entityId);
        Task<T> Entity<T>(Guid entityId);
    }
}
