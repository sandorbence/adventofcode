namespace GuardGallivant
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = ParseInput();
            Console.WriteLine($"First half: {FindVisitedAreas(input)}");
        }
        private static string[] ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            return File.ReadAllLines(filePath);
        }

        private static int FindVisitedAreas(string[] input)
        {
            (int row, int col) position = input.Select((x, i) => new { Row = x, Index = i })
                .Where(x => x.Row.Contains('^'))
                .Select(x => (x.Index, x.Row.IndexOf('^')))
                .First();

            List<(int row, int col)> obstacles = input.Select((x, i) =>
                new { Row = x, Index = i })
               .Select(x => x.Row
               .Select((y, i) => new { Area = y, Col = i })
               .Where(y => y.Area == '#')
               .Select(z => (x.Index, z.Col)))
               .SelectMany(x => x)
               .ToList();

            Direction direction = Direction.Up;
            HashSet<(int, int)> visitedAreas = new HashSet<(int, int)>();

            while (position.row > -1 && position.row < input.Length
                && position.col > -1 && position.col < input[0].Length)
            {
                (int row, int col) destination = direction switch
                {
                    Direction.Up => (position.row - 1, position.col),
                    Direction.Right => (position.row, position.col + 1),
                    Direction.Down => (position.row + 1, position.col),
                    Direction.Left => (position.row, position.col - 1)
                };

                if (obstacles.Contains(destination))
                {
                    direction = direction switch
                    {
                        Direction.Up => Direction.Right,
                        Direction.Right => Direction.Down,
                        Direction.Down => Direction.Left,
                        Direction.Left => Direction.Up
                    };
                }
                else
                {
                    position = destination;
                    visitedAreas.Add(position);
                }
            }

            return visitedAreas.Count;
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
