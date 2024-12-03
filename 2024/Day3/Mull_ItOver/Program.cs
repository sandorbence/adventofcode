using System;
using System.IO;

namespace Mull_ItOver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = ParseInput();
        }

        private static string[] ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            return File.ReadAllLines(filePath);
        }

        private static void FindMultiplications(string[] input)
        {
            foreach (string line in input)
            {
                string lineRest = line;

                lineRest.IndexOf("mul(");
            }
        }
    }
}
