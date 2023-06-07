namespace Day_1;

public static class Program
{
    public static void Main()
    {
        var relativePath = "../AOCinput";
        var input = File.ReadAllLines(relativePath);

        int actualElf = 0;

        List<int> maxes = new();

        foreach(var snack in input)
        {
            if(string.IsNullOrEmpty(snack.Trim()))
            {
                maxes.Add(actualElf);
                actualElf = 0;
                continue;
            }
            actualElf += int.Parse(snack.Trim());
        }

        maxes = maxes.OrderDescending().ToList();

        int maxThree = 0;
        for(int i = 0; i < 3; i++)
        {
            maxThree += maxes[i];
        }

        System.Console.WriteLine($"MaxOne: {maxes[0]}, MaxThree: {maxThree}");
        return;
    }
}