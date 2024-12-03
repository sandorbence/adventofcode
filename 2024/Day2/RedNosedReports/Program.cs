using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RedNosedReports
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = ParseInput();

            Console.WriteLine($"First half: {CalculateSafeReportNumber(input)}");
            Console.WriteLine($"Second half: {CalculateSafeReportNumber(input, dampened: true)}");
        }

        private static string[] ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            return File.ReadAllLines(filePath);
        }

        private static int CalculateSafeReportNumber(string[] input, bool dampened = false)
        {
            int safeReports = 0;

            foreach (string line in input)
            {
                List<int> levels = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x)).ToList();

                bool increasing = levels[1] - levels[0] > 0;

                if (IsSafe(levels))
                {
                    safeReports++;
                    continue;
                }

                if (dampened)
                {
                    for (int i = 0; i < levels.Count; i++)
                    {
                        List<int> dampenedLevels = new List<int>(levels);
                        dampenedLevels.RemoveAt(i);

                        if (IsSafe(dampenedLevels))
                        {
                            safeReports++;
                            break;
                        }
                    }
                }
            }

            return safeReports;
        }

        private static bool IsSafe(List<int> levels)
        {
            bool increasing = levels[1] - levels[0] > 0;

            for (int i = 0; i < levels.Count - 1; i++)
            {
                int diff = levels[i + 1] - levels[i];

                if (Math.Abs(diff) > 3 || diff == 0)
                {
                    return false;
                }

                if ((increasing && diff < 0) || (!increasing && diff > 0))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
