namespace Advent_Of_Code;

class Ej4
{
    const string path = @"X:\Visual Studio\Pruebas_de_todo\css\rsc\EJ.txt";
    string input2 = File.ReadAllText(path);
    public void EJ4()
    {
        var info_inicial = input2.Split("\r\n\r\n");

        var nums = info_inicial[0].Split(",").Select(x => int.Parse(x)).ToList();

        var tablas = info_inicial[1..].Select(x =>
        {
            int[][] tabla = new int[5][];

            for (int i = 0; i < 5; i++)
                tabla[i] = new int[5];

            var tabla_string = x
                                .Replace("\r\n", " ")
                                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => int.Parse(x.Trim()))
                                .ToList();

            for (int i = 0; i < 25; i++)
            {
                tabla[i / 5][i % 5] = tabla_string[i];
            }
            return tabla;
        }).Select(x => new Ayudita(x)).ToList();

        int actual_number = -1;
        int tabla_final = -1;
        int bingos = 0;

        int num_tablas = tablas.Count;

        foreach (var numero in nums)
        {
            for (int i = 0; i < tablas.Count; i++)
            {
                if (tablas[i].ganado) continue;

                if (tablas[i].check(numero))
                {
                    bingos++;
                    if(bingos == num_tablas)
                    {
                        tabla_final = i;
                        actual_number = numero;
                    }
                }
            }
            //if (tabla_final != -1) break;
        }

        int total = tablas[tabla_final].sum() * actual_number;

        Console.WriteLine(total);

    }

    class Ayudita
    {
        public int[][] tabla;
        public bool[,] marked;
        public bool ganado;

        public Ayudita(int[][] tablita)
        {
            tabla = tablita;
            ganado = false;
            marked = new bool[5, 5];

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    marked[i, j] = false;
        }

        public bool check(int num)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (tabla[i][j] == num && !marked[i, j])
                    {
                        marked[i, j] = true;

                        ganado = check_row(i) || check_column(j);

                        return ganado;
                    }
                }
            }
            return false;
        }

        private bool check_column(int num)
        {
            for (int i = 0; i < 5; i++)
                if (!marked[i, num])
                    return false;
            return true;
        }

        private bool check_row(int num)
        {
            for (int i = 0; i < 5; i++)
                if (!marked[num, i])
                    return false;
            return true;
        }

        public int sum()
        {
            int sum = 0;

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    if (!marked[i, j])
                        sum += tabla[i][j];

            return sum;
        }
    }

}
