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
    return split(strbuff , "\n\n");
}

uint64_t one(std::vector<std::vector<std::string>>& input) {
    uint64_t count = 0;
    for (auto& group : input){
        bool isValid = false;
        // Columnas
        for (int i = 1 ; i < group.front().size() && !isValid  ; ++i ){
            isValid = true;
            for (auto& line : group){
                auto r = line | rv::chunk(i);
                std::vector<std::string> parts(r.begin(), r.end());
                auto pleg = rv::zip(parts[0], parts[1] | rv::reverse );
                int times = std::ranges::count_if( pleg , [](std::pair<char,char> p){ return p.first == p.second; });
                if( times != std::ranges::min(parts[0].size(), parts[1].size()) ){
                    isValid = false;
                    break;
                }
            }
            if(isValid){
                count += i;
            }
        }
        if (isValid) continue;

        for (int i = 1 ; i < group.size() && !isValid ; ++i ){
            isValid = true;
            for (auto& line : group ){
                auto r = line | rv::chunk(i);
                std::vector<std::string> parts(r.begin(), r.end());
                auto pleg = rv::zip(parts[0], parts[1] | rv::reverse );
                int times = std::ranges::count_if( pleg , [](std::pair<char,char> p){ return p.first == p.second; });
                if( times != std::ranges::min(parts[0].size(), parts[1].size()) ){
                    isValid = false;
                    break;
                }
            }
            if(isValid){
                count += i;
            }
        }





    }







    return count;
}

uint64_t two(std::vector<std::vector<std::string>>& input) {
    uint64_t count = 0;

    return count;
}

int main()
{
    auto data = read(path);
    auto v = rv::transform ( data, [](std::string str) { return split(str); } );
    std::vector<std::vector<std::string>> arr(v.begin(), v.end());
    std::cout << one(arr) << "\n";
    std::cout << two(arr) << "\n";
    return 0;
}