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
    return split(strbuff);
}

std::vector<std::string> parseSpelled(std::vector<std::string> input){
    std::vector<std::string> numbers = {
        "zero","one","two","three","four","five","six","seven","eight","nine"
    };
    for(auto [lineIdx, line] : rv::enumerate(input)){
        for(auto [idx, num] : rv::enumerate(numbers)){
            while(line.contains(num)){
                auto pos = line.find(num);
                input[lineIdx].insert(pos+1,std::to_string(idx));
            }
        }
    }
    return input;
}

int one(std::vector<std::string> input){
    std::vector<int> res;
    for(auto& line : input){
        line.erase(std::remove_if(line.begin(), 
                              line.end(),
                              [](unsigned char x) { return !std::isdigit(x); }),
               line.end());
        std::string nums;
        nums.push_back(line.front());
        nums.push_back(line.back());
        res.push_back(std::stoi(nums));
    }
    return std::accumulate(res.begin(),res.end() , 0);
}

int two(std::vector<std::string> input){
    std::vector<int> res;
    for(auto& line : parseSpelled(input)){
        line.erase(std::remove_if(line.begin(), 
                              line.end(),
                              [](unsigned char x) { return !std::isdigit(x); }),
               line.end());
        std::string nums;
        nums.push_back(line.front());
        nums.push_back(line.back());
        res.push_back(std::stoi(nums));
    }
    return std::accumulate(res.begin(),res.end() , 0);
}

int main(){
    std::vector<std::string> v = read(path);
    std::cout << one(v) << "\n";
    std::cout << two(v);
}
