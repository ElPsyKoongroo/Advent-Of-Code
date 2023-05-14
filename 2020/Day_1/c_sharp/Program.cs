namespace Day_1;

class Program
{
    static void Main(string[] args)
    {
        bool test = args.Length != 0;
        Day_1 day = new(test);
        //day.Answer1();
        day.Answer2();
    }
}

public class Day_1
{
    string[] inputLines;
    public Day_1(bool test = false)
    {
        inputLines = test ? File.ReadAllLines("../AOCtest") : File.ReadAllLines("../AOCinput");
    }
    public void Answer1()
    {
        const int desired = 2020;

        int[] input = inputLines.Select(x=> int.Parse(x)).ToArray();

        for(int i = 0; i < input.Length; ++i)
        {
            for(int j = i+1; j < input.Length; ++j)
            {
                if(input[i] + input[j] == desired)
                {
                    System.Console.WriteLine(input[i] * input[j]);
                    return;
                }
            }
        }
    }

    public void Answer2()
    {
        const int desired = 2020;

        int[] input = inputLines.Select(x=> int.Parse(x)).ToArray();

        for(int i = 0; i < input.Length; ++i)
        {
            for(int j = i+1; j < input.Length; ++j)
            {
                for(int k = j+1; k < input.Length; ++k)
            {
                if(input[i] + input[j] + input[k] == desired)
                {
                    System.Console.WriteLine(input[i] * input[j] * input[k]);
                    return;
                }
            }
            }
        }
    }
}