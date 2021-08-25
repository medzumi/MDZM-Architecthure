using System;

namespace Architecture.Utilities
{
    public class IntGenerator
    {
        private const int maxWeight = Int32.MaxValue/2;
        private readonly int[] _biteWeights = new int[32];

        private Random _random;

        public IntGenerator(Random random)
        {
            _random = random;
        }

        public int Generate()
        {
            int multiplier = 1;
            int result = 0;
            var weight = maxWeight;
            for (int i = 0; i < 31; i++)
            {
                if (_random.Next(0, weight) > _biteWeights[i])
                {
                    _biteWeights[i]+=(_biteWeights[i]*2 +1);
                    result += multiplier;
                    weight -= multiplier*2;
                }
                
                multiplier *= 2;
            }

            return result;
        }

        public void Reserve(int value)
        {
            for (int i = 0; i < 32; i++)
            {
                var mod = value % 2;
                value = value >> 1;
                if (mod == 1)
                {
                    _biteWeights[i]++;
                }
            }
        }
    }
}