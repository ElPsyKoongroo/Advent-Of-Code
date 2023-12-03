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

struct Number{
    std::string value;
    int x,y;
    std::string xLocation;
};

bool checkSymbols(Number& n, std::vector<std::string>& input){
    for( int layer = -1; layer<2 ; ++layer){
        int x = n.x + layer;
        if(x < 0 || x >= input.size()) continue;
        int size = (n.value.size()+1) ;
        for(int pos = -1; pos < size ; ++pos ){
            int y = n.y + pos;
            int lsize = input[y].size();
            if(y < 0 || y >= lsize) continue;
            char token = input[x][y];
            if(!std::isdigit(token) && (token != '.')){
                return true;
            }
        }
    }  
    return false;
}

bool checkX(Number& n, std::vector<std::string>& input){
    for( int layer = -1; layer<2 ; ++layer){
        int x = n.x + layer;
        if(x < 0 || x >= input.size()) continue;
        int size = (n.value.size()+1) ;
        for(int pos = -1; pos < size ; ++pos ){
            int y = n.y + pos;
            int lsize = input[y].size();
            if(y < 0 || y >= lsize) continue;
            char token = input[x][y];
            if((token == '*')){
                n.xLocation += std::to_string(x); 
                n.xLocation += std::to_string(y) ; 
                return true;
            }
        }
    }  
    return false;
}

int one(std::vector<std::string>& input){
    std::vector<Number> numbers;
    int count = 0;
    std::string currentNumber = "";
    for( auto [idx, line] : rv::enumerate(input) ){
        for (int i = 0; i < line.size(); ++i) {
            if (std::isdigit(line[i])) {
                currentNumber += line[i];
            } else if (!currentNumber.empty()) {
                numbers.emplace_back(Number{currentNumber, (int)idx, (int)(i - currentNumber.size())});
                currentNumber.clear();
            }
        }
        if (!currentNumber.empty()) {
            numbers.emplace_back(Number{currentNumber, (int)idx, (int)(line.size() - currentNumber.size())});
            currentNumber.clear();
        }
    }
   for(auto& num : numbers){
        if(checkSymbols(num,input)){
             count += std::stoi(num.value);
        }
    }
    return count;
}


int two(std::vector<std::string> input){
    std::unordered_map<std::string, std::vector<Number>> closeToX;
    std::vector<Number> numbers;
    int count = 0;
    std::string currentNumber = "";
    for( auto [idx, line] : rv::enumerate(input) ){
        for (int i = 0; i < line.size(); ++i) {
            if (std::isdigit(line[i])) {
                currentNumber += line[i];
            } else if (!currentNumber.empty()) {
                numbers.emplace_back(Number{currentNumber, (int)idx, (int)(i - currentNumber.size())});
                currentNumber.clear();
            }
        }
        if (!currentNumber.empty()) {
            numbers.emplace_back(Number{currentNumber, (int)idx, (int)(line.size() - currentNumber.size())});
            currentNumber.clear();
        }
    }
    for(auto& num : numbers){
        if(checkX(num,input)){
            closeToX[num.xLocation].push_back(num);
        }
    }

    for(auto [value, vecs] : closeToX ){
        if(vecs.size() == 2){
            count += (std::stoi(vecs[0].value) * std::stoi(vecs[1].value));
        }
    }
    return count;
}

int main(){
    std::vector<std::string> v = read(path);
    std::cout << one(v) << "\n";
    std::cout << two(v) << "\n";
}