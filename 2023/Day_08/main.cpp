#include <iostream>
#include <fstream>
#include <concepts>
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

constexpr std::string path = "input.txt";

std::string instructions;
struct dir{
    std::string izq;
    std::string der;
};
std::map<std::string,dir> nodos; 

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

void parse(std::vector<std::string> input) {
    instructions = input[0];

    for( auto [nIdx,line] : rv::enumerate(input) | rv::drop(2)){
        int idx = 0;
        std::string filterdLine;
        std::ranges::copy_if(line , std::back_inserter(filterdLine), [](char c){ return std::isalpha(c); });
        nodos[filterdLine.substr(0, 3)] = dir { 
            filterdLine.substr(3, 3) ,
            filterdLine.substr(6, 3)
        };
    }

}

int one() {
    std::string node = "AAA";
    int count = 0;
    for(; node != "ZZZ"; ++count){

        char instr = instructions[count % instructions.size()];
        auto& [left, right] = nodos[node];
        node = instr == 'L' ? left : right;

    }
    return count;
}

uint64_t two() {
    std::vector<uint64_t> cycleCount;
    
    auto keysA = nodos | rv::filter(
        [](const std::pair<std::string,dir>& pair) { return pair.first[2] == 'A'; 
        }) | rv::keys;
    std::vector<std::string> startNodes(std::begin(keysA),std::end(keysA));

    for( auto& startNode : startNodes ){
        std::string node = startNode;
        uint64_t count = 0; 
        for(; node[2] != 'Z'; ++count){

            char instr = instructions[count % instructions.size()];
            auto& [left, right] = nodos[node];
            node = instr == 'L' ? left : right;

        }

        cycleCount.push_back(count);
    }


    uint64_t result = cycleCount[0];
    for (uint64_t i = 1; i < cycleCount.size(); ++i) {
        result = (result * cycleCount[i]) / std::gcd(result, cycleCount[i]);
    }


    return result;

}

int main()
{
    auto v = read(path);
    parse(v);
    std::cout << one() << "\n";
    std::cout << two() << "\n";
}