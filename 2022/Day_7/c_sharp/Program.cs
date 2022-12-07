namespace AdventOfCode;
public static class Program
{
    public static void Main()
    {
        Day_7 day = new();
        day.Answer2();
    }
}

public class Day_7
{
    string[] inputLines;
    string[] testLines;
    public Day_7()
    {
        inputLines = File.ReadAllLines("../AOCinput");
        testLines = File.ReadAllLines("../AOCtest");
    }
    Dictionary<string, ulong> dirSizes;
    Dictionary<string, List<string>> dirTree;

    public void Answer1()
    {
        const ulong MAX_SIZE = 100000;


        dirSizes = new();
        dirTree = new();

        GetAllSizes(inputLines);

        Dictionary<string, ulong> dirSizesRecursives = new();

        foreach(var dir in dirSizes.Keys)
        {
            dirSizesRecursives.Add(dir, GetDirSize(dir));
        }

        ulong totalSize = dirSizesRecursives.Where(x=> x.Value <= MAX_SIZE).Select(x=> x.Value).Aggregate((size, element) => size += element);

        System.Console.WriteLine(totalSize);
    }

    public void Answer2()
    {
        const ulong MAX_SIZE = 100000ul;
        
        const ulong FILESYSTEM_SIZE = 70000000ul;

        const ulong SPACE_NEEDED = 30000000ul;

        dirSizes = new();
        dirTree = new();

        GetAllSizes(inputLines);
        
        Dictionary<string, ulong> dirSizesRecursives = new();

        foreach(var dir in dirSizes.Keys)
        {
            dirSizesRecursives.Add(dir, GetDirSize(dir));
        }

        ulong usedSpace = dirSizesRecursives["/"];

        ulong freeSpace = FILESYSTEM_SIZE - usedSpace;

        ulong totalSize = 
            dirSizesRecursives
            .Select(x=> x.Value)
            .Order()
            .First( x=> freeSpace + x >= SPACE_NEEDED); 

        System.Console.WriteLine(totalSize);
    }

    private void GetAllSizes(string[] inputLines)
    {
        HashSet<string> alreadyLsPaths = new();

        bool hasToCheckLsOutput = true;

        string actualPath = "";

        foreach(string line in inputLines)
        {
            string[] input = line.Split(" ");

            switch (input)
            {
                case [string dolar, string command, ..] when dolar == "$": //Tiene un comando
                {
                    if(command == "cd")
                    {
                        string path = input[2];
                        if(path == "/") actualPath = path;
                        else if(path == "..") actualPath = actualPath.Substring(0, actualPath.LastIndexOf('/'));
                        else 
                        {
                            actualPath = actualPath + ( actualPath == "/" ? "" : "/") + path;
                        }

                        if(!dirTree.ContainsKey(actualPath))
                        {
                            dirTree.Add(actualPath, new List<string>());
                        }
                    }
                    else
                    {
                        if(alreadyLsPaths.Add(actualPath))
                        {
                             hasToCheckLsOutput = true;
                             dirSizes.Add(actualPath, 0);
                        }
                        else hasToCheckLsOutput = false;
                    }
                    break;
                }
                case [string size, _] when hasToCheckLsOutput && int.TryParse(size, out int sizeInt): //Es un archivo
                {
                    dirSizes[actualPath] += (ulong)sizeInt;
                    break;
                }
                default:    //Es un fichero
                {
                    if(hasToCheckLsOutput)
                    {
                        dirTree[actualPath].Add(input[1]);
                    }
                    break;
                }
            }
        }
    }

    private ulong GetDirSize(string dirPath)
    {
        ulong totalSize = 0;

        foreach (var dir in dirTree[dirPath])
        {
            string actualDir = dirPath + (dirPath == "/" ? "" : "/") + dir;

            totalSize += GetDirSize(actualDir); 
        }

        totalSize += dirSizes[dirPath];
        return totalSize;
    }
}