using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HoofIt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<(int, int), int> map = ParseInput();
            Console.WriteLine($"Fist half: {FindTrails(map)}");
        }

        private static Dictionary<(int, int), int> ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] content = File.ReadAllLines(filePath);

            Dictionary<(int, int), int> result = new Dictionary<(int, int), int>();

            for (int i = 0; i < content.Length; i++)
            {
                for (int j = 0; j < content[i].Length; j++)
                {
                    result[(i, j)] = Convert.ToInt32(content[i][j].ToString());
                }
            }

            return result;
        }

        private static int FindTrails(Dictionary<(int, int), int> map)
        {
            List<(int row, int col)> trailheads = map.Where(x => x.Value == 0).Select(x => x.Key).ToList();

            int trails = 0;

            foreach ((int row, int col) trailhead in trailheads)
            {
                HashSet<(int, int)> tops = new HashSet<(int, int)>();
                Move(map, trailhead, tops);
                trails += tops.Count;
            }

            return trails;
        }

        private static void Move(Dictionary<(int row, int col), int> map, (int row, int col) position, HashSet<(int, int)> visitedTops)
        {
            int currentHeight = map.First(x => x.Key == position).Value;

            if (currentHeight == 9)
            {
                visitedTops.Add(position);
                return;
            }

            List<(int row, int col)> nextPositions = map.Where(x => x.Value == currentHeight + 1
            && IsNeighbouringPosition(position, x.Key)).Select(x => x.Key).ToList();

            foreach ((int row, int col) nextPosition in nextPositions)
            {
                Move(map, nextPosition, visitedTops);
            }
        }

        private static bool IsNeighbouringPosition((int row, int col) position, (int row, int col) field)
        {
            bool leftNeighbour = position.row == field.row && position.col == field.col + 1;
            bool rightNeighbour = position.row == field.row && position.col == field.col - 1;
            bool topNeighbour = position.row == field.row + 1 && position.col == field.col;
            bool bottomNeighbour = position.row == field.row - 1 && position.col == field.col;

            return leftNeighbour || rightNeighbour || topNeighbour || bottomNeighbour;
        }
    }
}
