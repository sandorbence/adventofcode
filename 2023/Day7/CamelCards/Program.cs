using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CamelCards
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            List<(string hand, int bet)> hands = new List<(string, int)>();

            for (int i = 0; i < text.Length; i++)
            {
                hands.Add(GetHandsAndBets(text[i]));
            }

            List<(string hand, int bet)> rankedHandsFirstHalf = RankHands(hands);
            List<(string hand, int bet)> rankedHandsSecondHalf = RankHands(hands, true);

            int finalScoreFirstHalf = 0;
            int finalScoreSecondHalf = 0;

            for (int i = 0; i < rankedHandsFirstHalf.Count; i++)
            {
                finalScoreFirstHalf += (i + 1) * rankedHandsFirstHalf[i].bet;
            }
            for (int i = 0; i < rankedHandsSecondHalf.Count; i++)
            {
                finalScoreSecondHalf += (i + 1) * rankedHandsSecondHalf[i].bet;
            }

            Console.WriteLine($"First half: {finalScoreFirstHalf}");
            Console.WriteLine($"Second half: {finalScoreSecondHalf}");

        }

        /// <summary>
        /// Get the hand and bet from an input line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        static (string hand, int bet) GetHandsAndBets(string line)
        {
            return (line.Split(' ')[0], Convert.ToInt32(line.Split(' ')[1]));
        }

        /// <summary>
        /// Get the type of a hand.
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="secondHalf"></param>
        /// <returns></returns>
        static string GetType(string hand, bool secondHalf = false)
        {
            List<(char symbol, int count)> cards = new List<(char symbol, int count)>();

            foreach (char c in hand)
            {
                if (!cards.Select(card => card.symbol).ToList().Contains(c))
                {
                    cards.Add((c, 1));
                }
                else
                {
                    var cardsWithSymbol = cards.Where(card => card.symbol == c).First();
                    int index = cards.IndexOf(cardsWithSymbol);

                    (char sym, int count) = (cardsWithSymbol.symbol, cardsWithSymbol.count + 1);

                    cards[index] = (sym, count);
                }
            }

            int jokerCount = cards.Any(card => card.symbol == 'J') ? cards.Where(card => card.symbol == 'J').First().count : 0;

            int maxCount = cards.Select(card => card.count).Max();

            if (jokerCount > 0 && secondHalf)
            {
                var maxCountCards = cards.Where(card => card.count == maxCount).ToList();

                // Only add jokers to hand if the max count is not a joker
                if (maxCountCards.Count == 1 && maxCountCards.First().symbol == 'J')
                {
                    // Find the second most common card
                    if (maxCount != 5)
                        maxCount = cards.OrderByDescending(card => card.count).ToList()[1].count + jokerCount;
                }
                // If there are two pairs in hand or most common card is not joker
                else
                {
                    maxCount += jokerCount;
                }

                // So that the different type of cards will be one less
                cards.Remove(maxCountCards.First());
            }

            return maxCount switch
            {
                5 => "Five",
                4 => "Four",
                3 => cards.Count switch
                {
                    2 => "FullHouse",
                    _ => "Three"
                },
                2 => cards.Count switch
                {
                    3 => "TwoPairs",
                    _ => "OnePair"
                },
                1 => "HighCard",
                _ => "",
            };
        }

        /// <summary>
        /// Rank all hands by types and then by symbols.
        /// </summary>
        /// <param name="hands"></param>
        /// <returns></returns>
        static List<(string hand, int bet)> RankHands(List<(string hand, int bet)> hands, bool secondHalf = false)
        {
            List<(string hand, int bet, int typeRank)> sortedHands = new List<(string hand, int bet, int typeRank)>();

            sortedHands.AddRange(hands.Select(hand => (hand.hand, hand.bet, typeRanks[GetType(hand.hand, secondHalf)])));

            sortedHands = sortedHands.OrderBy(hand => hand.typeRank).ToList();

            List<(string hand, int bet)> orderedHands = new List<(string hand, int bet)>();

            if (secondHalf)
            {
                symbolRanks.Remove('J');
                symbolRanks['J'] = -1;
            }

            foreach (var type in sortedHands.GroupBy(hand => hand.typeRank))
            {
                List<(string hand, int bet)> handsOfType = type.Select(hand => (hand.hand, hand.bet)).ToList();

                orderedHands.AddRange(handsOfType.OrderBy(hand => hand.hand, new HandComparer()).ToList());
            }

            return orderedHands;
        }

        /// <summary>
        /// Compare hands by symbols.
        /// </summary>
        class HandComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] == y[i])
                        continue;
                    if (symbolRanks[x[i]] > symbolRanks[y[i]])
                        return 1;
                    else if (symbolRanks[x[i]] < symbolRanks[y[i]])
                        return -1;
                }

                return 0;
            }
        }

        /// <summary>
        /// Ranking for types.
        /// </summary>
        static Dictionary<string, int> typeRanks = new Dictionary<string, int>()
        {
            { "HighCard", 0 },
            { "OnePair", 1 },
            { "TwoPairs", 2 },
            { "Three", 3 },
            { "FullHouse", 4 },
            { "Four", 5 },
            { "Five", 6 }
        };

        /// <summary>
        /// Ranking for symbols.
        /// </summary>
        static Dictionary<char, int> symbolRanks = new Dictionary<char, int>()
        {
            { '2', 0 },
            { '3', 1 },
            { '4', 2 },
            { '5', 3 },
            { '6', 4 },
            { '7', 5 },
            { '8', 6 },
            { '9', 7 },
            { 'T', 8 },
            { 'J', 9 },
            { 'Q', 10 },
            { 'K', 11 },
            { 'A', 12 }
        };
    }
}