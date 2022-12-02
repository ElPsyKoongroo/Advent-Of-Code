namespace Day_1;

public static class Program
{
    public static void Main()
    {
        var relativePath = "../../AOCinput";
        var input = File.ReadAllLines(relativePath);

        int maxElf = -1;
        int actualElf = 0;

        foreach(var snack in input)
        {
            if(string.IsNullOrEmpty(snack.Trim()))
            {
                if(actualElf > maxElf)
                {
                    maxElf = actualElf;
                }
                actualElf = 0;
                continue;
            }
            actualElf += int.Parse(snack.Trim());
        }

        maxElf = actualElf > maxElf ? actualElf : maxElf;
        

        System.Console.WriteLine(maxElf);

        return;
    }
}