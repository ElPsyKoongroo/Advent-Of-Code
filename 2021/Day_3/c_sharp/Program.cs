namespace Advent_Of_Code;

public class EJ3 : EJ
{

    public void EJ3_2()
    {
        int gamma = 0, epsilon = 0;
        int[] ones = new int[12];

        int total = input.Count;

        for (int i = 0; i < 12; i++) ones[i] = 0;

        foreach (var aux in input)
        {
            for (int i = 0; i < aux.Length; i++)
            {
                int num = (int)Char.GetNumericValue(aux[i]);

                if (num == 1)
                    ones[i]++;
            }
        }

        
        foreach (int i in ones)
        {
            gamma <<= 1;
            epsilon <<= 1;

            if(i < input.Count / 2)
                gamma += 1;
            else
                epsilon += 1;
        }

        var oxy = input;
        var co2 = input;

        for (int i = 0; i < 12; i++)
        {
            int one_co2 = 0;
            int one_oxy = 0;

            foreach (var aux in oxy)
            {
                int num = (int)Char.GetNumericValue(aux[i]);

                if (num == 1)
                    one_oxy++;
                
            }
            foreach (var aux in co2)
            {
                int num = (int)Char.GetNumericValue(aux[i]);

                if (num == 1)
                    one_co2++;

            }
            one_oxy = one_oxy >= oxy.Count / 2.0 ? 1 : 0;
            one_co2 = one_co2 < co2.Count / 2.0 ? 1 : 0;

            if (oxy.Count > 1)
            {
                oxy = oxy.Where(x =>
                {
                    return (int)Char.GetNumericValue(x[i]) == one_oxy;
                }).ToList();
            }

            if (co2.Count > 1)
            {
                co2 = co2.Where(x =>
                {
                    return (int)Char.GetNumericValue(x[i]) == one_co2;
                }).ToList();
            }

            if (co2.Count == 1 && oxy.Count == 1) break;
        }

        int a = Convert.ToInt32(oxy.First(), 2);
        int b = Convert.ToInt32(co2.First(), 2);

        Console.WriteLine($"Ox = {a}, co2 = {b}");

        Console.WriteLine(a*b);

        Console.Read();
    }
}
