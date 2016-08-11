using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Activation
{
    class Activator:IActivator
    {
        public T Activate<T>()
        {
            return System.Activator.CreateInstance<T>();
        }

        public object Activate(Type type)
        {
            return System.Activator.CreateInstance(type);
        }
    }
}
