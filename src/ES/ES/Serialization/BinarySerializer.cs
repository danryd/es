using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ES.Serialization
{
    class BinarySerializer : ISerializer
    {
        readonly BinaryFormatter formatter = new BinaryFormatter();
        public object Deserialize(byte[] @event)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(@event, 0, @event.Length);
                ms.Position = 0;
                return formatter.Deserialize(ms);

            }
        }
        public T Deserialize<T>(byte[] @event)
        {
            return (T)Deserialize(@event);
        }
        public byte[] Serialize(object @event)
        {
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, @event);
                return ms.ToArray();
            }

        }
    }
}
