using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace cs;

class Program
{
    record Tile(Direction D1, Direction D2) {
        public Tile(char c) : this(Direction.None, Direction.None){
            (D1, D2) = c switch {
                '|' => (Direction.N, Direction.S),
                '-' => (Direction.W, Direction.E),
                'L' => (Direction.N, Direction.E),
                'J' => (Direction.N, Direction.W),
                '7' => (Direction.S, Direction.W),
                'F' => (Direction.S, Direction.E),
                'S' => (Direction.Any, Direction.Any),
                _ => (Direction.None, Direction.None),
            };
        }

        public Direction[] GetDirections() {
            if(D1 != Direction.Any) {
                return [D1, D2];
            }
            return [
                Direction.N, Direction.S,
                Direction.E, Direction.W
            ];
        }

        public bool ContainsDirection(Direction d) =>
            D1 == Direction.Any || D1 == d || D2 == d;

        public static bool CanConnect(Tile t1, Tile t2, Direction d) {
            if(t1.D1 == Direction.None || t2.D1 == Direction.None) return false;

            if(!t1.ContainsDirection(d)) return false;

            if(t2.D1 == Direction.Any) return true;

            return t2.ContainsDirection(InverseDirection(d));
        }
    };

    static Tile ToTile(char c) => new(c);

    enum Direction {
        N, W, E, S,
        None, Any
    }

    static Direction InverseDirection(Direction d) => d switch {
        Direction.N => Direction.S,
        Direction.S => Direction.N,
        Direction.W => Direction.E,
        Direction.E => Direction.W,
        _ => Direction.None
    };

    static (int x, int y) DirectionToIndex(Direction d) => d switch {
        Direction.N => (0, -1),
        Direction.S => (0, 1),
        Direction.W => (-1, 0),
        Direction.E => (1, 0),
        _ => (0,0)
    };

    static void Main(string[] args)
    {
        string path = args.FirstOrDefault("../AOCinput");
        // Console.WriteLine(Sol1(path));
        Console.WriteLine(Sol2(path));
    }
    

    static Tile[][] ParseInput(string path) {
        return 
            File.ReadAllLines(path)
            .Select(l=> 
                l.Select(ToTile).ToArray()
            ).ToArray();
    }

    static int Sol1(string path) {
        
        var input = ParseInput(path);
        var (sizeY, sizeX) = (input.Length, input[0].Length);

        var (y ,x, _) = 
            input
            .SelectMany((l,i) => l.Select((v, j) => (i,j,v)))
            .First(t=> t.v.D1 == Direction.Any);

        Console.WriteLine($"S -> {y},{x}");

        Direction actualDir = Direction.None;

        foreach(var d in new Direction[]{Direction.N,Direction.S,Direction.E}) {
            var dIndex = DirectionToIndex(d);
            (int y, int x) DI = (y + dIndex.y, x + dIndex.x);

            if( DI.y < sizeY && DI.y >= 0 && 
                DI.x < sizeX && DI.x >= 0) {
                if(Tile.CanConnect(input[y][x], input[DI.y][DI.x], d)) {
                    actualDir = 
                        input[DI.y][DI.x]
                        .GetDirections().First(x=> x != InverseDirection(d));

                    x = DI.x;
                    y = DI.y;
                    break;
                }
            }
        }

        int dist = 0;

        while(input[y][x].D1 != Direction.Any) {
            dist++;

            var dIndex = DirectionToIndex(actualDir);
            (int y, int x) DI = (y + dIndex.y, x + dIndex.x);

            actualDir = 
                input[DI.y][DI.x]
                .GetDirections().First(x=> x != InverseDirection(actualDir));

            x = DI.x;
            y = DI.y;        
        }

        Console.WriteLine($"Dist: {dist}");

        var sequence = Enumerable.Range(1, dist).ToArray();

        var result = 
            sequence
            .Zip(sequence.Reverse())
            .Select(x=> Math.Min(x.First, x.Second))
            .Max();

        return result;
    }

    static int[][] Adjacents(Tile[][] input) {
        var (sizeY, sizeX) = (input.Length, input[0].Length);

        var adjacents = new int[sizeY][];
        for(int i = 0; i < sizeY; ++i) {
            adjacents[i] = new int[sizeX];
        }

        for(int i = 0; i < sizeY; ++i) {
            for(int j = 0; j < sizeX; ++j) {
                var actTile = input[i][j];
                if(actTile.D1 == Direction.None) {
                    adjacents[i][j] = 0;
                    continue;
                }
                int conections = 0;

                foreach(var d in actTile.GetDirections()) {
                    var dIndex = DirectionToIndex(d);
                    (int y, int x) = (i + dIndex.y, j + dIndex.x);

                    if( y < sizeY && y >= 0 && 
                        x < sizeX && x >= 0) {
                        if(Tile.CanConnect(actTile, input[y][x], d))
                            conections++;

                    }
                }

                adjacents[i][j] = conections;
            }
        }

        for(int i = 0; i < adjacents.Length; ++i) {
            for(int j = 0; j < adjacents[0].Length; ++j) {
                Console.Write(adjacents[i][j]);
            }
            Console.WriteLine();
        }

        return adjacents;
    }


