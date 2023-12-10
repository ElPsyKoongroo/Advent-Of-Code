#include <iostream>
#include <fstream>
#include <algorithm>
#include <functional>
#include <numeric>
#include <cctype>
#include <cmath>
#include <vector>
#include <string>
#include <sstream>
#include <utility>
#include <set>
#include <ranges>
#include <map>
#include <chrono>

namespace rv = std::ranges::views;
using pairlist = std::initializer_list<std::pair<int,int>>;
std::string path = "input.txt";

auto toInt = [](std::string num) { return std::stoull(num); };

bool isNum(std::string str) {
    try {
        std::stoi(str);
        return true;
    }
    catch (std::exception e) {
        return false;
    }
}
enum class dir {
    N,
    S,
    W,
    E,
    None
};
struct Data{
    char type;
    dir source;
    dir dest;
};

struct Pipe{
    Data val;
    int x,y;
};
Data SPipe { 'S' , dir::None, dir::None}; 
Data HPipe { '-' , dir::E, dir::W}; 
Data VPipe { '|' , dir::N, dir::S}; 
Data LPipe { 'L' , dir::N, dir::E}; 
Data JPipe { 'J' , dir::N, dir::W}; 
Data _7Pipe { '7' , dir::S, dir::W}; 
Data FPipe { 'F' , dir::S, dir::E}; 
std::vector<Data> dataPipes = {SPipe, HPipe, VPipe, LPipe,JPipe,_7Pipe,FPipe};
std::vector<Pipe> loop;
std::vector<std::string> split(std::string input, std::string delimiter = "\n") {
    std::vector<std::string> lines;
    size_t pos = 0;
    std::string token;
    while ((pos = input.find(delimiter)) != std::string::npos) {
        token = input.substr(0, pos);
        lines.push_back(token);
        input.erase(0, pos + delimiter.length());
    }
    lines.push_back(input);
    return lines;
}

std::vector<std::string> read(std::string path) {
    std::fstream f(path, std::ios::in);
    f.seekg(0, f.end);
    int fileSize = f.tellg();
    f.seekg(0, f.beg);
    char* buffer = new char[fileSize];
    f.read(buffer, fileSize);
    std::string strbuff = buffer;
    delete[] buffer;
    return split(strbuff);
}

std::pair<dir,Pipe> nextPipe(dir actDir, Pipe actPipe, std::vector<std::string> input){
    if(actPipe.y == 102 && actPipe.x == 3){
        std::cout << "a\n";
    }
    dir nextDir = actDir == actPipe.val.source ? actPipe.val.dest : actPipe.val.source;
    switch(nextDir){
        case dir::E : {
            char nextPipeChar = input[actPipe.y][actPipe.x+1];
            Data data = *std::ranges::find_if(dataPipes, 
                [nextPipeChar](char t){ return t == nextPipeChar; }, &Data::type);
            return std::make_pair(dir::W, Pipe{data,actPipe.x+1,actPipe.y});
        }
        case dir::N : {
            char nextPipeChar = input[actPipe.y-1][actPipe.x];
            Data data = *std::ranges::find_if(dataPipes, 
                [nextPipeChar](char t){ return t == nextPipeChar; }, &Data::type);
            return std::make_pair(dir::S, Pipe{data,actPipe.x,actPipe.y-1});
        }
        case dir::S : {
            char nextPipeChar = input[actPipe.y+1][actPipe.x];
            Data data = *std::ranges::find_if(dataPipes, 
                [nextPipeChar](char t){ return t == nextPipeChar; }, &Data::type);
            return std::make_pair(dir::N, Pipe{data,actPipe.x,actPipe.y+1});
        }
        case dir::W : {
            char nextPipeChar = input[actPipe.y][actPipe.x-1];
            Data data = *std::ranges::find_if(dataPipes, 
                [nextPipeChar](char t){ return t == nextPipeChar; }, &Data::type);
            return std::make_pair(dir::E, Pipe{data,actPipe.x-1,actPipe.y});
        }
        default : {
            auto e = std::exception();
            throw e;
            return std::make_pair(dir::None, Pipe{dataPipes[0],actPipe.x-1,actPipe.y});
        }
    }
}

