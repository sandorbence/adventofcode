using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            // First half
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

            // Second half
            List<(long start, long end)> seedRanges = new List<(long start, long end)>();

            for (int i = 0; i < seeds.Count; i += 2)
            {
                seedRanges.Add((seeds[i], seeds[i] + seeds[i + 1] - 1));
            }

            List<(long start, long end)> destinations = new List<(long start, long end)>();

            foreach (var seedRange in seedRanges)
            {
                var sourceRange = new List<(long start, long end)> { seedRange };

                for (int i = 0; i < allMappings.Count; i++)
                {
                    // Find all intervals for a specific map type
                    var intervals = new List<(long start, long end)>();
                    var newSource = new List<(long start, long end)>();

                    foreach (var interval in sourceRange)
                    {
                        intervals.AddRange(FindIntervals(allMappings[i], interval.start, interval.end));
                    }

                    // For each interval find the destination
                    foreach (var interval in intervals)
                    {
                        long start = FindDestination(allMappings[i], interval.start);
                        long end = FindDestination(allMappings[i], interval.end);

                        newSource.Add((start, end));
                    }

                    sourceRange = newSource;
                }

                destinations.AddRange(sourceRange);
            }

            locations = destinations.SelectMany(interval => new long[] { interval.start, interval.end }).ToList();

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

                if (source >= sourceStart && source <= sourceEnd)
                {
                    return destStart + (source - sourceStart);
                }
            }

            return source;
        }

        /// <summary>
        /// Find all intervals for a range of items that need to be mapped.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        static List<(long start, long end)> FindIntervals(List<Dictionary<string, (long source, long destination)>> map, long start, long end)
        {
            List<(long start, long end)> intervals = new List<(long start, long end)>();

            long sourceStart = start;
            long sourceEnd = end;

            // Order mappings from lowest start to highest
            var orderedMaps = map.OrderBy(mapping => mapping["start"].source).ToList();

            foreach (var mapping in orderedMaps)
            {
                long mappingIntervalStart = mapping["start"].source;
                long mappingIntervalEnd = mapping["end"].source;

                if (sourceStart < mappingIntervalStart)
                {
                    // Source is out of mapping interval
                    if (sourceEnd < mappingIntervalStart)
                    {
                        intervals.Add((sourceStart, sourceEnd));
                        break;
                    }

                    intervals.Add((sourceStart, mappingIntervalStart - 1));

                    sourceStart = mappingIntervalStart;
                }

                if (sourceStart >= mappingIntervalStart)
                {
                    // Source is out of mapping interval
                    if (sourceStart > mappingIntervalEnd)
                        continue;

                    // Source is fully contained in one of the mappings
                    if (sourceEnd <= mappingIntervalEnd)
                    {
                        intervals.Add((sourceStart, sourceEnd));
                        break;
                    }

                    intervals.Add((sourceStart, mappingIntervalEnd));

                    if (sourceEnd != mappingIntervalEnd)
                        sourceStart = mappingIntervalEnd + 1;
                }
            }

            // Add last interval if it is greater than the last mapping interval
            if (sourceStart > orderedMaps.Last()["end"].source)
                intervals.Add((sourceStart, sourceEnd));

            return intervals;
        }
    }
}