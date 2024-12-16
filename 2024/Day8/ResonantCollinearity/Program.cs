using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ResonantCollinearity
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<(int, int), char> map = ParseInput();
            Console.WriteLine($"First half: {FindAntinodes(map)}");
        }

        private static Dictionary<(int, int), char> ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] content = File.ReadAllLines(filePath);

            Dictionary<(int, int), char> result = new Dictionary<(int, int), char>();

            for (int i = 0; i < content.Length; i++)
            {
                for (int j = 0; j < content[i].Length; j++)
                {
                    result[(i, j)] = content[i][j];
                }
            }

            return result;
        }

        private static int FindAntinodes(Dictionary<(int, int), char> map)
        {
            HashSet<char> visitedAntennas = new HashSet<char>();

            using StreamWriter sw = new StreamWriter("asd.txt", true);
            List<(int, int)> allLocations = new List<(int, int)>();

            foreach (var area in map)
            {
                if (area.Value == '.') continue;
                if (visitedAntennas.Contains(area.Value)) continue;

                visitedAntennas.Add(area.Value);

                List<(int row, int col)> antennas = map.Where(x => x.Value == area.Value).Select(x => x.Key).ToList();

                foreach (var antenna in antennas)
                {
                    foreach (var otherAntenna in antennas.Where(x => x.row != antenna.row || x.col != antenna.col))
                    {
                        List<(int, int)> possibleLocations = GetAntinodeLocations(antenna, otherAntenna);

                        var antinodes = map.Where(x =>
                         possibleLocations.Contains(x.Key))
                            .ToList();

                        allLocations.AddRange(antinodes.Select(x => x.Key));
                    }
                }
            }

            allLocations.Distinct().OrderBy(x => x.Item1).ThenBy(x => x.Item2).ToList().ForEach(x => sw.WriteLine(x));

            return allLocations.Distinct().Count();
        }

        private static List<(int row, int col)> GetAntinodeLocations((int row, int col) first, (int row, int col) second)
        {
            List<(int row, int col)> result = new List<(int row, int col)>();

            result.Add((2 * first.row - second.row, 2 * first.col - second.col));
            result.Add((2 * second.row - first.row, 2 * second.col - first.col));

            return result;
        }
    }
}
