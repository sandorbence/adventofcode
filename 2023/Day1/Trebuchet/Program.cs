namespace Trebuchet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            List<int> digitsFirst = new List<int>();
            List<int> digitsSecond = new List<int>();

            foreach (string line in text)
            {
                digitsFirst.Add(FindAndConcatenateDigits(line, true));
                digitsSecond.Add(FindAndConcatenateDigits(line, false));
            }

            Console.WriteLine($"Sum for the first half: {digitsFirst.Sum()}");
            Console.WriteLine($"Sum for the second half: {digitsSecond.Sum()}");
        }

        private static int FindAndConcatenateDigits(string line, bool first)
        {
            if (first)
                return FindAndConcatenateDigitsFirst(line);
            else
                return FindAndConcatenateDigitsSecond(line);
        }

        private static int FindAndConcatenateDigitsFirst(string line)
        {
            List<int> digits = new List<int>();

            foreach (char c in line)
            {
                if (int.TryParse(c.ToString(), out int result))
                    digits.Add(result);
            }

            return Convert.ToInt32($"{digits[0]}{digits[digits.Count - 1]}");
        }

        private static int FindAndConcatenateDigitsSecond(string line)
        {
            string newLine = line;

            foreach (var item in NumbersToReplace)
            {
                newLine = newLine.Replace(item.Key, item.Value);
            }

            return FindAndConcatenateDigitsFirst(newLine);
        }

        private static Dictionary<string, string> NumbersToReplace = new Dictionary<string, string>()
        {
            {"one", "o1e" },
            {"two", "t2o" },
            {"three", "th3ee" },
            {"four", "fo4r" },
            {"five", "fi5e" },
            {"six", "s6x" },
            {"seven", "se7en" },
            {"eight", "ei8ht" },
            {"nine", "ni9e" }
        };
    }
}