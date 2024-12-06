namespace CeresSearch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = ParseInput();

            Console.WriteLine($"First half: {FindWords(input)}");
            Console.WriteLine($"Second half: {FindXMas(input)}");
        }
        private static string[] ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            return File.ReadAllLines(filePath);
        }

        private static int FindWords(string[] input)
        {
            int count = 0;

            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    if (input[i][j] != 'X') continue;

                    // Search left
                    if (j > 2)
                    {
                        if (IsXmas(input[i][j - 1], input[i][j - 2], input[i][j - 3])) count++;

                        // Search left-up
                        if (i > 2 && IsXmas(input[i - 1][j - 1], input[i - 2][j - 2], input[i - 3][j - 3])) count++;

                        // Search left-down
                        if (i < input.Length - 3 && IsXmas(input[i + 1][j - 1], input[i + 2][j - 2], input[i + 3][j - 3])) count++;
                    }
                    // Search right
                    if (j < input[i].Length - 3)
                    {
                        if (IsXmas(input[i][j + 1], input[i][j + 2], input[i][j + 3])) count++;

                        // Search right-up
                        if (i > 2 && IsXmas(input[i - 1][j + 1], input[i - 2][j + 2], input[i - 3][j + 3])) count++;

                        // Search right-down
                        if (i < input.Length - 3 && IsXmas(input[i + 1][j + 1], input[i + 2][j + 2], input[i + 3][j + 3])) count++;
                    }
                    // Search up
                    if (i > 2)
                    {
                        if (IsXmas(input[i - 1][j], input[i - 2][j], input[i - 3][j])) count++;
                    }
                    // Search down
                    if (i < input.Length - 3)
                    {
                        if (IsXmas(input[i + 1][j], input[i + 2][j], input[i + 3][j])) count++;
                    }
                }
            }

            return count;
        }

        private static int FindXMas(string[] input)
        {
            int count = 0;

            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    if (i < input.Length - 2 && j < input[i].Length - 2)
                    {
                        char middle = input[i + 1][j + 1];
                        char topLeft = input[i][j];
                        char bottomRight = input[i + 2][j + 2];
                        char topRight = input[i + 2][j];
                        char bottomLeft = input[i][j + 2];

                        if (IsX_Mas(middle, topLeft, bottomRight, topRight, bottomLeft)) count++;
                    }
                }
            }

            return count;
        }


        private static bool IsXmas(char first, char second, char third)
        {
            if (first != 'M' || second != 'A' || third != 'S') return false;

            return true;
        }

        private static bool IsX_Mas(char middle, char topLeft, char bottomRight, char topRight, char bottomLeft)
        {
            if (middle != 'A') return false;

            bool leftToRight = (topLeft == 'M' && bottomRight == 'S') || (topLeft == 'S' && bottomRight == 'M');
            bool rightToLeft = (topRight == 'M' && bottomLeft == 'S') || (topRight == 'S' && bottomLeft == 'M');

            if (leftToRight && rightToLeft) return true;

            return false;
        }
    }
}