namespace cs;

class Program
{
    static void Main(string[] args)
    {
        string path = args.FirstOrDefault("../AOCtest");
        Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }

    static (int x, int y, int z)[] ParseInput(string path) =>
        File.ReadAllLines(path)
        .Select(l => l.Split(",").Select(int.Parse).ToArray())
        .Select(x => (x[0],x[1],x[2]))
        .ToArray();

    static int Sol1(string path) {
        var input = ParseInput(path);

        var total = 0;
        foreach(var (x,y,z) in input) {
            if(!input.Contains((x+1,y,z))) total++;
            if(!input.Contains((x-1,y,z))) total++;
            if(!input.Contains((x,y+1,z))) total++;
            if(!input.Contains((x,y-1,z))) total++;
            if(!input.Contains((x,y,z+1))) total++;
            if(!input.Contains((x,y,z-1))) total++;
        }
        return total;
    }

    static int Sol2(string path) {
        var input = ParseInput(path);

        int maxX = input.MaxBy(X=>X.x).x;
        int maxY = input.MaxBy(X=>X.y).y;
        int maxZ = input.MaxBy(X=>X.z).z;
        int minX = input.MinBy(X=>X.x).x;
        int minY = input.MinBy(X=>X.y).y;
        int minZ = input.MinBy(X=>X.z).z;

        List<(int x, int y, int z)> noInfo = [];

        for(int x = minX-1; x <= maxX+1; ++x) {
            for(int y = minY-1; y <= maxY+1; ++y) {
                for(int z = minZ-1; z <= maxZ+1; ++z) {
                    var p = (x,y,z);
                    if(!input.Contains(p)) noInfo.Add(p);
                }
            }
        }

        Queue<(int x, int y, int z)> actualQueue = [];
        List<(int x, int y, int z)> actualPath = [];
        List<(int x, int y, int z)> inside = [];


        while(noInfo.Count!=0) {
            actualQueue.Enqueue(noInfo[0]);
            noInfo.RemoveAt(0);

            (int x,int y,int z)[] directions = [
                (1,0,0),(-1,0,0),
                (0,1,0),(0,-1,0),
                (0,0,1),(0,0,-1),
            ];
            bool isInside = true;

            while(actualQueue.TryDequeue(out var actual)) {
                actualPath.Add(actual);

                foreach(var (x, y, z) in directions) {
                    int newX = actual.x+x;
                    int newY = actual.y+y;
                    int newZ = actual.z+z;
                    if(newX > maxX || newX < 0 ||
                        newY > maxY || newY < 0 ||
                        newZ > maxZ || newZ < 0) {
                            isInside = false;
                            continue;
                    }
                    var newP = (newX, newY, newZ);
                    if(noInfo.Contains(newP)) {
                        noInfo.Remove(newP);
                        actualQueue.Enqueue(newP);
                    }
                }
            }

            if(isInside) {
                inside = [..inside, ..actualPath];
            }
            actualPath.Clear();
        }

        var total = 0;
        foreach(var (x,y,z) in input) {
            if(!input.Contains((x+1,y,z)) && !inside.Contains((x+1,y,z))) total++;
            if(!input.Contains((x-1,y,z)) && !inside.Contains((x-1,y,z))) total++;
            if(!input.Contains((x,y+1,z)) && !inside.Contains((x,y+1,z))) total++;
            if(!input.Contains((x,y-1,z)) && !inside.Contains((x,y-1,z))) total++;
            if(!input.Contains((x,y,z+1)) && !inside.Contains((x,y,z+1))) total++;
            if(!input.Contains((x,y,z-1)) && !inside.Contains((x,y,z-1))) total++;
        }
        return total;
    }
}
