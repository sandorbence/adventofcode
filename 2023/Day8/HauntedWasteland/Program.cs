namespace HauntedWasteland
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);


            ParseInput(text);
        }

        static Dictionary<string, (string left, string right)> points = new Dictionary<string, (string left, string right)>();

        /// <summary>
        /// Parse the input string.
        /// </summary>
        /// <param name="text"></param>
        static void ParseInput(string[] text)
        {
            int startIndex = 0;
            string instructions = "";

            // For multiple lines of instructions
            for (int i = 0; i < text.Length; i++)
            {
                if (string.IsNullOrEmpty(text[i]))
                {
                    startIndex = i;
                    break;
                }
                instructions += text[i];
            }

            for (int i = startIndex; i < text.Length; i++)
            {
                string[] sides = text[i].Split('=');
                string left = sides[1].Split(',')[0].Substring(1);
                string right = sides[1].Split(',')[1].Trim().Substring(0, 3);

                points[sides[0].Trim()] = (left, right);
            }

        }

        /// <summary>
        /// Move from one point to the next.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="instruction"></param>
        static void MoveOne((string left, string right) point, string instruction)
        {

        }

        static void Move()
        {
            var startingPoint = points["AAA"];

            foreach (var point in points)
            {

            }
        }
    }
}