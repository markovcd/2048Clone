using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

enum Direction { Left = -1, Down = 0, Right = 1, Up = 2 }

class Program
{
    static Direction ParseDirection(string s)
    {
        return (Direction)Enum.Parse(typeof(Direction), s, true);
    }

    static void Transpose(bool left, int n, ref int[][] a)
    {
        var b = new int[n][];

        for (int i = 0; i < n; i++)
            b[i] = new int[n];

        for (int x = 0; x < n; x++)
            for (int y = 0; y < n; y++)
            {
                var x2 = left ? n - y - 1 : y;
                var y2 = left ? x : n - x - 1;

                b[x2][y2] = a[x][y];
            }

        a = b;
    }

    static void Transpose(int dir, int n, ref int[][] a, bool rev = false)
    {
        bool left = (dir < 0) ^ rev;
        dir = Math.Abs(dir);

        while (dir-- > 0)
            Transpose(left, n, ref a);
    }

    static void Move(Direction dir, int n, ref int[][] a)
    {
        Transpose((int)dir, n, ref a);

        for (int y = 0; y < n; y++)
        {
            for (int x = n - 2; x >= 0; x--)
            {
                if (a[x][y] == 0) continue;

                var i = x + 1;
                while (a[i][y] == 0)
                    if (++i == n) break;

                a[--i][y] = a[x][y];
                if (i != x) a[x][y] = 0;

                if (i == n - 1) continue;

                if (a[i + 1][y] == a[i][y])
                {
                    a[i + 1][y] *= 2;
                    a[i][y] = 0;
                }
            }
        }

        Transpose((int)dir, n, ref a, true);
    }

    static void Add(int n, int[][] a)
    {
        var r = new Random();
        var l = new List<Tuple<int, int>>();

        for (int x = 0; x < n; x++)
            for (int y = 0; y < n; y++)
                if (a[x][y] == 0) l.Add(Tuple.Create(x, y));

        var t = l[r.Next(0, l.Count)];
        a[t.Item1][t.Item2] = 2;

    }

    static void Print(int[][] a)
    {
      
        Console.WriteLine(a.Select(b => b.Select(i => i.ToString())
                                         .Aggregate(((s, s1) => s + "\t" + s1)))
                           .Aggregate(((s, s1) => s + "\n" + s1)));
      
    }

    static void Main(string[] args)
    {
        var n = 4;
        var a = new int[n][];
        for (int i = 0; i < n; i++)
            a[i] = new int[n];

        Add(n, a);

        while (true)
        {
            Console.Clear();
            Print(a);
            var key = Console.ReadKey().Key;

            Direction dir;

            if (key == ConsoleKey.LeftArrow) dir = Direction.Left;
            else if (key == ConsoleKey.DownArrow) dir = Direction.Down;
            else if (key == ConsoleKey.RightArrow) dir = Direction.Right;
            else if (key == ConsoleKey.UpArrow) dir = Direction.Up;
            else continue;

            Move(dir, n, ref a);
            Add(n, a);
        }

                

                

            
    }
}