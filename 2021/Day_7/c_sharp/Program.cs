namespace Advent_Of_Code;

public class EJ7 : EJ
{
    public void EJ()
    {
        List<int> nums = input2
            .Split(",")
            .Select(x => int.Parse(x.Trim()))
            .ToList();

        int max = nums.Max();
        int min = nums.Min();

        int size = max - min + 1;

        List<int> distances = Enumerable.Repeat(0, size).ToList();

        for (int i = 0; i < size; i++)
        {
            foreach (int num in nums)
            {
                distances[i] += Math.Abs(num - (min+i));
            }
        }

        int min2 = distances.Min();

        Console.WriteLine(min2);

    }

    public void EJ2()
    {
        List<int> nums = input2
            .Split(",")
            .Select(x => int.Parse(x.Trim()))
            .ToList();

        int max = nums.Max();
        int min = nums.Min();

        int size = max - min + 1;

        List<int> distances = Enumerable.Repeat(0, size).ToList();

        for (int i = 0; i < size; i++)
        {
            foreach (int num in nums)
            {
                int distance = Math.Abs(num - (min + i));

                int fuel = 0;

                for (int j = 1; j <= distance; j++)
                {
                    fuel += j;
                }

                distances[i] += fuel;
            }
        }

        int min2 = distances.Min();

        Console.WriteLine(min2);

    }
}