using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BridgeRepair {
    public class Node {
        public List<Node> Children = null;
        public int Value {get; set;}
        public string Operator {get;set;} = null;
    
        public long ApplyValue(long parentValue) {
            return this.Operator == "+" ? parentValue + this.Value : parentValue * this.Value;
        }

        public static Node BuildTree(Node node, List<int> nums) {
            if(nums.Count > 0) {
                Node plus = new Node{Value = nums[0], Operator = "+"};
                Node mult = new Node{Value = nums[0], Operator = "*"};
                nums.RemoveAt(0);
                node.Children = new List<Node>();
                node.Children.Add(BuildTree(plus, nums));
                node.Children.Add(BuildTree(mult, nums));
            }

            return node;
        }

        public void Print() {
            Console.WriteLine($"Value: {this.Value}, Operator: {this.Operator}");
            if (this.Children != null) {
                foreach(Node child in this.Children) {
                    child.Print();
                }
            }
        }

        public static bool FindSolutions(Node parent, long expected) {
            try {
                Calculate(parent, expected, 0);
            }
            catch (Exception ex) {
                if(ex.Message == "success"){ return true;}

                return false;
            }

            return false;
        }

        private static void Calculate(Node current, long expected, long result) {
            result = current.ApplyValue(result);
            Console.WriteLine(result);
            Console.WriteLine(current.Children.Count);

            if(result > expected) throw new Exception("fail");
            if(result == expected) throw new Exception("success");

            foreach(Node child in current.Children) {
                Calculate(child, expected, result);
            }
        }
    }
}