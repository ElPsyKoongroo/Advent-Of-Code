namespace Advent_Of_Code;

public class EJ5 : EJ
{
    class Punto
    {
        public int x, y;

        public class Linea
        {
            public Punto a, b;
            public int sentido_hor, sentido_ver;
            public Linea(Punto a, Punto b)
            {
                this.a = a;
                this.b = b;
                sentido_hor = 0;
                sentido_ver = 0;


                if (a.x != b.x)
                    sentido_hor = a.x < b.x ? 1 : -1;
                if (a.y != b.y)
                    sentido_ver = a.y < b.y ? 1 : -1;
            }

            public int distancia()
            {
                if (a.x != b.x)
                    return Math.Abs(a.x - b.x);
                return Math.Abs(a.y - b.y);
            }
        }

        public Punto(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public void EJ()
    {
        var lineas = input.Select(x =>
        {
            var puntos = x.Split("->").Select(y =>
            {
                List<int>? coords = y.Split(",").Select(z => int.Parse(z.Trim())).ToList();

                return new Punto(coords[0], coords[1]);
            }).ToList();

            return new Punto.Linea(puntos[0], puntos[1]);
        }).ToList();

        int max_x = lineas.Max(x =>
        {
            return Math.Max(x.a.x, x.b.x);
        });
        int max_y = lineas.Max(x =>
        {
            return Math.Max(x.a.y, x.b.y);
        });

        int[][] matrix = new int[max_x + 1][];

        for (int i = 0; i < max_x+1; i++)
        {
            matrix[i] = Enumerable.Repeat(0, max_y + 1).ToArray();
        }

        lineas = lineas.Where(x =>
        {
            return x.a.x == x.b.x || x.a.y == x.b.y || Math.Abs(x.a.y - x.b.y) == Math.Abs(x.a.x - x.b.x);
        }).ToList();

        foreach (var linea in lineas)
        {
            int distancia = linea.distancia();
            for (int i = 0; Math.Abs(i) <= distancia; i++)
            {
                matrix[linea.a.x + linea.sentido_hor*i][linea.a.y + linea.sentido_ver*i] += 1;
            }
        }

        int total = matrix.Sum(x =>
        {
            return x.Sum(y => y > 1 ? 1 : 0);

        });

        /*for (int i = 0; i < max_x; i++)
        {
            for (int j = 0; j < max_y; j++)
            {
                if (matrix[i][j] > 1)
                    total++;
            }
        }*/


        Console.WriteLine(total);
    }

}