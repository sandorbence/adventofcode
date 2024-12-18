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
            List<string> stones = ParseInput();

            for (int i = 0; i < 25; i++)
            {
                stones = Blink(stones);
            }

            Console.WriteLine($"First half: {stones.Count}");
        }

        private static List<string> ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            return File.ReadAllText(filePath).Split(' ')/*.Select(x => Convert.ToInt32(x))*/.ToList();
        }

        private static List<string> Blink(List<string> stones)
        {
            List<string> newStones = new List<string>();

            foreach (string stone in stones)
            {
                newStones.AddRange(ApplyRules(stone));
            }

            return newStones;
        }

        private static List<string> ApplyRules(string stone)
        {
            long num = Convert.ToInt64(stone);

            if (num == 0) return new List<string> { "1" };

            if (stone.Length % 2 == 0) return new List<string> { stone[..(stone.Length / 2)], Convert.ToInt32(stone[(stone.Length / 2)..]).ToString() };

            return new List<string> { (num * 2024).ToString() };
        }
    }
}
