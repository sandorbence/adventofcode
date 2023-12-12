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
            //int moves = Move();
            //Console.WriteLine($"Total number of moves needed: {moves}");

            // Second Part
            Move(false);
        }

        static Dictionary<Point, (string left, string right)> points = new Dictionary<Point, (string left, string right)>();
        static Dictionary<Point, Point> pointsAfterIteration = new Dictionary<Point, Point>();
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
            List<List<int>> divisors = new List<List<int>>();

            foreach (var point in points)
            {
                var currentPoint = point.Key;

                for (int i = 0; i < instructions.Length; i++)
                {
                    MoveOne(ref currentPoint, instructions[i]);
                }

                pointsAfterIteration[point.Key] = currentPoint;
            }

            foreach (var point in startingPoints)
            {
                long moves = 0;
                var currentPoint = point;

                while(true)
                {
                    moves += instructions.Length;
                    currentPoint = pointsAfterIteration[currentPoint];
                    //if (singleStart && currentPoint == points.Where(point => point.Key.Name == "ZZZ").First().Key)
                    //    break;
                    if (!singleStart && currentPoint.Name.EndsWith("Z"))
                        break;
                }

                allMoves.Add(moves);
                movesSingle = moves;
            }

            if (singleStart)
                return movesSingle;

            return 0;
        }

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