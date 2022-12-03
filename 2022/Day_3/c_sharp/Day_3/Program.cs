namespace Program;

public static class Program
{
    public static void Main()
    {
        Day_3 day = new();
        day.Answer1();
    }
}

public class Day_3
{
    string[] inputLines;
    public Day_3()
    {
        inputLines  = File.ReadAllLines("../../AOCinput");
    }

    public void Answer1()
    {
        int sum = 0;

        foreach(var line in inputLines)
        {
            var badSnack = line.FirstOrDefault(x=> line.Count(y=> y == x) == 2, '-');
            if(badSnack == '-')
            {
                continue;
            } 
                

            int badSnackInt = badSnack >= 'a' ? badSnack - 'a' + 1 : badSnack - 'A' + 27;
            sum += badSnackInt;
        }

        System.Console.WriteLine(sum);
    }
}