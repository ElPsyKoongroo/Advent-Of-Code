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
    return split(strbuff);
}

bool isNum(std::string in) {
    try {
        std::stoi(in);
        return true;
    }
    catch(std::exception e){
        return false;
    }
}


int one(std::vector<std::string> input) {
    int count = 0;
    for (auto& line : input) {
        int winNumCount = 0;
        line = line.substr(line.find(":")+1);
        std::string token;
        std::stringstream iss(line);
        std::set<int> winNums;
        iss >> token;
        while (isNum(token)) {
            winNums.insert(std::stoi(token));
            token.clear();
            iss >> token;
        }
        token.clear();
        iss >> token;
        while (isNum(token)) {
            auto [it, inserted] = winNums.insert(std::stoi(token));
            if (!inserted) winNumCount++;
            token.clear();
            iss >> token;
        }

        if (winNumCount == 0) continue;
        else count += pow(2, winNumCount - 1);
    }
    return count;
}

int two(std::vector<std::string> input) {
    std::map<int, int> boletos;
    int size = input.size();
    for ( auto i : rv::iota(1,size+1) ) {
        boletos[i] = 1;
    }
    int idx = 1;
    for (auto& line : input) {
        int boletitos = boletos[idx];
        int winNumCount = 1;
        line = line.substr(line.find(":") + 1);
        std::string token;
        std::stringstream iss(line);
        std::set<int> winNums;
        iss >> token;
        while (isNum(token)) {
            winNums.insert(std::stoi(token));
            token.clear();
            iss >> token;
        }
        token.clear();
        iss >> token;
        while (isNum(token)) {
            auto [it, inserted] = winNums.insert(std::stoi(token));
            if (!inserted) {
                boletos[idx + winNumCount++]+= boletitos;
            }
            token.clear();
            iss >> token;
        }
        idx++;
    }

    int count = 0;
    for (auto [boleto, cantidad] : boletos) {
        count += cantidad;
    }
    return count;
}

int main()
{
    auto v = read(path);
    std::cout << one(v) << "\n";
    std::cout << two(v) << "\n";
}
