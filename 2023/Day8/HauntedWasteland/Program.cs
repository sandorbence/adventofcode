using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HauntedWasteland
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            ParseInput(text);

            // First part
            long moves = Move();
            Console.WriteLine($"Total number of moves needed in the first part: {moves}");

            // Second Part
            moves = Move(false);
            Console.WriteLine($"Total number of moves needed in the second part: {moves}");
        }

        static Dictionary<Point, (string left, string right)> points = new Dictionary<Point, (string left, string right)>();
        static string instructions = "";

        /// <summary>
        /// Parse the input string.
        /// </summary>
        /// <param name="text"></param>
        static void ParseInput(string[] text)
        {
            int startIndex = 0;

            // For multiple lines of instructions
            for (int i = 0; i < text.Length; i++)
            {
                if (string.IsNullOrEmpty(text[i]))
                {
                    startIndex = i + 1;
                    break;
                }
                instructions += text[i];
            }

            for (int i = startIndex; i < text.Length; i++)
            {
                string[] sides = text[i].Split('=');
                string left = sides[1].Split(',')[0].Trim().Substring(1);
                string right = sides[1].Split(',')[1].Trim().Substring(0, 3);

                points[new Point(sides[0].Trim())] = (left, right);
            }

            // Add left and right points to every point after every point has been instantiated
            foreach (var point in points)
            {
                point.Key.Left = points.Where(item => item.Key.Name == point.Value.left).First().Key;
                point.Key.Right = points.Where(item => item.Key.Name == point.Value.right).First().Key;
            }
        }

        /// <summary>
        /// Move from one point to the next.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="instruction"></param>
        static void MoveOne(ref Point point, char instruction)
        {
            if (instruction == 'L')
                point = point.Left;
            else
                point = point.Right;
        }

        /// <summary>
        /// Move from point to point until we reach ZZZ.
        /// </summary>
        /// <param name="singleStart"></param>
        /// <returns></returns>
        static long Move(bool singleStart = true)
        {
            List<Point> startingPoints;

            if (singleStart)
                startingPoints = new List<Point> { points.Where(point => point.Key.Name == "AAA").First().Key };
            else
            {
                startingPoints = points.Where(point => point.Key.Name.EndsWith("A")).Select(item => item.Key).ToList();
            }

            long movesSingle = 0;
            List<long> allMoves = new List<long>();

            foreach (var point in startingPoints)
            {
                long moves = 0;
                var currentPoint = point;

                for (int i = 0; ; i++)
                {
                    // Start from the beginning if end of instructions is reached
                    if (i == instructions.Length)
                    {
                        i = 0;
                    }
                    moves++;
                    MoveOne(ref currentPoint, instructions[i]);

                    if (singleStart && currentPoint == points.Where(p => p.Key.Name == "ZZZ").First().Key)
                    {
                        break;
                    }
                    if (!singleStart && currentPoint.Name.EndsWith("Z"))
                    {
                        break;
                    }
                }

                allMoves.Add(moves);
                movesSingle = moves;
            }

            if (singleStart)
                return movesSingle;

            allMoves.ForEach(moves => Console.WriteLine(moves));

            List<int> divisors = new List<int>();

            foreach (var moves in allMoves)
            {
                divisors.AddRange(FindDivisors(moves));
            }

            long lcm = 1;

            for (int i = 0; i < divisors.Count - 1; i++)
            {
                lcm = FindLeastCommonMultiple(divisors[i], lcm);
            }

            return lcm;
        }

        /// <summary>
        /// Find the divisors of a number except for 1 and itself.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static List<int> FindDivisors(long number)
        {
            List<int> result = new List<int>();

            for (int i = 2; i < number - 1; i++)
            {
                if (number % i == 0)
                    result.Add(i);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static long FindLeastCommonMultiple(long a, long b)
        {
            long num1, num2;
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (int i = 1; i < num2; i++)
            {
                long mult = num1 * i;
                if (mult % num2 == 0)
                {
                    return mult;
                }
            }

            return num1 * num2;
        }
        /// <summary>
        /// Class representing one point.
        /// </summary>
        public class Point
        {
            public string Name;
            public Point Left { get; set; }
            public Point Right { get; set; }

            public Point(string name)
            {
                this.Name = name;
            }
        }
    }
}