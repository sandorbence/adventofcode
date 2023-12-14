using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CubeConundrum
{
    internal class Program
    {
        private static Dictionary<string, int> maxQuantities = new Dictionary<string, int>()
        {
            {"red", 12 },
            {"green", 13},
            {"blue", 14}
        };

        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            Dictionary<int, List<List<(int quantity, string color)>>> games = new Dictionary<int, List<List<(int quantity, string color)>>>();

            List<int> possibleIndexes = new List<int>();

            foreach (var line in text)
            {
                var game = ParseLine(line);

                games[game.index] = game.reveals;
            }

            List<int> powers = new List<int>();

            foreach (var game in games)
            {
                bool possible = true;

                var index = game.Key;
                var reveals = game.Value;

                List<int> redQuantities = new List<int>();
                List<int> greenQuantities = new List<int>();
                List<int> blueQuantities = new List<int>();

                foreach (var reveal in reveals)
                {
                    if (!reveal.All(cubes => Decide(cubes.quantity, cubes.color)))
                        possible = false;
                    redQuantities.AddRange(reveal.Where(cubes => cubes.color == "red").Select(cube => cube.quantity).ToList());
                    greenQuantities.AddRange(reveal.Where(cubes => cubes.color == "green").Select(cube => cube.quantity).ToList());
                    blueQuantities.AddRange(reveal.Where(cubes => cubes.color == "blue").Select(cube => cube.quantity).ToList());
                }

                int redMax = redQuantities.Max();
                int greenMax = greenQuantities.Max();
                int blueMax = blueQuantities.Max();

                powers.Add(redMax * greenMax * blueMax);

                if (possible)
                    possibleIndexes.Add(index);
            }

            Console.WriteLine(possibleIndexes.Sum());
            Console.WriteLine(powers.Sum());
        }

        /// <summary>
        /// Parse the line of one game.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>Tuple containing the game's index and the list of reveals.</returns>
        static (int index, List<List<(int quantity, string color)>> reveals) ParseLine(string line)
        {
            int index = Convert.ToInt32(line.Split(':')[0].Split(' ')[1]);
            string[] reveals = line.Split(':')[1].Split(';');

            List<List<(int, string)>> revealedCubes = new List<List<(int, string)>>();

            foreach (string reveal in reveals)
            {
                string[] cubeLines = reveal.Split(',');

                List<(int, string)> cubes = new List<(int, string)>();

                foreach (string cube in cubeLines)
                {
                    string[] data = cube.Trim().Split(' ');
                    int quantity = Convert.ToInt32(data[0]);
                    string color = data[1];

                    cubes.Add((quantity, color));
                }

                revealedCubes.Add(cubes);
            }

            return (index, revealedCubes);
        }

        /// <summary>
        /// Decide if the game is possible based on one reveal.
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        static bool Decide(int quantity, string color)
        {
            if (quantity > maxQuantities[color])
                return false;
            return true;
        }
    }
}