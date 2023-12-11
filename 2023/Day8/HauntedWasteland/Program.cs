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
            int moves = Move();
            Console.WriteLine($"Total number of moves needed: {moves}");
        }

        static Dictionary<Point, (string left, string right)> points = new Dictionary<Point, (string left, string right)>();
        static string instructions = "";
        static Point currentPoint;

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
        static void MoveOne(Point point, char instruction)
        {
            if (instruction == 'L')
                currentPoint = point.Left;
            else
                currentPoint = point.Right;
        }

        /// <summary>
        /// Move from point to point until we reach ZZZ.
        /// </summary>
        /// <returns></returns>
        static int Move()
        {
            currentPoint = points.Where(point => point.Key.Name == "AAA").First().Key;
            int moves = 0;

            for (int i = 0; ; i++)
            {
                // Start from the beginning
                if (i >= instructions.Length)
                    i = 0;
                moves++;
                MoveOne(currentPoint, instructions[i]);
                if (currentPoint == points.Where(point => point.Key.Name == "ZZZ").First().Key)
                    break;
            }

            return moves;
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