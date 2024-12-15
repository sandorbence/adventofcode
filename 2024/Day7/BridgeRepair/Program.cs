using System;
using System.Collections.Generic;
using System.Linq;

namespace BridgeRepair
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] input = new string[] {ParseInput()[0]};
            long sum = 0;

            foreach(string line in input) {
                long expected = Convert.ToInt64(line.Split(':')[0]);
                List<int> nums = line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x)).ToList();

                Node parent = Node.BuildTree(new Node{Value = nums[0], Operator = "+"}, nums[1..]);
                //Console.WriteLine("-------------------");
                parent.Print();
                //Console.WriteLine("-------------------");
                if(Node.FindSolutions(parent, expected)) {
                    sum += expected;
                }
            }
            
            Console.WriteLine($"First half: {sum}");
        }

        private static string[] ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input_test.txt");

            return File.ReadAllLines(filePath);
        }
    }
}
