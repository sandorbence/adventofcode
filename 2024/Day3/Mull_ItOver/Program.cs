using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Mull_ItOver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = ParseInput();
            Console.WriteLine($"First half: {FindMultiplications(input)}");
        }

        private static string[] ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            return File.ReadAllLines(filePath);
        }

        private static long FindMultiplications(string[] input)
        {
            long sum = 0;

            Regex multiplyings = new Regex(@"mul\(\d{1,3},\d{1,3}\)");
            Regex numbers = new Regex(@"\d+");

            foreach (string line in input)
            {
                foreach (Match match in multiplyings.Matches(line))
                {
                    int mult = 1;

                    foreach (Match num in numbers.Matches(match.Value))
                    {
                        mult *= Convert.ToInt32(num.Value);
                    }

                    sum += mult;
                }
            }

            return sum;
        }
    }
}
