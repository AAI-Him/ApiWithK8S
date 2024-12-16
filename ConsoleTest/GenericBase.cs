using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    internal class GenericBase<T1, T2, T3>
        where T1 : class
        where T2 : class
        where T3 : class
    {
        public delegate bool GenericBaseEvent(T1 t1, T2 t2, T3 t3);
        public event GenericBaseEvent Event;
        public GenericBase(T1 t1, T2 t2, T3 t3) 
        {
        }
    }
}
