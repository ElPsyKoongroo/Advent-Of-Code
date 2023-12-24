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

auto ith_element = [](int i) {
    return [i](auto const& v){
        return v[i];
    };
};

uint64_t one(std::vector<std::vector<std::string>>& input) {
    uint64_t count = 0;
    for (auto& group : input){
        bool isValid = false;
        for (int i = 1 ; i < group.front().size() && !isValid  ; ++i ){
            isValid = true;
            for (auto& line : group){
                std::vector<std::string> parts;
                auto r = line | rv::chunk(i);
                for (const auto& chunk : line | rv::chunk(i)) {
                    parts.push_back(std::string(chunk.begin(), chunk.end()));
                }
                auto pleg = rv::zip(parts[0] | rv::reverse , parts[1] );
                int times = std::ranges::count_if( pleg , [](std::pair<char,char> p){ return p.first == p.second; });
                if( times != std::ranges::min(parts[0].size(), parts[1].size()) ){
                    isValid = false;
                    break;
                }
            }
            if(isValid){
                std::cout << "---> "<< i <<" <---\n";
                count += i;
            }
        }

        if (isValid) continue;

        for (int i = 1 ; i < group.size() && !isValid ; ++i ){
            isValid = true;

            for ( int ic = 0 ; ic < group.front().size() ; ++ic ){

                auto column = [ic](int i) {
                    return std::views::transform(ith_element(i));
                };
                auto l = group | column(ic);
                std::string line(l.begin(), l.end());
                
                std::vector<std::string> parts;
                for (const auto& chunk : line | rv::chunk(i)) {
                    parts.push_back(std::string(chunk.begin(), chunk.end()));
                }
                auto pleg = rv::zip(parts[0] | rv::reverse, parts[1]);
                int times = std::ranges::count_if( pleg , [](std::pair<char,char> p){ return p.first == p.second; });
                if( times != std::ranges::min(parts[0].size(), parts[1].size()) ){
                    isValid = false;
                    break;
                }
            }
            if(isValid){
                std::cout << "---7 "<< i <<" L----\n";
                count += 100*i;
            }
        }
    }
    return count;
}

uint64_t two(std::vector<std::vector<std::string>>& input) {
    uint64_t count = 0;
        for (auto& group : input){
        std::cout << "----------------------------------\n";
        bool isValid = false;
        bool isSmudge = false;
        int index = INT32_MAX;
        for (int i = 1 ; i < group.front().size() ; ++i ){
            int matches = 0;
            isValid = true;
            for (auto& line : group){
                std::vector<std::string> parts;
                auto r = line | rv::chunk(i);
                for (const auto& chunk : line | rv::chunk(i)) {
                    parts.push_back(std::string(chunk.begin(), chunk.end()));
                }
                auto pleg = rv::zip(parts[0] | rv::reverse , parts[1] );
                int times = std::ranges::count_if( pleg , [](std::pair<char,char> p){ return p.first == p.second; });
                matches += times;
                if( times != std::ranges::min(parts[0].size(), parts[1].size()) ){
                    isValid = false;
                }
            }
            int factor = std::min((int)group.front().size() - i, i);
            if(matches == group.size() * factor - 1 ){
                isValid = true;
                isSmudge = true;
                index = i;
                std::cout << "Sm---> "<< i <<" <---\n";
                break;
            }
            if(isValid){
                index = i;
                std::cout << "V---> "<< i <<" <---\n";
                // break;
            }
        }
        if (isSmudge) {count += index ; continue;}
        isValid = false;
        for (int i = 1 ; i < group.size(); ++i ){
            isValid = true;
            int matches = 0;
            for ( int ic = 0 ; ic < group.front().size() ; ++ic ){
                
                auto column = [ic](int i) {
                    return std::views::transform(ith_element(i));
                };
                auto l = group | column(ic);
                std::string line(l.begin(), l.end());
                
                std::vector<std::string> parts;
                for (const auto& chunk : line | rv::chunk(i)) {
                    parts.push_back(std::string(chunk.begin(), chunk.end()));
                }
                auto pleg = rv::zip(parts[0] | rv::reverse, parts[1]);
                int times = std::ranges::count_if( pleg , [](std::pair<char,char> p){ return p.first == p.second; });
                matches += times;
                if( times != std::ranges::min(parts[0].size(), parts[1].size()) ){
                    isValid = false;
                }
            }
            int factor = std::min((int)group.size() - i, i);
            if(matches == group.front().size() * factor - 1 ){
                isValid = true;
                isSmudge = true;
                index = i;
                std::cout << "Sm---7 "<< i <<" L----\n";
                break;
            }
            if(isValid){
                std::cout << "V---7 "<< i <<" L----\n";
                index = 100 * i;
                //break;
            }
        }
        if (isSmudge) { count += ( 100 * index ); continue;}
        else count += ( index );
    }
    return count;
}

int main()
{
    auto data = read(path);
    auto v = rv::transform ( data, [](std::string str) { return split(str); } );
    std::vector<std::vector<std::string>> arr(v.begin(), v.end());
    //std::cout << one(arr) << "\n";
    std::cout << two(arr) << "\n";
    return 0;
}