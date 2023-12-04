using System;
using System.Linq;

namespace GearRatios
{
    internal class Program
    {

        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            int partNumberSum = 0;

            var firstRowNumbers = FindNumbers(text[0], out List<int> specialCharactersFirstRow);
            FindNumbers(text[1], out List<int> specialCharactersSecondRow);
            partNumberSum += FindPartNumbers(text[0], firstRowNumbers, specialCharactersFirstRow, indexesBefore: null, specialCharactersSecondRow);

            for (int i = 1; i < text.Length - 1; i++)
            {
                var rowNumbers = FindNumbers(text[i], out List<int> specialCharacters);
                FindNumbers(text[i - 1], out List<int> specialCharactersBefore);
                FindNumbers(text[i + 1], out List<int> specialCharactersAfter);
                partNumberSum += FindPartNumbers(text[i], rowNumbers, specialCharacters, specialCharactersBefore, specialCharactersAfter);
            }

            var lastRowNumbers = FindNumbers(text[text.Length - 1], out List<int> specialCharactersLastRow);
            FindNumbers(text[text.Length - 2], out List<int> specialCharactersLastButOneRow);
            partNumberSum += FindPartNumbers(text[text.Length - 1], lastRowNumbers, specialCharactersLastRow, specialCharactersLastButOneRow, indexesAfter: null);

            Console.WriteLine(partNumberSum);
        }

        static List<(int index, int length, int value)> FindNumbers(string line, out List<int> specialCharacters)
        {
            List<(int index, int length, int value)> numbers = new List<(int, int, int)>();
            specialCharacters = new List<int>();

            for (int i = 0; i < line.Length;)
            {
                if (int.TryParse(line[i].ToString(), out _))
                {
                    string number = "";

                    for (int j = i; j < line.Length; j++)
                    {
                        if (int.TryParse(line[j].ToString(), out _))
                        {
                            number += line[j];
                        }
                        else
                            break;
                    }

                    numbers.Add((i, number.Length, Convert.ToInt32(number)));
                    i += number.Length;
                }
                else if (line[i] != '.')
                {
                    specialCharacters.Add(i);
                    i++;
                }
                else
                {
                    i++;
                }
            }

            return numbers;
        }

        static int FindPartNumbers(string line, List<(int index, int length, int value)> numbers, List<int> indexes, List<int> indexesBefore = null, List<int> indexesAfter = null)
        {
            int rowSum = 0;

            foreach (var number in numbers)
            {
                bool isPartNumber = false;

                // Check left
                if (number.index > 0)
                {
                    // Check same row
                    if (indexes.Contains(number.index - 1))
                        isPartNumber = true;

                    // Check row above
                    if ((indexesBefore != null) && (indexesBefore.Contains(number.index - 1)))
                        isPartNumber = true;

                    // Check row below
                    if ((indexesAfter != null) && (indexesAfter.Contains(number.index - 1)))
                        isPartNumber = true;
                }

                int rightSide = number.index + number.length - 1;

                // Check right
                if (rightSide < line.Length - 1)
                {
                    // Check same row
                    if (indexes.Contains(rightSide + 1))
                        isPartNumber = true;

                    // Check row above
                    if ((indexesBefore != null) && (indexesBefore.Contains(rightSide + 1)))
                        isPartNumber = true;

                    // Check row below
                    if ((indexesAfter != null) && (indexesAfter.Contains(rightSide + 1)))
                        isPartNumber = true;
                }

                // Check top and bottom
                for (int i = number.index; i < rightSide + 1; i++)
                {
                    if ((indexesBefore != null) && (indexesBefore.Contains(i)))
                        isPartNumber = true;

                    if ((indexesAfter != null) && (indexesAfter.Contains(i)))
                        isPartNumber = true;
                }

                if (isPartNumber)
                {
                    rowSum += number.value;
                }
            }

            return rowSum;
        }

    }
}