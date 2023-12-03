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

#include <ranges>
namespace rv = std::ranges::views;

std::string path = "input.txt";

auto isNum = [](char it) { return std::isdigit(it); };
auto isAlpha = [](char it) { return std::isalpha(it); };

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
bool isInteger(const std::string& str) {
    try {
        std::stoi(str);
        return true;
    } catch (std::exception e) {
        return false;
    }
}

struct Number{
    std::string value;
    int x,y;
};

int one(std::vector<std::string> input){
    std::vector<Number> numbers;
    for( auto [idx, line] : rv::enumerate(input) ){
        int pos = 0;
        std::stringstream iss(line);
        std::string token;
        while (std::getline(iss, token, '.')) {
            token.erase(std::remove_if(token.begin(), token.end(),
                        [](unsigned char x) { return !std::isdigit(x); }), token.end());
            if (!token.empty()) {
                numbers.push_back(Number{ token, (int)idx, pos} );
            }
            pos += token.size() + 1;
        }
    }
    return 0;
}

int two(std::vector<std::string> input){
    return 0;
}

int main(){
    std::vector<std::string> v = read(path);
    std::cout << one(v) << "\n";
    //std::cout << two(v) << "\n";
}