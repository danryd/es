using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ES
{
    internal class Store : IStore
    {

        private readonly ISerializer serializer;
        private readonly IStorage storage;
        private readonly IActivator activator;

        public Store(ISerializer serializer, IStorage storage, IActivator activator)
        {

            this.serializer = serializer;
            this.storage = storage;
            this.activator = activator;
        }

        public async Task Save(Guid entityId, object @event)
        {
            var data = serializer.Serialize(@event);
            var eventType = @event.GetType().FullName;
            await storage.Save(entityId, eventType, data);
        }

        public async Task<IEnumerable<T>> Events<T>(Guid entityId)
        {

            var events = await storage.Events(entityId);

            var result = new List<T>();
            foreach (var data in events)
            {
                result.Add(serializer.Deserialize<T>(data));
            }
            return result;


        }

        public async Task<T> Entity<T>(Guid entityId)
        {

            var entity = activator.Activate<T>();
            var events = await storage.Events(entityId);


            foreach (var eventStream in events)
            {
                var @event = serializer.Deserialize(eventStream);
                Apply(entity, @event);
            }

            return entity;

        }

        private void Apply(object entity, object @event)
        {
            Type type = entity.GetType();
            var method =
                type
                    .GetMethods().Single(m => m.ReturnType == typeof(void) && m.GetParameters().Length == 1 &&
                            m.GetParameters()[0].ParameterType == @event.GetType());
            method.Invoke(entity, new[] { @event });
        }
    }
}