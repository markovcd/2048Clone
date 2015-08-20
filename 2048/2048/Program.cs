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

    static void Process(Direction dir, int n, ref int[][] a)
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

    static void Main(string[] args)
    {
        using (StreamReader reader = File.OpenText(args[0]))
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine()
                                 .Split(';')
                                 .Select(s => s.Trim())
                                 .ToArray();

                var dir = ParseDirection(line[0]);
                var n = int.Parse(line[1]);

                var a = line[2].Split('|')
                               .Select(s => s.Split()
                                             .Select(int.Parse)
                                             .ToArray())
                               .ToArray();

                Process(dir, n, ref a);

                var r = a.Select(b => b.Select(i => i.ToString())
                                       .Aggregate((total, next) => total + " " + next))
                         .Aggregate((total, next) => total + "|" + next);

                Console.WriteLine(r);

            }
    }
}