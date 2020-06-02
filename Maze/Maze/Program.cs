using System;

namespace Maze
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] array = {
                           { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                           { 1, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1},
                           { 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
                           { 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1},
                           { 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1},
                           { 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1},
                           { 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                           { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
                           };
            Maze maze = new Maze(array);
            Point start = new Point(1, 1);
            Point end = new Point(5, 10);
            var parent = maze.FindPath(start, end, false);

            Console.WriteLine("Print path:");
            while (parent != null)
            {
                Console.WriteLine(parent.X + ", " + parent.Y);
                parent = parent.ParentPoint;
            }
            Console.ReadLine();
        }
    }
}