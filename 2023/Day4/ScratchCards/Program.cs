using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            int points = 0;

            // Initialize all games with one copy
            games = new int[text.Length];
            for (int i = 0; i < games.Length; i++)
            {
                games[i] = 1;
            }

            for (int i = 0; i < text.Length; i++)
            {
                points += ParseLine(text[i]);
            }

            Console.WriteLine($"Point in the first half: {points}.");
            Console.WriteLine($"Copies in the second half: {games.ToList().Sum()}.");
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

            int matches = GetCardMatches(winningNumbers, yourNumbers);

            // Add copies
            if (matches != 0)
            {
                for (int i = 0; i < matches; i++)
                {
                    games[cardIndex + i] += games[cardIndex - 1];
                }
            }

            return matches > 0 ? (int)Math.Pow(2, matches - 1) : 0;
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