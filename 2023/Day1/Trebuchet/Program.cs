namespace Trebuchet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            List<int> digits = new List<int>();

            foreach (string line in text)
            {
                digits.Add(FindAndConcatenateDigits(line));
            }

            Console.WriteLine(digits.Sum());
        }

        private static int FindAndConcatenateDigits(string line)
        {
            List<int> digits = new List<int>();

            foreach (char c in line)
            {
                if (int.TryParse(c.ToString(), out int result))
                    digits.Add(result);
            }

            return Convert.ToInt32($"{digits[0]}{digits[digits.Count - 1]}");
        }
    }
}