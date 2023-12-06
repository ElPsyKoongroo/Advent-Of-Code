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

struct data {
    uint64_t distancia, tiempo;
};

auto toInt = [](std::string num) { return std::stoull(num); };

bool isNum(std::string str){
    try{
        std::stoi(str);
        return true;
    }
    catch(std::exception e){
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
    return split(strbuff, "\n");
}

std::vector<data> parse(std::vector<std::string> input) {
    std::vector<data> datos;
    auto timeRow = input.front();
    auto disRow = input.back();
    timeRow = timeRow.substr(timeRow.find(":")+1);
    disRow = disRow.substr(disRow.find(":")+1);
    std::stringstream issT(timeRow);
    std::stringstream issD(disRow);
    std::string tokenT;
    std::string tokenD;
    for(;;){
        issT >> tokenT;
        if(!isNum(tokenT)){
            break;
        }
        issD >> tokenD;
        auto datito = data(toInt(tokenT), toInt(tokenD));
        tokenD.clear();
        tokenT.clear();
        datos.push_back(datito);
    }
    return datos;
}

data parse2(std::vector<std::string> input) {
    std::vector<data> datos;
    auto timeRow = input.front();
    auto disRow = input.back();
    timeRow = timeRow.substr(timeRow.find(":")+1);
    disRow = disRow.substr(disRow.find(":")+1);

    timeRow.erase(std::remove_if(timeRow.begin(), timeRow.end(), isspace), timeRow.end());
    disRow.erase(std::remove_if(disRow.begin(), disRow.end(), isspace), disRow.end());

    return data(toInt(timeRow), toInt(disRow));
}

int one(std::vector<data>& datos) {
    int count = 1;
    for(auto [tiempo , distancia] : datos){
        auto disc = std::sqrt( std::pow(tiempo, 2) - 4 * (distancia+1) );
        int max_time = std::floor((tiempo + disc)/2);
        int min_time = std::ceil((tiempo - disc)/2);
        count*= (max_time-min_time+1);
    }
    return count;
}

uint64_t two(data dato) {
    auto [tiempo , distancia] = dato;
    auto disc = std::sqrt( std::pow(tiempo, 2) - 4 * (distancia+1) );
    uint64_t max_time = std::floor((tiempo + disc)/2);
    uint64_t min_time = std::ceil((tiempo - disc)/2);
    return (max_time-min_time+1);
}

int main()
{
    auto v = read(path);
    auto datos = parse(v);
    auto d = parse2(v);
    std::cout << one(datos) << "\n";
    std::cout << two(d) << "\n";
}