int one(std::vector<std::string> input) {
    dir actual = dir::N;
    int sizeY = input.size();
    int sizeX = input.begin()->size();
    for(auto [y, line] : input | rv::enumerate){
        auto x = line.find('S');
        if(x != std::string::npos){
            loop.push_back(Pipe{ SPipe , (int)x , (int)y  });
        };
    }
    for( auto [idx, dirs] : 
            std::vector<std::pair<int,int>>(pairlist{ {0,1}, {-1,0} , {0,-1} , {1,0} })
            | rv::enumerate ) {
        if(loop.size() == 2) break;
        int newY = loop.front().y + dirs.first;
        int newX = loop.front().x + dirs.second;
        if(!(newX >= 0 && newX < sizeX && newY >= 0 && newY < sizeY)) continue;
        char next = input[newY][newX];
        if(next == '.') continue;
        
        switch(idx){
            case 0:{ // EAST
               // funcion() if(next == '-') loop.push_back(Pipe{HPipe, newX , newY});
                else if(next == 'J') loop.push_back(Pipe{JPipe, newX , newY});
                else if(next == '7') loop.push_back(Pipe{_7Pipe, newX , newY});
                actual = dir::W;
                break;
            }
            case 1:{ // NORTH
                if(next == '|') loop.push_back(Pipe{VPipe, newX , newY});
                else if(next == '7') loop.push_back(Pipe{_7Pipe, newX , newY});
                else if(next == 'F') loop.push_back(Pipe{FPipe, newX , newY});
                actual = dir::S;
                break;
            }
            case 2:{ // WEST
                if(next == '-') loop.push_back(Pipe{HPipe, newX , newY});
                else if(next == 'L') loop.push_back(Pipe{LPipe, newX , newY});
                else if(next == 'F') loop.push_back(Pipe{FPipe, newX , newY});
                actual = dir::E;
                break;
            }
            case 3:{ // SOUTH
                if(next == '|') loop.push_back(Pipe{VPipe, newX , newY});
                else if(next == 'L') loop.push_back(Pipe{LPipe, newX , newY});
                else if(next == 'J') loop.push_back(Pipe{JPipe, newX , newY});
                actual = dir::N;
                break;
            }
        }
    }
    
    char pipe = loop.back().val.type;
    while (pipe != 'S'){
        auto [newDir, newPipe] = nextPipe(actual, loop.back() , input);
        loop.push_back(newPipe);
        actual = newDir;
        pipe = loop.back().val.type;
    }
    
    return std::ceil(loop.size()/2);
}
struct Coord{
    int x , y;
};

void expansion(Coord c, bool& isOutside , std::vector<std::string>& square, std::vector<Coord>& visited){
    if(c.x < 0 || c.x >= square.front().size() || c.y < 0 || c.y >= square.size()) { isOutside = true; return; }
    if(square[c.y][c.x] == '.') visited.push_back(c) ;
    expansion(Coord{c.x + 1, c.y} , isOutside , square , visited );
}

void refresh ( std::vector<std::string>& square, int x , int y ){
    std::vector<Coord> visited;
    bool isOutside = false;
    expansion(Coord{x,y}, isOutside, square, visited );
}

int two(std::vector<std::string> input) {
    uint64_t count = 0;

    int minX = std::ranges::min_element(loop, std::less{} , &Pipe::x)->x;
    int maxX = std::ranges::max_element(loop,std::less{} , &Pipe::x)->x;
    int minY = std::ranges::min_element(loop,std::less{} , &Pipe::y)->y;
    int maxY = std::ranges::max_element(loop,std::less{} , &Pipe::y)->y;

    std::vector<std::string> square;
    for(auto& line : input | rv::drop(minY) | rv::take(maxY-minY+1)){
        auto newLine = line.substr(minX, maxX - minX + 1);
        auto hashLine = rv::transform(newLine , [](char c){ return c != '.' ? '#' : '.'; });
        std::string hashStr = std::string(hashLine.begin(),hashLine.end());
        square.push_back(hashStr);
    }

    for(auto [y,line] : square | rv::enumerate){
        int x = line.find('.');
        if(x == std::string::npos) continue;
        refresh(square, x , y);
    }

    return std::ranges::count(square | std::views::join, 'I');
}
// 1 -> 5

// 4 -> 9
// 0 -> drop(4) -> 4 -> take(9-4+1) 
// 0 -> drop(1) -> 1 -> take(5-1+1) -> -> 5 

int main()
{
    auto v = read(path);
    //parse(v);
    std::cout << one(v) << "\n";
    std::cout << two(v) << "\n";
}