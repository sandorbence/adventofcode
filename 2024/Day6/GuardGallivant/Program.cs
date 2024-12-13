using System;
using System.Collections.Generic;

namespace GuardGallivant
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = ParseInput();
            (int row, int col) position = FindGuard(input);
            List<(int row, int col)> obstacles = FindObstacles(input);

            HashSet<(int, int)> visitedAreas = FindVisitedAreas(position, obstacles, input.Length, input[0].Length);
            Console.WriteLine($"First half: {visitedAreas.Count}");
            Console.WriteLine($"Second half: {FindLoops(position, obstacles, input.Length, input[0].Length, visitedAreas)}");
        }
        private static string[] ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            return File.ReadAllLines(filePath);
        }

        private static HashSet<(int, int)> FindVisitedAreas((int row, int col) position, List<(int row, int col)> obstacles, int height, int width)
        {
            Direction direction = Direction.Up;
            HashSet<(int, int)> visitedAreas = new HashSet<(int, int)>();
            HashSet<(int, int, Direction)> turns = new HashSet<(int, int, Direction)>();

            while (position.row > -1 && position.row < height
                && position.col > -1 && position.col < width)
            {
                (int row, int col) destination = Step(position, direction);

                if (obstacles.Contains(destination))
                {
                    direction = direction switch
                    {
                        Direction.Up => Direction.Right,
                        Direction.Right => Direction.Down,
                        Direction.Down => Direction.Left,
                        Direction.Left => Direction.Up
                    };

                    if(turns.Contains((destination.row, destination.col, direction))) throw new Exception();

                    turns.Add((destination.row, destination.col, direction));
                }
                else
                {
                    position = destination;
                    visitedAreas.Add(position);
                }
            }

            return visitedAreas;
        }

        private static int FindLoops((int row, int col) position, List<(int row, int col)> obstacles, int height, int width, HashSet<(int row, int col)> visitedAreas) {
            int loops = 0;
            
            foreach((int row, int col) visitedArea in visitedAreas) {
                try
                {
                    obstacles.Add(visitedArea);
                    FindVisitedAreas(position, obstacles, height, width);
                }
                catch(Exception)
                {
                    loops++;
                }
                finally
                {
                    obstacles.Remove(visitedArea);
                }
            }

            return loops;
        }

        private static (int row, int col) FindGuard(string[] input) {
            return input.Select((x, i) => new { Row = x, Index = i })
                .Where(x => x.Row.Contains('^'))
                .Select(x => (x.Index, x.Row.IndexOf('^')))
                .First();
        }

        private static List<(int row, int col)> FindObstacles(string[] input) {
            return input.Select((x, i) =>
                new { Row = x, Index = i })
               .Select(x => x.Row
               .Select((y, i) => new { Area = y, Col = i })
               .Where(y => y.Area == '#')
               .Select(z => (x.Index, z.Col)))
               .SelectMany(x => x)
               .ToList();
        }

        private static (int, int) Step((int row, int col) position, Direction direction){
            return direction switch
                {
                    Direction.Up => (position.row - 1, position.col),
                    Direction.Right => (position.row, position.col + 1),
                    Direction.Down => (position.row + 1, position.col),
                    Direction.Left => (position.row, position.col - 1)
                };
        }

        private enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }
    }
}
