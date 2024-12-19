using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlutonianPebbles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<long, long> stones = ParseInput();
            long sumFirstHalf = 0;

            for (int i = 0; i < 75; i++)
            {
                stones = Blink(stones);

                if (i == 24) sumFirstHalf = stones.Values.Sum();
            }

            Console.WriteLine($"First half: {sumFirstHalf}");
            Console.WriteLine($"Second half: {stones.Values.Sum()}");
        }

        private static Dictionary<long, long> ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            return File.ReadAllText(filePath).Split(' ').ToDictionary(x => Convert.ToInt64(x), x => (long)1);
        }

        private static Dictionary<long, long> Blink(Dictionary<long, long> stones)
        {
            Dictionary<long, long> newStones = new Dictionary<long, long>(stones);

            foreach (var stone in stones)
            {
                newStones[stone.Key] -= stone.Value;

                List<long> splitStones = ApplyRules(stone.Key);

                foreach (long newStone in splitStones)
                {
                    if (newStones.ContainsKey(newStone))
                    {
                        newStones[newStone] += stone.Value;
                    }
                    else
                    {
                        newStones[newStone] = stone.Value;
                    }
                }

                if (newStones[stone.Key]==0) newStones.Remove(stone.Key);
            }

            return newStones;
        }

        private static List<long> ApplyRules(long stone)
        {
            string stoneChars = stone.ToString();

            if (stone == 0) return new List<long> { 1 };

            if (stoneChars.Length % 2 == 0) return new List<long>
            {
                Convert.ToInt64(stoneChars[..(stoneChars.Length / 2)]),
                Convert.ToInt64(stoneChars[(stoneChars.Length / 2)..])
            };

            return new List<long> { stone * 2024 };
        }
    }
}
