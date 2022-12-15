using System.Text.RegularExpressions;

namespace AdventOfCode;
public static class Program
{
    private const string regString =
            @"^\[" +        //0
            @"[^\[\]]*" +
            "(" +           //1
            "(" +           //3
            @"(?'Open'\[)" +
            @"[^\[\]]*" +
            ")+" +          //3
            "(" +           //4
            @"(?'Close-Open'\])" +
            @"[^\[\]]*" +
            ")+" +          //4
            @"[^\[\]]*" +
            ")*" +          //1
            @"[^\[\]]*" +
            "(?" +          //5
            "(Open)" +
            "(?!)" +
            ")" +           //5
            @"\]";          //0
    public static void Main(string[] args)
    {
        Day_13 day = new();
        //day.Answer1();
        day.Answer2();
    }
}

public class Day_13
{
    string testText;
    string inputText;

    private const string regString =
        @"^\[" +        //0
        @"[^\[\]]*" +
        "(" +           //1
        "(" +           //3
        @"(?'Open'\[)" +
        @"[^\[\]]*" +
        ")+" +          //3
        "(" +           //4
        @"(?'Close-Open'\])" +
        @"[^\[\]]*" +
        ")+" +          //4
        @"[^\[\]]*" +
        ")*" +          //1
        @"[^\[\]]*" +
        "(?" +          //5
        "(Open)" +
        "(?!)" +
        ")" +           //5
        @"\]";          //0

    public Day_13()
    {
        inputText = File.ReadAllText("../AOCinput");
        testText = File.ReadAllText("../AOCtest");
    }

    public void Answer1()
    {
        var pairs = inputText.Split("\n\n");

        int total = 0;

        for(int i = 0; i < pairs.Length; ++i)
        {
            var lines = pairs[i].Split("\n");
            int compare = CompareStrings(lines[0], lines[1]);

            if (compare == 1) total+= i+1;
        }

        System.Console.WriteLine(total);
    }

    private int CompareStrings(string left, string right)
    {
        return (int.TryParse(left, out int leftValue), int.TryParse(right, out int rightValue)) switch
        {
            (true, true)    => Math.Sign(rightValue-leftValue),
            (false, false)  => CompareLists(left, right),
            (true, false)   => CompareLists($"[{left}]", right),
            (false, true)   => CompareLists(left, $"[{right}]"),
        };
    }

    private int CompareLists(string left, string right)
    {
        int leftIt = 0;
        int rightIt = 0;

        string leftComp;
        string rightComp;
        
        switch (leftIt >= left.Length, rightIt >= right.Length)
        {
            case(false, true): {return -1;}
            case(true, false): {return 1;}
            case(true, true): {return 0;}
        };
        
        left=left[1..^1];
        right=right[1..^1];
    


        Regex reg = new(regString);

        while (leftIt < left.Length && rightIt < right.Length)
        {
            switch (left[leftIt], right[rightIt])
            {
                case ('[', '['):
                {
                    leftComp = reg.Match(left[leftIt..]).Value;
                    rightComp = reg.Match(right[rightIt..]).Value;

                    int comp = Compare(leftComp, rightComp);

                    if(comp != 0) return comp;
                    break;
                }
                case ('[', char r) when r != '[': //List - Num
                {
                    leftComp = reg.Match(left[leftIt..]).Value;
                    rightComp = string.Concat(right[rightIt..].TakeWhile(x=> x - '0' >= 0 && x - '0' <= 9));

                    int comp = Compare(leftComp, $"[{rightComp}]");

                    if(comp != 0) return comp;
                    break;
                }
                case (char l, '[') when l != '[': //Num - List
                {
                    rightComp = reg.Match(right[rightIt..]).Value;
                    leftComp = string.Concat(left[leftIt..].TakeWhile(x=> x - '0' >= 0 && x - '0' <= 9));

                    int comp = Compare($"[{leftComp}]", rightComp);

                    if(comp != 0) return comp;
                    break;
                }
                case (char l, char r):
                {
                    leftComp = string.Concat(left[leftIt..].TakeWhile(x=> x - '0' >= 0 && x - '0' <= 9));
                    rightComp = string.Concat(right[rightIt..].TakeWhile(x=> x - '0' >= 0 && x - '0' <= 9));

                    int leftNum;
                    int rightNum;
                    
                    leftNum = int.Parse(leftComp);
                    rightNum = int.Parse(rightComp);
                
                    int comp = Math.Sign(rightNum - leftNum);

                    if(comp != 0) return comp;

                    rightIt+=rightComp.Length;
                    if(rightIt < right.Length && right[rightIt] == ',') ++rightIt;

                    leftIt+=leftComp.Length;
                    if(leftIt < left.Length && left[leftIt] == ',') ++leftIt;

                    break;
                }
            }
        }

        return (leftIt < left.Length, rightIt < right.Length) switch
        {
            (false, true) => 1,
            (true, false) => -1,
            _ => 0
        };
        

        int Compare(string leftCompAux, string rightCompAux)
        {
            int comp = CompareLists(leftCompAux, rightCompAux);

            rightIt+=rightComp.Length;
            if(rightIt < right.Length && right[rightIt] == ',') ++rightIt;

            leftIt+=leftComp.Length;
            if(leftIt < left.Length && left[leftIt] == ',') ++leftIt;

            return comp;
        }
    }

    public void Answer2()
    {
        var elementsAux = string.Join("\n", inputText.Split("\n\n")).Split("\n").ToList();
        elementsAux.Add("[[2]]");
        elementsAux.Add("[[6]]");

        var elements = elementsAux.ToArray();

        int totalCycles = elements.Length;

        for(int i = 0; i < totalCycles - 1; ++i)
        {
            for(int j = 0; j < totalCycles - i - 1; ++j)
            {
                int compare = CompareStrings(elements[j], elements[j+1]);
                if(compare == -1)
                {
                    (elements[j], elements[j+1]) = (elements[j+1], elements[j]);
                }
            }
        }

        int index1 = elements.ToList().FindIndex(x=> x == "[[2]]") + 1;
        int index2 = elements.ToList().FindIndex(x=> x == "[[6]]") + 1;

        System.Console.WriteLine(index1);
        System.Console.WriteLine(index2);
        System.Console.WriteLine(index1 * index2);
    }
}