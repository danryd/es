 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES
{
    interface ISerializer
    {
        object Deserialize(byte[] @event);
        T Deserialize<T>(byte[] @event);
        byte[] Serialize(object @event);
    }
}
