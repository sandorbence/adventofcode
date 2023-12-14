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
        static List<string> columns = new List<string>();
        static (int row, int column) currentPoint;
        static int moves = 0;
        static Directions lastDirection;

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
            } while (rows[currentPoint.row][currentPoint.column] != 'S');

            Console.WriteLine($"First half: {(moves % 2 == 0 ? moves / 2 : (int)(moves / 2) + 1)}");
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