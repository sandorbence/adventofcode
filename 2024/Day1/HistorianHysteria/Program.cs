using System;
using System.Collections.Generic;
using System.IO;

namespace HistorianHysteria
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = ParseInput();

            CalculateDiff(input);
            CalculateSimilarity(input);
        }

        private static string[] ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            return File.ReadAllLines(filePath);
        }

        private static void CalculateDiff(string[] input)
        {
            int diffSum = 0;

            List<int> leftNums = new List<int>();
            List<int> rightNums = new List<int>();

            foreach (string line in input)
            {
                int num1 = Convert.ToInt32(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);
                int num2 = Convert.ToInt32(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

                leftNums.Add(num1);
                rightNums.Add(num2);
            }

            leftNums.Sort();
            rightNums.Sort();

            for (int i = 0; i < leftNums.Count; i++)
            {
                diffSum += Math.Abs(leftNums[i] - rightNums[i]);
            }

            Console.WriteLine($"First half: {diffSum}");
        }

        private static void CalculateSimilarity(string[] input)
        {
            Dictionary<int, int> nums = new Dictionary<int, int>();

            int similarityScore = 0;

            foreach (string line in input)
            {
                int num1 = Convert.ToInt32(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);

                if (!nums.ContainsKey(num1))
                {
                    nums[num1] = 1;
                }
                else
                {
                    nums[num1]++;
                }
            }

            foreach (string line in input)
            {
                int num2 = Convert.ToInt32(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

                if (nums.ContainsKey(num2))
                {
                    similarityScore += nums[num2] * num2;
                }
            }

            Console.WriteLine($"Second half: {similarityScore}");
        }
    }
}
