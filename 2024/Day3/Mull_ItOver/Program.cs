using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mull_ItOver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = ParseInput();
            Console.WriteLine($"First half: {FindMultiplications(input)}");
            Console.WriteLine($"Second half: {FindMultiplications(input, secondPart: true)}");
        }

        private static string[] ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            return File.ReadAllLines(filePath);
        }

        private static long FindMultiplications(string[] input, bool secondPart = false)
        {
            long sum = 0;

            Regex multiplyings = new Regex(@"mul\(\d{1,3},\d{1,3}\)");
            Regex numbers = new Regex(@"\d+");

            Regex enable = new Regex(@"do\(\)");
            Regex disable = new Regex(@"don't\(\)");

            bool recentCommand = true;

            foreach (string line in input)
            {
                Dictionary<int, bool> commands = enable.Matches(line).Union(
                    disable.Matches(line)).ToDictionary(x => x.Index, x => x.Value == "do()");

                commands[0] = recentCommand;

                foreach (Match match in multiplyings.Matches(line))
                {
                    int mult = 1;

                    foreach (Match num in numbers.Matches(match.Value))
                    {
                        mult *= Convert.ToInt32(num.Value);
                    }

                    if (!secondPart ||
                        (secondPart && commands[commands.Keys.Where(x => match.Index - x >= 0)
                        .OrderByDescending(x => x).First()]))
                    {
                        sum += mult;
                    }
                }

                recentCommand = commands.OrderByDescending(x => x.Key).First().Value;
            }

            return sum;
        }
    }
}
