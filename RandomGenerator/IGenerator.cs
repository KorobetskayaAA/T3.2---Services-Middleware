using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomGenerator
{
    public interface IGenerator<T>
    {
        T Value { get; }
    }
}
