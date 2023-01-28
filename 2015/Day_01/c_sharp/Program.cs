namespace AdventOfCode;
public class Program
{
    public static void Main(string[] args)
    {
        Day_01 day = new();
        day.Answer2();
    }
}

public class Day_01
{
    string inputText;
    public Day_01()
    {
        inputText = File.ReadAllText("../AOCinput");
    }

    public void Answer1()
    {
        int negative = inputText.Count(x=> x == ')');
        int floor = inputText.Length - negative * 2;
        System.Console.WriteLine(floor);
    }

    public void Answer2()
    {
        int total = 0;
        for(int i = 0; i < inputText.Length; ++i)
        {
            if(inputText[i] == '(')
                ++total;
            else
                --total;
            if(total == -1)
            {
                System.Console.WriteLine(i+1);
                break;
            }
            
        }
    }
}