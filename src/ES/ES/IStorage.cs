using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES
{
    interface IStorage
    {
        Task Save(Guid entityId, string eventType, byte[] data);
        Task<IEnumerable<byte[]>>  Events(Guid entityId);
        void Initialize();
    }
}
