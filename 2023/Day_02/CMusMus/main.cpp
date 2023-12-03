#include <iostream>
#include <fstream>
#include <algorithm>
#include <functional>
#include <numeric>
#include <cctype>
#include <cmath>
#include <vector>
#include <string>

#include <ranges>
namespace rv = std::ranges::views;

std::string path = "input.txt";

auto isNum = [](char it) { return std::isdigit(it); };
auto isAlpha = [](char it) { return std::isalpha(it); };

std::unordered_map<std::string, int> rgb { {"red" , 0} ,{"green" , 0} ,{"blue" , 0} };
std::unordered_map<std::string, int> barrier = {{"red" , 12},{"green" , 13},{"blue" , 14}};

std::vector<std::string> split(std::string input, std::string delimiter = "\n"){
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

std::vector<std::string> read(std::string path){
    std::fstream f( path , std::ios::in);
    f.seekg(0,f.end);
    uint fileSize = f.tellg();
    f.seekg(0,f.beg);
    char* buffer = new char[fileSize];
    f.read(buffer, fileSize);
    std::string strbuff = buffer;
    delete buffer;
    return split(strbuff);
}

bool check(){
    return rgb["red"] <= barrier["red"] 
        && rgb["green"] <= barrier["green"]
        && rgb["blue"] <= barrier["blue"];
}

int one(std::vector<std::string> input){
    int goods = 0;
    for( auto[ idx,line] : rv::enumerate(input) ) {
        bool valid = true;
        line = line.substr(line.find(":"));
        auto subGames = split(line, ";"); 
        for(auto& subGame : subGames){ // Game 1 : {...}
            rgb["red"] = 0;
            rgb["green"] = 0;
            rgb["blue"] = 0;
            auto buckets = split(subGame, ",");
            for (auto& bucket : buckets){ // {red green blue}
                std::string buffer;
                std::ranges::copy_if(bucket, std::back_inserter(buffer), isNum);
                int value = std::stoi(buffer);
                buffer.clear();
                std::ranges::copy_if(bucket, std::back_inserter(buffer), isAlpha);
                rgb[buffer] = value;
            }
            if(!check()){
                valid = false;
                break;
            }
        }
        if(valid){
            goods += idx+1;
        }
    }

    return goods;
}

int two(std::vector<std::string> input){
    int goods = 0;
    for( auto[ idx,line] : rv::enumerate(input) ) {
        bool valid = true;
        line = line.substr(line.find(":"));
        auto subGames = split(line, ";"); 
        rgb["red"] = 0;
        rgb["green"] = 0;
        rgb["blue"] = 0;
        for(auto& subGame : subGames){ // Game 1 : {...}
            auto buckets = split(subGame, ",");
            for (auto& bucket : buckets){ // {red green blue}
                std::string buffer;
                std::ranges::copy_if(bucket, std::back_inserter(buffer), isNum);
                int value = std::stoi(buffer);
                buffer.clear();
                std::ranges::copy_if(bucket, std::back_inserter(buffer), isAlpha);
                rgb[buffer] = std::max(value, rgb[buffer]);
            }
        }
        goods += (rgb["red"]*rgb["green"]*rgb["blue"]);    
    }
    return goods;
}

int main(){
    std::vector<std::string> v = read(path);
    std::cout << one(v) << "\n";
    std::cout << two(v)<< "\n";
}