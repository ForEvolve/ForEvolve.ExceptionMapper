using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionMapper
{
    public sealed class HandlerOrder
    {
        public const int DefaultOrder = 1;
        public const int FallbackOrder = 50;
        public const int SerializerOrder = 100;
    }
}
