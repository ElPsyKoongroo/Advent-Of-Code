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

struct Coord{
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
                if(next == '-') loop.push_back(Pipe{HPipe, newX , newY});
                else if(next == 'J') loop.push_back(Pipe{JPipe, newX , newY});
                else if(next == '7') loop.push_back(Pipe{_7Pipe, newX , newY});
                actual = dir::W;
                SPipe.source = dir::E;
                break;
            }
            case 1:{ // NORTH
                if(next == '|') loop.push_back(Pipe{VPipe, newX , newY});
                else if(next == '7') loop.push_back(Pipe{_7Pipe, newX , newY});
                else if(next == 'F') loop.push_back(Pipe{FPipe, newX , newY});
                actual = dir::S;
                SPipe.source = dir::N;
                break;
            }
            case 2:{ // WEST
                if(next == '-') loop.push_back(Pipe{HPipe, newX , newY});
                else if(next == 'L') loop.push_back(Pipe{LPipe, newX , newY});
                else if(next == 'F') loop.push_back(Pipe{FPipe, newX , newY});
                SPipe.source = dir::W;
                actual = dir::E;
                break;
            }
            case 3:{ // SOUTH
                if(next == '|') loop.push_back(Pipe{VPipe, newX , newY});
                else if(next == 'L') loop.push_back(Pipe{LPipe, newX , newY});
                else if(next == 'J') loop.push_back(Pipe{JPipe, newX , newY});
                actual = dir::N;
                SPipe.source = dir::S;
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
    SPipe.dest = actual;
    return std::ceil(loop.size()/2);
}

void dfs(Coord c, std::vector<std::string>& square , std::vector<Coord>& group) {
    if (c.y < 0 || c.y >= square.size() || c.x < 0 || c.x >= square[0].size() || square[c.y][c.x] != '.')
        return;

    square[c.y][c.x] = 'X'; // Mark visited
    group.emplace_back(Coord{ c.x, c.y } );

    dfs(Coord{c.x + 1, c.y}, square, group);
    dfs(Coord{c.x - 1, c.y}, square, group);
    dfs(Coord{c.x, c.y + 1}, square, group);
    dfs(Coord{c.x, c.y - 1}, square, group);
}

bool counts(Coord& c, std::vector<std::string>& square , int minX, int minY){
    if (c.y+1 < 0 || c.y+1 >= square.size())
        return false; // I
    auto newc= Coord{c.x,c.y+1};
    int count = 0;
    int size = square.size();
    while (newc.y < size){
        if(square[newc.y][newc.x] == '-'){
            count++;
            newc.y++;
            continue;
        }
        else if(square[newc.y][newc.x] == 'L'){
            newc.y++;
            while (!(square[newc.y][newc.x] == 'F' || square[newc.y][newc.x] == '7')) newc.y++;
            if(square[newc.y][newc.x]== '7') count++;
            newc.y++;
            continue;
        }
        else if(square[newc.y][newc.x] == 'J'){
            newc.y++;
            while (!(square[newc.y][newc.x] == 'F' || square[newc.y][newc.x] == '7')) newc.y++;
            if(square[newc.y][newc.x] == 'F') count++;
            newc.y++;
            continue;
        }
        else if(square[newc.y][newc.x] == 'F'){
            newc.y++;
            while (!(square[newc.y][newc.x] == 'L' || square[newc.y][newc.x] == 'J')) newc.y++;
            if(square[newc.y][newc.x] == 'J') count++;
            newc.y++;
            continue;
        }
        else if(square[newc.y][newc.x] == '7'){
            newc.y++;
            while (!(square[newc.y][newc.x] == 'L' || square[newc.y][newc.x] == 'J')) newc.y++;
            if(square[newc.y][newc.x]== 'L') count++;
            newc.y++;
            continue;
        }
        newc.y++;
    }

    return count % 2 == 1;

}

int two(std::vector<std::string> input) {
    uint64_t count = 0;

    for(auto [idy,line] : input | rv::enumerate){
        for(auto [idx,ch] : line | rv::enumerate){
            int x = idx;
            int y = idy;
            auto it = std::find_if(loop.begin() , loop.end() , [x,y](Pipe& p){ return p.x == x && p.y == y; });
            if(it == loop.end()){
                ch = '.';
            }
        }
    }

    auto it = *std::find_if(dataPipes.begin()+1 ,dataPipes.end(), [](Data& d) { 
        return ( d.source == SPipe.source || d.dest == SPipe.source )
                && ( d.source == SPipe.dest || d.dest == SPipe.dest ) ; 
    });
    input[loop.front().y][loop.front().x] = it.type;

    int minX = std::ranges::min_element(loop, std::less{} , &Pipe::x)->x;
    int maxX = std::ranges::max_element(loop,std::less{} , &Pipe::x)->x;
    int minY = std::ranges::min_element(loop,std::less{} , &Pipe::y)->y;
    int maxY = std::ranges::max_element(loop,std::less{} , &Pipe::y)->y;

    std::vector<std::string> square;
    for(auto& line : input | rv::drop(minY) | rv::take(maxY-minY+1)){
        auto newLine = line.substr(minX, maxX - minX + 1);
        square.push_back(newLine);
    }

    auto squareCopy = square;
    std::vector<std::vector<Coord>> groups;
    for(auto [y,line] : squareCopy | rv::enumerate){
        auto x = std::find(line.begin(), line.end(), '.');
        while(x != line.end()){
            std::vector<Coord> group;
            dfs(Coord{(int)std::distance(line.begin(),x),(int)y} , squareCopy, group);
            groups.push_back(group);
            x = std::find(x+1, line.end(), '.');
        }
    }
    



    for (auto& group : groups){
        if(counts(group.front(), square , minX, minY)){
            count+= group.size();
        }
    }
    return count;
}

int main()
{
    auto v = read(path);
    std::cout << one(v) << "\n";
    std::cout << two(v) << "\n";
    return 0;
}