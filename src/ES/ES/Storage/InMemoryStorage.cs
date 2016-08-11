using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Storage
{
    class InMemoryStorage:IStorage
    {

        private struct EventObject
        {
            public Guid entityId;
            public string eventType;
            public byte[] data;
        }
        
        private List<EventObject> events;
        public Task Save(Guid entityId, string eventType, byte[] data)
        {
            return Task.Run(() => events.Add(new EventObject {data=data,entityId = entityId,eventType = eventType}));
        }

        public Task<IEnumerable<byte[]>> Events(Guid entityId)
        {
            return Task.Run(() => events.Where(e => e.entityId == entityId).Select(e => e.data));
        }

        public void Initialize()
        {
           events= new List<EventObject>();
        }
    }
}
