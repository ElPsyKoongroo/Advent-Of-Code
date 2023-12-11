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
std::string path = "input.txt";

struct Coord{
    int x,y;
};
std::vector<int> emptyRow;
std::vector<int> emptyCol;
std::vector<Coord> galaxies;

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

void countEmpty(std::vector<std::string>& input){

    for(int i = 0; i < input.size(); ++i){
        auto idx = input[i].find('#');
        if(idx == std::string::npos){
            emptyRow.push_back(i);
        }
    }
    for (size_t i = 0; i < input[0].size(); ++i) {
        bool insert_column = std::ranges::all_of(input, [i](const std::string& row) {
            return row[i] != '#';
        });

        if (insert_column) {
            emptyCol.push_back(i);
        }
    }
}

uint64_t one(std::vector<std::string>& input) {
    uint64_t count = 0;
    countEmpty(input);

    for( auto [y , line] : input | rv::enumerate){
        for( auto [x , ch] : line | rv::enumerate){
            if(ch == '#') galaxies.push_back(Coord{(int)x,(int)y});
        }
    }

    for ( auto [idx, galaxy] : galaxies | rv::enumerate | rv::take(galaxies.size()-1) ){
        count += std::accumulate(galaxies.begin()+idx+1 ,
            galaxies.end(),0,[&galaxy](uint64_t val, Coord& c){
            uint64_t num = 1;
            auto& farY = galaxy.y > c.y ? galaxy.y : c.y; 
            auto& closeY = galaxy.y < c.y ? galaxy.y : c.y; 
            auto& farX = galaxy.x > c.x ? galaxy.x : c.x; 
            auto& closeX = galaxy.x < c.x ? galaxy.x : c.x;

            auto rowAmong = std::count_if(emptyRow.begin() , emptyRow.end() , [&](int rowIdx){
                return rowIdx < farY && rowIdx > closeY;
            } );

            auto colAmong = std::count_if(emptyCol.begin() , emptyCol.end() , [&](int colIdx){
                return colIdx < farX && colIdx > closeX;
            } );

            return val + std::abs(c.y - galaxy.y) + (num * rowAmong) + std::abs(c.x - galaxy.x) + (num * colAmong);
        });
    }

    return count;
}

uint64_t two(std::vector<std::string>& input) {
    uint64_t count = 0;

    for ( auto [idx, galaxy] : galaxies | rv::enumerate | rv::take(galaxies.size()-1) ){
        count += std::accumulate(galaxies.begin()+idx+1 ,
            galaxies.end(),0ull,[&galaxy](uint64_t val, Coord& c) -> uint64_t {
            uint64_t num = 999'999;
            auto& farY = galaxy.y > c.y ? galaxy.y : c.y; 
            auto& closeY = galaxy.y < c.y ? galaxy.y : c.y; 
            auto& farX = galaxy.x > c.x ? galaxy.x : c.x; 
            auto& closeX = galaxy.x < c.x ? galaxy.x : c.x;

            uint64_t rowAmong = std::count_if(emptyRow.begin() , emptyRow.end() , [&](int rowIdx){
                return rowIdx < farY && rowIdx > closeY;
            } );

            uint64_t colAmong = std::count_if(emptyCol.begin() , emptyCol.end() , [&](int colIdx){
                return colIdx < farX && colIdx > closeX;
            } );

            return (uint64_t)(val + (uint64_t)std::abs(c.y - galaxy.y) + (num * rowAmong) + (uint64_t)std::abs(c.x - galaxy.x) + (num * colAmong));
        });
    }

    return count;
}

//
//  ..#..
//  #....
//  ....#

int main()
{
    auto v = read(path);
    std::cout << one(v) << "\n";
    std::cout << two(v) << "\n";
    return 0;
}