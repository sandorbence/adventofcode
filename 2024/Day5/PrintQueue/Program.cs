namespace PrintQueue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = ParseInput();

            Console.WriteLine($"First half: {FindUpdates(input, right: true)}");
            Console.WriteLine($"Second half: {FindUpdates(input, right: false)}");
        }

        private static string ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            return File.ReadAllText(filePath);
        }

        private static int FindUpdates(string input, bool right = true)
        {
            string[] rules = input.Split("\r\n\r\n")[0].Split("\r\n");
            string[] updates = input.Split("\r\n\r\n")[1].Split("\r\n");

            Dictionary<int, List<int>> orders = new Dictionary<int, List<int>>();

            foreach (string rule in rules)
            {
                int[] ruleParts = rule.Split('|').Select(x => Convert.ToInt32(x)).ToArray();

                if (!orders.ContainsKey(ruleParts[0]))
                {
                    orders[ruleParts[0]] = new List<int> { ruleParts[1] };
                }
                else
                {
                    orders[ruleParts[0]].Add(ruleParts[1]);
                }
            }

            int rightMiddlePageSum = 0;
            int wrongMiddlePageSum = 0;

            foreach (string update in updates)
            {
                List<int> nums = update.Split(',').Select(x => Convert.ToInt32(x)).ToList();

                bool isGoodUpdate = true;

                for (int i = 0; i < nums.Count - 1; i++)
                {
                    if (nums[(i + 1)..].Any(x => orders[x].Contains(nums[i])))
                    {
                        isGoodUpdate = false;
                        break;
                    }
                }

                if (isGoodUpdate)
                {
                    rightMiddlePageSum += nums[(nums.Count - 1) / 2];
                }
                else
                {
                    nums.Sort((x, y) =>
                    {
                        if (orders[y].Contains(x)) return 1;

                        return -1;
                    });

                    wrongMiddlePageSum += nums[(nums.Count - 1) / 2];
                }
            }

            return right ? rightMiddlePageSum : wrongMiddlePageSum;
        }
    }
}
