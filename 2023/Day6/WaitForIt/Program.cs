namespace WaitForIt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            List<(int time, int distance)> records = new List<(int, int)>();

            records.AddRange(GetTimesAndDistances(text));

            int numberOfWays = 1;

            foreach (var record in records)
            {
                int newRecord = 0;

                for (int i = 0; i < record.time; i++)
                {
                    if (GetDistanceTraveled(i, record.time) > record.distance)
                        newRecord++;
                }

                numberOfWays *= newRecord;
            }

            Console.WriteLine(numberOfWays);
        }

        static List<(int time, int distance)> GetTimesAndDistances(string[] text)
        {
            List<(int, int)> result = new List<(int, int)>();

            string[] timesString = text[0].Split(':')[1].Split(' ');
            string[] distancesString = text[1].Split(':')[1].Split(' ');

            List<int> times = new List<int>();
            List<int> distances = new List<int>();

            foreach (string s in timesString)
            {
                if (!string.IsNullOrEmpty(s))
                    times.Add(Convert.ToInt32(s));
            }

            foreach (string s in distancesString)
            {
                if (!string.IsNullOrEmpty(s))
                    distances.Add(Convert.ToInt32(s));
            }

            for (int i = 0; i < times.Count; i++)
            {
                result.Add((times[i], distances[i]));
            }

            return result;
        }

        /// <summary>
        /// Get distance traveled based on race time and holding time.
        /// </summary>
        /// <param name="holdingTime"></param>
        /// <param name="raceTime"></param>
        /// <returns></returns>
        static int GetDistanceTraveled(int holdingTime, int raceTime)
        {
            int timeLeft = raceTime - holdingTime;

            if (timeLeft <= 0) return 0;

            return (holdingTime * timeLeft);
        }
    }
}