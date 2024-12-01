using System;
using System.Collections.Generic;
using System.IO;

namespace HistorianHysteria
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CalculateDiff();
        }

        private static void CalculateDiff()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            int diffSum = 0;

            List<int> leftNums = new List<int>();
            List<int> rightNums = new List<int>();

            foreach (string line in text)
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

            Console.WriteLine(diffSum);
        }
    }
}
