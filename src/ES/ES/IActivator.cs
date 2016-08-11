using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES
{
    interface IActivator
    {
        T Activate<T>();
        object Activate(Type type);
    }
}
