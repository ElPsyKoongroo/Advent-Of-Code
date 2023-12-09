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

std::vector<std::vector<int>> histories;

bool isNum(std::string str) {
    try {
        std::stoi(str);
        return true;
    }
    catch (std::exception e) {
        return false;
    }
}

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
    for(auto& line : input){
        std::string token;
        histories.push_back(std::vector<int>());
        std::stringstream iss(line);
        iss >> token;
        while(isNum(token)){
            histories.back().push_back(std::stoi(token));
            token.clear();
            iss >> token;
        }
    }
}

auto Zero = [](const int64_t i){ return i == 0; };
auto adjDiff = [](int64_t a, int64_t b){ return b - a ;};
auto lastElem = [](std::vector<int64_t> vec){ return vec.back(); };
auto firstElem = [](std::tuple<std::ptrdiff_t, std::vector<int64_t> &> idx_vec) { 
    return std::get<0>(idx_vec) % 2 ? - (std::get<1>(idx_vec).front()) : std::get<1>(idx_vec).front() ;
};

int64_t one() {
    int64_t count = 0;
    for( auto& history : histories ){
        std::vector<std::vector<int64_t>> topDown;
        topDown.push_back(std::vector<int64_t>(history.begin(),history.end()));
        while( !std::ranges::all_of(topDown.back() , Zero ) ){
            auto diffs = topDown.back() | rv::adjacent_transform<2>(adjDiff );    
            topDown.push_back(std::vector<int64_t>(diffs.begin() , diffs.end()));
        }
        auto last = topDown | rv::transform(lastElem) ;
        count += std::accumulate(last.begin(), last.end(), 0);
    }
    return count;
}

int64_t two() {
    int64_t count = 0;
    for( auto& history : histories ){
        std::vector<std::vector<int64_t>> topDown;
        topDown.push_back(std::vector<int64_t>(history.begin(),history.end()));
        while( !std::ranges::all_of(topDown.back() , Zero ) ){
            auto diffs = topDown.back() | rv::adjacent_transform<2>(adjDiff );    
            topDown.push_back(std::vector<int64_t>(diffs.begin() , diffs.end()));
        }
        
        for( auto [idx, vec] : topDown | rv::enumerate | rv::reverse | rv::drop(1) ){
            vec.insert(vec.begin(), - topDown[idx+1].front() + vec.front());
        }

        count += topDown.front().front();
        // auto first = topDown | rv::reverse | rv::enumerate | rv::transform(firstElem) ;
        // std::vector<int64_t> firsts (first.begin(), first.end());
        // count += std::accumulate(first.begin(), first.end(), 0);
    }
    return count;
}

int main()
{
    auto v = read(path);
    parse(v);
    std::cout << one() << "\n";
    std::cout << two() << "\n";
}