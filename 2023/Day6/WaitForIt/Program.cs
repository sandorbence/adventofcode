namespace WaitForIt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            List<(long time, long distance)> records = new List<(long, long)>();

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

            Console.WriteLine($"First half: {numberOfWays}");

            // Second half
            (long time, long distance) = GetTimesAndDistances(text, true)[0];

            Console.WriteLine($"Second half: {FindRecords(time, distance)}");
        }

        /// <summary>
        /// Get the time and distance for each race.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        static List<(long time, long distance)> GetTimesAndDistances(string[] text, bool singleRace = false)
        {
            List<(long, long)> result = new List<(long, long)>();

            string[] timesString = text[0].Split(':')[1].Split(' ');
            string[] distancesString = text[1].Split(':')[1].Split(' ');

            List<int> times = new List<int>();
            List<int> distances = new List<int>();

            string time = "";
            string distance = "";

            foreach (string s in timesString)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    times.Add(Convert.ToInt32(s));
                    time += s;
                }
            }

            foreach (string s in distancesString)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    distances.Add(Convert.ToInt32(s));
                    distance += s;
                }
            }

            for (int i = 0; i < times.Count; i++)
            {
                result.Add((times[i], distances[i]));
            }

            return singleRace ? new List<(long time, long distance)>() { (Convert.ToInt64(time), Convert.ToInt64(distance)) } : result;
        }

        /// <summary>
        /// Get distance traveled based on race time and holding time.
        /// </summary>
        /// <param name="holdingTime"></param>
        /// <param name="raceTime"></param>
        /// <returns></returns>
        static long GetDistanceTraveled(long holdingTime, long raceTime)
        {
            long timeLeft = raceTime - holdingTime;

            if (timeLeft <= 0) return 0;

            return (holdingTime * timeLeft);
        }

        /// <summary>
        /// Find the solution to the equasion and get the difference between the two roots.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        static long FindRecords(long time, long distance)
        {
            var sqrt1 = (time - Math.Sqrt(Math.Pow(time, 2) - 4 * distance)) / 2;
            var sqrt2 = (time + Math.Sqrt(Math.Pow(time, 2) - 4 * distance)) / 2;

            return (long)(Math.Floor(sqrt2) - Math.Ceiling(sqrt1)) + 1;
        }
    }
}