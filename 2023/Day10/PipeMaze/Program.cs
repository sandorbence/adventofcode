using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PipeMaze
{
    internal class Program
    {
        static string tubeElements = "|-LJ7F";
        static List<string> rows = new List<string>();
        static (int row, int column) currentPoint;
        static int moves = 0;
        static Directions lastDirection;
        static List<Directions> loopDirections = new List<Directions>();
        static List<(int row, int column)> loopElements = new List<(int row, int column)>();

        static void Main(string[] args)
        {
            DirectoryInfo baseDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;

            string filePath = Path.Combine(baseDirectory.FullName, "input.txt");

            string[] text = File.ReadAllLines(filePath);

            DoMapping(text);
            currentPoint = FindStartPoint();

            // Loop through the map
            do
            {
                LookForNext();
                loopElements.Add(currentPoint);
            } while (rows[currentPoint.row][currentPoint.column] != 'S');

            Console.WriteLine($"First half: {(moves % 2 == 0 ? moves / 2 : (int)(moves / 2) + 1)}");

            Console.WriteLine(FindInnerTiles());
        }

        /// <summary>
        /// Add all text input into a map.
        /// </summary>
        /// <param name="text"></param>
        static void DoMapping(string[] text)
        {
            foreach (string line in text)
            {
                rows.Add(line);
            }
        }

        /// <summary>
        /// Find the starting point on the map.
        /// </summary>
        /// <returns></returns>
        static (int row, int column) FindStartPoint()
        {
            int row = 0, column = 0;

            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                {
                    if (rows[i][j] == 'S')
                    {
                        row = i;
                        column = j;
                        break;
                    }
                }
            }

            return (row, column);
        }

        /// <summary>
        /// Look for the next in all directions, then move one.
        /// </summary>
        static void LookForNext()
        {
            int row = currentPoint.row;
            int column = currentPoint.column;

            List<Directions> possibleDirections = new List<Directions>();

            if (column != 0)
            {
                possibleDirections.Add(Directions.Left);
            }
            if (column != rows.First().Length - 1)
            {
                possibleDirections.Add(Directions.Right);
            }
            if (row != 0)
            {
                possibleDirections.Add(Directions.Up);
            }
            if (row != rows.Count - 1)
            {
                possibleDirections.Add(Directions.Down);
            }

            foreach (var direction in possibleDirections)
            {
                if (CanMove(direction))
                {
                    Move(direction);
                    lastDirection = direction;
                    break;
                }
            }

            loopDirections.Add(lastDirection);
            moves++;
        }

        /// <summary>
        /// Decide if we can move in specific direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        static bool CanMove(Directions direction)
        {
            char current = rows[currentPoint.row][currentPoint.column];
            char next = direction switch
            {
                Directions.Left => rows[currentPoint.row][currentPoint.column - 1],
                Directions.Right => rows[currentPoint.row][currentPoint.column + 1],
                Directions.Up => rows[currentPoint.row - 1][currentPoint.column],
                Directions.Down => rows[currentPoint.row + 1][currentPoint.column],
                _ => ' '
            };

            bool canMove = direction switch
            {
                Directions.Left when (lastDirection != Directions.Right) && (current == 'S' || current == '-' || current == 'J' || current == '7')
                && (next == 'S' || next == '-' || next == 'L' || next == 'F') => true,
                Directions.Right when (lastDirection != Directions.Left) && (current == 'S' || current == '-' || current == 'L' || current == 'F')
                && (next == 'S' || next == '-' || next == 'J' || next == '7') => true,
                Directions.Up when (lastDirection != Directions.Down) && (current == 'S' || current == '|' || current == 'L' || current == 'J')
                && (next == 'S' || next == '|' || next == '7' || next == 'F') => true,
                Directions.Down when (lastDirection != Directions.Up) && (current == 'S' || current == '|' || current == '7' || current == 'F')
                && (next == 'S' || next == '|' || next == 'L' || next == 'J') => true,
                _ => false,
            };

            return canMove;
        }

        /// <summary>
        /// Move one step.
        /// </summary>
        /// <param name="direction"></param>
        /// <exception cref="Exception"></exception>
        static void Move(Directions direction)
        {
            currentPoint = direction switch
            {
                Directions.Left => (currentPoint.row, currentPoint.column - 1),
                Directions.Right => (currentPoint.row, currentPoint.column + 1),
                Directions.Up => (currentPoint.row - 1, currentPoint.column),
                Directions.Down => (currentPoint.row + 1, currentPoint.column),
                _ => throw new Exception("Cannot move.")
            };
        }

        static int FindInnerTiles()
        {
            Dictionary<(int row, int column), Directions> loopElementDirections = new Dictionary<(int row, int column), Directions>();

            for (int i = 0; i < loopElements.Count; i++)
            {
                loopElementDirections.Add(loopElements[i], loopDirections[i]);
            }

            int innerTiles = 0;

            for (int i = 0; i < rows.Count; i++)
            {
                string row = rows[i];

                for (int j = 0; j < row.Length; j++)
                {
                    if (row[j] != '.')
                    {
                        continue;
                    }

                    var throughElements = loopElements.Where(element => element.row == i && element.column > j).ToList();
                    string restRow = "";

                    for (int k = j + 1; k < row.Length; k++)
                    {
                        if (!throughElements.Contains((i, k)))
                        {
                            restRow += '.';
                        }
                        else
                        {
                            restRow += row[k];
                        }
                    }

                    if (throughElements.Any(element => rows[element.row][element.column] == 'S'))
                    {
                        throughElements = loopElements.Where(element => element.row == i && element.column < j).ToList();
                        restRow = "";

                        for (int k = 0; k < j; k++)
                        {
                            if (!throughElements.Contains((i, k)))
                            {
                                restRow += '.';
                            }
                            else
                            {
                                restRow += row[k];
                            }
                        }
                    }

                    MutateRow(restRow, out int sameEnd, out int sameLength, out int diffEnd, out int diffLength);

                    int throughNumber = throughElements.Count - sameLength - diffLength + diffEnd;

                    if (throughNumber % 2 == 1)
                    {
                        innerTiles++;

                    }
                }
            }

            return innerTiles;
        }

        static void MutateRow(string row, out int sameEnd, out int sameLength, out int diffEnd, out int diffLength)
        {
            sameEnd = 0;
            diffEnd = 0;
            sameLength = 0;
            diffLength = 0;

            string row2 = row;

            while (row2.IndexOf('L') != -1)
            {
                for (int i = row2.IndexOf('L'); i < row2.Length; i++)
                {
                    if (row2[i] == '7')
                    {
                        diffEnd++;
                        diffLength += i + 1 - row2.IndexOf('L');
                        row2 = row2.Substring(i + 1);
                        break;
                    }

                    if (row2[i] == 'J')
                    {
                        sameEnd++;
                        sameLength += i + 1 - row2.IndexOf('L');
                        row2 = row2.Substring(i + 1);
                        break;
                    }
                }
            }

            while (row.IndexOf('F') != -1)
            {
                for (int i = row.IndexOf('F'); i < row.Length; i++)
                {
                    if (row[i] == '7')
                    {
                        sameEnd++;
                        sameLength += i + 1 - row.IndexOf('F');
                        row = row.Substring(i + 1);
                        break;
                    }

                    if (row[i] == 'J')
                    {
                        diffEnd++;
                        diffLength += i + 1 - row.IndexOf('F');
                        row = row.Substring(i + 1);
                        break;
                    }
                }
            }

        }

        /// <summary>
        /// Direction towards which we can possibly move.
        /// </summary>
        enum Directions
        {
            Left,
            Right,
            Up,
            Down
        }
    }
}