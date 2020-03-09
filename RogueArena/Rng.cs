namespace RogueArena
{
    using System;

    public class Rng
    {
        private static readonly Lazy<Rng> Lazy = new Lazy<Rng>(() => new Rng());
        private Random _random;

        private Rng()
        {
        }

        private static Random Instance => Lazy.Value._random ?? (Lazy.Value._random = new Random());

        public static int Next(int maxValue)
        {
            return Instance.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return Instance.Next(minValue, maxValue);
        }
    }
}
