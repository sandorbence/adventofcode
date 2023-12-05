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

        /// <summary>
        /// Find all numbers and stopre their index in the row and their length and value.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="specialCharacters"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Find all numbers that are next to a special character.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="numbers"></param>
        /// <param name="indexes"></param>
        /// <param name="indexesBefore"></param>
        /// <param name="indexesAfter"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Find all asterisks in a row.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        static List<int> FindAsterisks(string line)
        {
            List<int> indexes = new List<int>();

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '*')
                    indexes.Add(i);
            }

            return indexes;
        }

        /// <summary>
        /// Find all numbers that are gears and return their gear ratio in one row.
        /// </summary>
        /// <param name="asteriskIndexes"></param>
        /// <param name="rowLength"></param>
        /// <param name="numbersSameRow"></param>
        /// <param name="numbersBefore"></param>
        /// <param name="numbersAfter"></param>
        /// <returns></returns>
        static List<int> FindGears(List<int> asteriskIndexes, int rowLength, List<(int index, int length, int value)> numbersSameRow, List<(int index, int length, int value)> numbersBefore = null, List<(int index, int length, int value)> numbersAfter = null)
        {
            List<int> gearRatios = new List<int>();

            foreach (var index in asteriskIndexes)
            {
                List<int> gearValues = new List<int>();

                List<(int index, int value)> rightSides = numbersSameRow.Select(number => (number.index + number.length - 1, number.value)).ToList();

                if (numbersBefore != null)
                    rightSides.AddRange(numbersBefore.Select(number => (number.index + number.length - 1, number.value)).ToList());

                if (numbersAfter != null)
                    rightSides.AddRange(numbersAfter.Select(number => (number.index + number.length - 1, number.value)).ToList());

                // Check left
                if ((index > 0) && (rightSides.Select(number => number.index).Contains(index - 1)))
                {
                    gearValues.Add(rightSides.Where(number => number.index == index - 1).ToList().First().value);
                }

                List<(int index, int value)> leftSides = numbersSameRow.Select(number => (number.index, number.value)).ToList();

                if (numbersBefore != null)
                    leftSides.AddRange(numbersBefore.Select(number => (number.index, number.value)).ToList());

                if (numbersAfter != null)
                    leftSides.AddRange(numbersAfter.Select(number => (number.index, number.value)).ToList());

                // Check right
                if ((index < rowLength - 1) && (leftSides.Select(number => number.index).Contains(index + 1)))
                {
                    gearValues.Add(leftSides.Where(number => number.index == index + 1).ToList().First().value);
                }

                List<(int index, int value)> numberLengths = new List<(int index, int value)>();

                if (numbersBefore != null)
                {
                    foreach (var number in numbersBefore)
                    {
                        numberLengths.AddRange(ListIndexes(number.index, number.length, number.index));
                    }
                }

                if (numbersAfter != null)
                {
                    foreach (var number in numbersAfter)
                    {
                        numberLengths.AddRange(ListIndexes(number.index, number.length, number.index));
                    }
                }

                // Check top and bottom
                if (numberLengths.Select(number => number.index).Contains(index))
                {
                    gearValues.Add(numberLengths.Where(number => number.index == index).First().value);
                }

                // Check if numbers make a gear
                if (gearValues.Count == 2)
                {
                    gearRatios.Add(gearValues[0] * gearValues[1]);
                }
            }

            return gearRatios;
        }

        static List<(int index, int value)> ListIndexes(int index, int length, int value)
        {
            List<(int index, int value)> indexes = new List<(int, int)>();

            for (int i = index; i < index + length; i++)
            {
                indexes.Add((i, value));
            }

            return indexes;
        }
    }
}