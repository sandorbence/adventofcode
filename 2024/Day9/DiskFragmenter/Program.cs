using System;
using System.Collections.Generic;
using System.IO;

namespace DiskFragmenter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = ParseInput();
            List<string> files = DeCompress(input);
            List<string> files2 = new List<string>(files);
            List<string> compressedFiles = Compress(files);
            List<string> compressedFiles2 = CompressWhole(files2);

            Console.WriteLine($"First half: {CalculateChecksum(compressedFiles)}");
            Console.WriteLine($"Second half: {CalculateChecksum(compressedFiles2)}");
        }

        private static string ParseInput()
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            return File.ReadAllText(filePath);
        }

        private static List<string> DeCompress(string input)
        {
            List<string> files = new List<string>();
            int id = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < Convert.ToInt32(input[i].ToString()); j++)
                    {
                        files.Add(id.ToString());
                    }

                    id++;
                }
                else
                {
                    for (int j = 0; j < Convert.ToInt32(input[i].ToString()); j++)
                    {
                        files.Add(".");
                    }
                }
            }

            return files;
        }

        private static List<string> Compress(List<string> input)
        {
            int index = 0;
            int lastIndex = input.Count - 1;

            for (int i = 0; i < input.Count; i++)
            {
                if (input[i] != ".") continue;

                index = i;

                for (int j = lastIndex; j > index; j--)
                {
                    if (input[j] == ".") continue;

                    string lastFile = input[j];

                    input[i] = lastFile;
                    input[j] = ".";

                    lastIndex = j;
                    break;
                }
            }

            return input;
        }

        private static List<string> CompressWhole(List<string> input)
        {
            int index = 0;
            int lastIndex = input.Count - 1;

            for (int i = 0; i < input.Count; i++)
            {
                if (input[i] != ".") continue;

                index = i;

                while (i < input.Count && input[i] == ".")
                {
                    i++;
                }

                int freeSpace = i - index;

                for (int j = lastIndex; j > 0; j--)
                {
                    if (input[j] == ".") continue;

                    lastIndex = j;
                    string lastFilePiece = input[j];

                    while (j > 0 && input[j] == lastFilePiece)
                    {
                        j--;
                    }

                    int fileSize = lastIndex - j;

                    if (index > j)
                    {
                        i = 0;
                        lastIndex = j;
                        break;
                    }

                    if (freeSpace < fileSize)
                    {
                        break;
                    }

                    for (int k = index; k < index + fileSize; k++)
                    {
                        input[k] = lastFilePiece;
                    }

                    for (int k = lastIndex; k > j; k--)
                    {
                        input[k] = ".";
                    }

                    lastIndex = j;
                    i = 0;
                    break;
                }
            }

            return input;
        }

        private static long CalculateChecksum(List<string> input)
        {
            long result = 0;

            for (int i = 0; i < input.Count; i++)
            {
                if (input[i] == ".") continue;

                result += i * Convert.ToInt32(input[i]);
            }

            return result;
        }
    }
}
