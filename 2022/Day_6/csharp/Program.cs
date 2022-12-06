using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode;


public static class Program
{
    public static void Main()
    {
        Day_6 day = new();
        day.Answer2();
    }
}

public class Day_6
{
    string inputText;
    string testText;
    public Day_6()
    {
        inputText = File.ReadAllText("../AOCinput");
        testText = File.ReadAllText("../AOCtest");
    }
    public void Answer1()
    {
        Regex reg = new Regex(@"(\w)(?!\1)(\w)(?!\1|\2)(\w)(?!\1|\2|\3)(\w)");

        Match m = reg.Match(inputText);

        int pos = m.Index + 4;

        System.Console.WriteLine(pos);
        System.Console.WriteLine(m.Value);
    }

    public void Answer2()
    {
        StringBuilder regTextBuld = new();

        const int size = 14;

        for(int i = 0; i < size; ++i)
        {
            if(i != 0)
            {
                regTextBuld.Append("(?!");
                for(int j = 1; j <= i; ++j)
                {
                    if(j != 1) regTextBuld.Append("|");
                    regTextBuld.Append($"\\{j}");
                }
                regTextBuld.Append(")");
            }
            regTextBuld.Append(@"(\w)");
        }
        string regText = regTextBuld.ToString();
        
        Regex reg = new Regex(regText);

        Match m = reg.Match(inputText);

        int pos = m.Index + size;

        System.Console.WriteLine(pos);
        System.Console.WriteLine(m.Value);
    }
}