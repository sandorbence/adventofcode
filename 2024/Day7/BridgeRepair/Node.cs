using System;
using System.Collections.Generic;

namespace BridgeRepair
{
    public class Node
    {
        public List<Node> Children = new List<Node>();
        public int Value { get; set; }
        public string Operator { get; set; } = null;

        public long ApplyValue(long parentValue)
        {
            return this.Operator == "+" ? parentValue + this.Value : parentValue * this.Value;
        }

        public static Node BuildTree(Node node, List<int> nums)
        {
            if (nums.Count > 0)
            {
                Node plus = new Node { Value = nums[0], Operator = "+" };
                Node mult = new Node { Value = nums[0], Operator = "*" };
                node.Children.Add(BuildTree(plus, new List<int>(nums[1..])));
                node.Children.Add(BuildTree(mult, new List<int>(nums[1..])));
            }

            return node;
        }

        public static bool FindSolutions(Node parent, long expected)
        {
            try
            {
                Calculate(parent, expected, 0);
            }
            catch (Exception ex)
            {
                if (ex.Message == "success") return true;

                return false;
            }

            return false;
        }

        private static void Calculate(Node current, long expected, long result)
        {
            result = current.ApplyValue(result);

            if (result > expected) return;
            if (result == expected) throw new Exception("success");

            foreach (Node child in current.Children)
            {
                Calculate(child, expected, result);
            }
        }
    }
}