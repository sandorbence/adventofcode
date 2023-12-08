using System.Collections.Generic;

namespace SeedMapping
{
    internal class Program
    {
        static List<List<string>> maps = new List<List<string>>();

        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            GetMaps(text);

            List<long> seeds = maps[0][0].Split(':')[1].Trim().Split(' ').ToList().Select(seed => Convert.ToInt64(seed)).ToList();

            List<List<Dictionary<string, (long source, long destination)>>> allMappings = new List<List<Dictionary<string, (long source, long destination)>>>();

            for (int i = 1; i < maps.Count; i++)
            {
                allMappings.Add(DoMapping(maps[i]));
            }

            List<long> locations = new List<long>();

            foreach (long seed in seeds)
            {
                long source = seed;

                for (int i = 0; i < allMappings.Count; i++)
                {
                    source = FindDestination(allMappings[i], source);
                }

                locations.Add(source);
            }

            Console.WriteLine(locations.Min());
        }

        /// <summary>
        /// Get the maps from the input file.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="index"></param>
        static void GetMaps(string[] text, int index = 0)
        {
            List<string> map = new List<string>();

            for (int i = index == 0 ? index : index + 1; i < text.Length; i++)
            {
                if (!string.IsNullOrEmpty(text[i]))
                    map.Add(text[i]);
                else
                {
                    maps.Add(map);
                    GetMaps(text, i + 1);
                    return;
                }
            }
            maps.Add(map);
        }

        /// <summary>
        /// Do the mapping based on a map.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        static List<Dictionary<string, (long source, long destination)>> DoMapping(List<string> map)
        {
            List<Dictionary<string, (long source, long destination)>> mappings = new List<Dictionary<string, (long, long)>>();

            foreach (string mapItem in map)
            {
                Dictionary<string, (long source, long destination)> mapping = new Dictionary<string, (long, long)>();

                string[] items = mapItem.Split(' ');

                long destinationRangeStart = Convert.ToInt64(items[0]);
                long sourceRangeStart = Convert.ToInt64(items[1]);
                long rangeLength = Convert.ToInt64(items[2]);

                long destinationRangeEnd = destinationRangeStart + rangeLength;
                long sourceRangeEnd = sourceRangeStart + rangeLength;

                mapping["start"] = (sourceRangeStart, destinationRangeStart);
                mapping["end"] = (sourceRangeEnd, destinationRangeEnd);

                mappings.Add(mapping);
            }

            return mappings;
        }

        /// <summary>
        /// Find the destination of a given source.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        static long FindDestination(List<Dictionary<string, (long source, long destination)>> map, long source)
        {
            foreach (var mapping in map)
            {
                long sourceStart = mapping["start"].source;
                long sourceEnd = mapping["end"].source;
                long destStart = mapping["start"].destination;
                long destEnd = mapping["end"].destination;

                if (source >= sourceStart && source <= sourceEnd)
                {
                    return destStart + (source - sourceStart);
                }
            }

            return source;
        }
    }
}