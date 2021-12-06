using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomGenerator
{
    public class IntegerGenerator : IGenerator<int>
    {
        static readonly Random rnd = new Random();

        readonly int value = rnd.Next();
        public int Value => value;
    }
}