    static int Sol2(string path) {
        var input = ParseInput(path);
        var (sizeY, sizeX) = (input.Length, input[0].Length);

        var (y ,x, _) = 
            input
            .SelectMany((l,i) => l.Select((v, j) => (i,j,v)))
            .First(t=> t.v.D1 == Direction.Any);

        var loop = new int[sizeY][];
        for(int i = 0; i < sizeY; ++i) {
            loop[i] = [..Enumerable.Repeat(0, sizeX)];
        }

        Direction actualDir = Direction.None;

        Direction[] startDirections = [Direction.Any,Direction.Any];
        var (startX, startY) = (x,y);
        bool found = false;
        foreach(var d in new Direction[]{Direction.N,Direction.S,Direction.E,Direction.W}) {
            var dIndex = DirectionToIndex(d);
            (int y, int x) DI = (startY + dIndex.y, startX + dIndex.x);

            if( DI.y < sizeY && DI.y >= 0 && 
                DI.x < sizeX && DI.x >= 0) {
                if(Tile.CanConnect(input[startY][startX], input[DI.y][DI.x], d)) {
                    if(!found) {
                        actualDir = 
                            input[DI.y][DI.x]
                            .GetDirections().First(x=> x != InverseDirection(d));

                        loop[y][x] = 1;
                        x = DI.x;
                        y = DI.y;
                        startDirections[0] = d;
                        found = true;
                    } else {
                        startDirections[1] = d;
                        break;
                    }
                }
            }
        }
        
        int dist = 1;

        while(input[y][x].D1 != Direction.Any) {

            var dIndex = DirectionToIndex(actualDir);
            (int y, int x) DI = (y + dIndex.y, x + dIndex.x);

            actualDir = 
                input[DI.y][DI.x]
                .GetDirections().First(x=> x != InverseDirection(actualDir));

            loop[y][x] = ++dist;

            x = DI.x;
            y = DI.y;        
        }
        input[startY][startX] = new Tile(startDirections[0],startDirections[1]);

        (int, int)[] directions = [
            (0, 1),
            (0, -1),
            (1, 0),
            (-1, 0),
        ];

        for(int i = 0; i < loop.Length; ++i) {
            for(int j = 0; j < loop[0].Length; ++j) {
                if(loop[i][j] > 0) continue;
                bool inside = false;

                foreach(var dir in directions) {
                    int crossedPipes = 0;
                    bool inPipe = false;
                    bool crossingPipe = false; //Siguiendo una pipe
                    int lastPipe = -1;
                    Direction[] followingPipe = [];
                    int actY = i+dir.Item1;
                    int actX = j+dir.Item2;

                    bool InRange() => actX >= 0 && actX < sizeX && actY >= 0 && actY < sizeY;

                    for (; InRange(); actX+=dir.Item2, actY+=dir.Item1) {
                        if(loop[actY][actX] <= 0) { //No estoy en pipe
                            if(!inPipe) continue; //No estaba en pipe

                            inPipe = false;
                            lastPipe = -1;
                            if(!crossingPipe) { //No estaba siguiendo
                                crossedPipes++;
                            } else {
                                var lastDirs = input[actY-dir.Item1][actX-dir.Item2].GetDirections();
                                if(!lastDirs.Any(x=> followingPipe.Any(y => x == y))) {
                                    crossedPipes++;
                                }
                            }
                            
                            crossingPipe = false;
                        } else { //Estoy en pipe
                            if(inPipe) { //Estaba en pipe
                                var wasCrossing = crossingPipe;
                                crossingPipe = Math.Abs(loop[actY][actX] - lastPipe) == 1
                                                || (
                                                    (lastPipe == dist && loop[actY][actX] == 1) || 
                                                    (loop[actY][actX] == dist && lastPipe == 1)
                                                );

                                if(!wasCrossing && !crossingPipe) { //No estaba siguiendo ni ahora
                                    crossedPipes++;
                                }
                                if(!wasCrossing) {
                                    followingPipe = input[actY-dir.Item1][actX-dir.Item2].GetDirections();
                                }
                                if(wasCrossing && !crossingPipe) {
                                    var lastDirs = input[actY-dir.Item1][actX-dir.Item2].GetDirections();
                                    if(!lastDirs.Any(x=> followingPipe.Any(y => x == y))) {
                                        crossedPipes++;
                                    }
                                }
                            } else { //No estaba en pipe
                                crossingPipe = false;
                                inPipe = true;
                                followingPipe = input[actY][actX].GetDirections();
                            }
                            lastPipe = loop[actY][actX];
                        }
                    }

                    if(inPipe) {
                        if(!crossingPipe) crossedPipes++;
                        else {
                            var lastDirs = input[actY-dir.Item1][actX-dir.Item2].GetDirections();
                            if(!lastDirs.Any(x=> followingPipe.Any(y => x == y))) {
                                crossedPipes++;
                            }
                        }
                    }

                    if(crossedPipes != 0) {
                        inside = crossedPipes%2 == 1;
                        break;
                    }
                }
                if(inside) loop[i][j] = -1;
            }
        }

        return loop.SelectMany(x=>x).Count(x=> x == -1);
    }
}
