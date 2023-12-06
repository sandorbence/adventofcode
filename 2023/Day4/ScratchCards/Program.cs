namespace ScratchCards
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            int points = 0;

            foreach (string line in text)
            {
                points += ParseLine(line);
            }

            Console.WriteLine(points);
        }

        /// <summary>
        /// Parse one line and get the points from the game.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        static int ParseLine(string line)
        {
            string[] firstHalf = line.Split(':')[0].Split(' ');
            string cardIndexString = "";
            for (int i = 0; i < firstHalf.Length; i++)
            {
                if (int.TryParse(firstHalf[i].ToString(), out _))
                    cardIndexString += firstHalf[i];
            }
            int cardIndex = Convert.ToInt32(cardIndexString);
            string winningNumbersString = line.Split(":")[1].Split('|')[0];
            string yourNumbersString = line.Split(":")[1].Split('|')[1];

            List<int> winningNumbers = new List<int>();
            List<int> yourNumbers = new List<int>();

            foreach (var item in winningNumbersString.Split(' '))
            {
                if (string.IsNullOrEmpty(item)) continue;
                winningNumbers.Add(Convert.ToInt32(item));
            }

            foreach (var item in yourNumbersString.Split(' '))
            {
                if (string.IsNullOrEmpty(item)) continue;
                yourNumbers.Add(Convert.ToInt32(item));
            }

            return GetCardPoints(winningNumbers, yourNumbers);
        }

        /// <summary>
        /// Get the winning points from each card.
        /// </summary>
        /// <param name="winningNumbers"></param>
        /// <param name="yourNumbers"></param>
        /// <returns></returns>
        static int GetCardPoints(List<int> winningNumbers, List<int> yourNumbers)
        {
            int matches = 0;

            foreach (var number in yourNumbers)
            {
                if (winningNumbers.Contains(number))
                    matches++;
            }

            return matches > 0 ? (int)Math.Pow(2, matches - 1) : 0;
        }
    }
}