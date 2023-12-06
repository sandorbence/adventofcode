namespace ScratchCards
{
    internal class Program
    {
        static int[] games;

        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            //int points = 0;
            int copies = 0;

            // Initialize all games with one copy
            games = new int[text.Length];
            for (int i = 0; i < games.Length; i++)
            {
                games[i] = 1;
            }

            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < games[i]; j++)
                {
                    copies += ParseLine(text[i]);
                }
                //points += ParseLine(text[i]);
            }

            //Console.WriteLine(points);
            Console.WriteLine(copies);
        }

        /// <summary>
        /// Parse one line and get the points from the game.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        static int ParseLine(string line, bool firstPart = false)
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

            int matches = GetCardMatches(winningNumbers, yourNumbers);

            if (matches != 0)
            {
                for (int i = 0; i < matches + 1; i++)
                {
                    games[cardIndex + i]++;
                }
            }

            if (firstPart)
                return matches > 0 ? (int)Math.Pow(2, matches - 1) : 0;

            Console.WriteLine($"At the end of game {cardIndex}.");
            return matches;
        }

        /// <summary>
        /// Get the matches from each card.
        /// </summary>
        /// <param name="winningNumbers"></param>
        /// <param name="yourNumbers"></param>
        /// <returns></returns>
        static int GetCardMatches(List<int> winningNumbers, List<int> yourNumbers)
        {
            int matches = 0;

            foreach (var number in yourNumbers)
            {
                if (winningNumbers.Contains(number))
                    matches++;
            }

            return matches;
        }


    }
}