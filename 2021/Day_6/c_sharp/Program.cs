namespace Advent_Of_Code;

public class EJ6 : EJ
{
    public void EJ()
    {
        List<int> peces = input2.Split(",").Select(x => int.Parse(x.Trim())).ToList();

        ulong[] total_each = new ulong[9];

        for (int i = 0; i < 7; i++)
        {
            total_each[i] = (ulong)peces.Count(x=> x == i);
        }

        total_each[7] = 0;
        total_each[8] = 0;

        for (int i = 0; i < 256; i++)
        {
            ulong total_to_create = total_each[0];

            for (int j = 0; j < 8; j++)
            {
                total_each[j] = total_each[j + 1];
            }
            total_each[8] = total_to_create;
            total_each[6] += total_to_create;
        }

        ulong total = 0;

        for (int i = 0; i < 9; i++)
        {
            total += total_each[i];
        }

        Console.WriteLine($"Total : {total}");

    }
}