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

            foreach (var game in games)
            {
                bool possible = true;

                var index = game.Key;
                var reveals = game.Value;

                foreach (var reveal in reveals)
                {
                    if (!reveal.All(cubes => Decide(cubes.quantity, cubes.color)))
                        possible = false;
                }

                if (possible)
                    possibleIndexes.Add(index);
            }

            Console.WriteLine(possibleIndexes.Sum());
        }

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

        static bool Decide(int quantity, string color)
        {
            if (quantity > maxQuantities[color])
                return false;
            return true;
        }
    }
}