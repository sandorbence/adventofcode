namespace MirageMaintenance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            int result = 0;

            for (int i = 0; i < text.Length; i++)
            {
                result += Extrapolate(GetHistory(text[i]));
            }

            Console.WriteLine(result);
        }

        /// <summary>
        /// Get the history from one line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        static List<int> GetHistory(string line)
        {
            List<int> history = new List<int>();

            foreach (var number in line.Split(' '))
            {
                history.Add(Convert.ToInt32(number));
            }

            return history;
        }

        /// <summary>
        /// Extrapolate from one line of history.
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        static int Extrapolate(List<int> history)
        {
            List<List<int>> diffList = new List<List<int>>() { history };
            List<int> prevDiffs = history;

            while (true)
            {
                List<int> diffs = new List<int>();

                for (int i = 0; i < prevDiffs.Count - 1; i++)
                {
                    diffs.Add(prevDiffs[i + 1] - prevDiffs[i]);
                }

                diffList.Add(diffs);
                prevDiffs = diffs;

                if (diffs.All(diff => diff == 0))
                    break;
            }

            // Add the last diff again to start extrapolating
            int lastValue = diffList[diffList.Count - 2].Last();
            diffList[diffList.Count - 2].Add(lastValue);

            for (int i = diffList.Count - 2; i > 0; i--)
            {
                int newValue = diffList[i].Last() + diffList[i - 1].Last();
                diffList[i - 1].Add(newValue);
            }

            return diffList.First().Last();
        }
    }
}