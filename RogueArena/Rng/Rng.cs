namespace RogueArena.Rng
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Rng
    {
        private static readonly Lazy<Rng> Lazy = new Lazy<Rng>(() => new Rng());
        private Random _random;

        private Rng()
        {
        }

        private static Random _instance => Lazy.Value._random ?? (Lazy.Value._random = new Random());

        public static int Next(int maxValue)
        {
            return _instance.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return _instance.Next(minValue, maxValue);
        }

        public static T ChoiceFromDictionary<T>(Dictionary<T, int> choiceDictionary)
        {
            var choices = choiceDictionary.Keys.ToList();
            var chances = choiceDictionary.Values.ToList();

            return choices[RandomChoiceIndex(chances)];
        }

        private static int RandomChoiceIndex(List<int> chances)
        {
            var total = chances.Sum();
            var randomChance = Next(total);

            var runningSum = 0;
            var choice = 0;
            foreach (var chance in chances)
            {
                runningSum += chance;

                if (randomChance < runningSum)
                {
                    return choice;
                }

                choice++;
            }

            return chances.Count - 1;
        }
    }
}
