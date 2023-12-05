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

using u64 = uint64_t;

struct Map {
    u64 dest, source, length;
};
std::string path = "input.txt";
std::vector<u64> seeds;
std::vector<std::vector<Map>> sections;

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
    return split(strbuff, "\n\n");
}

bool isNum(std::string in) {
    try {
        std::stoull(in);
        return true;
    }
    catch (std::exception e) {
        return false;
    }
}
auto toU64 = [](std::string num) { return std::stoull(num); };
void parse(std::vector<std::string> input) {
    auto listaSeeds = split(input[0].substr(input[0].find(":")+2), " ");
    std::ranges::copy(listaSeeds | rv::transform(toU64), std::back_inserter(seeds));

    for (auto& in : input | rv::drop(1) ) {
        sections.push_back(std::vector<Map>());
        auto nums = split(in);
        for (auto& num : nums | rv::drop(1) ) {
            auto mapita = Map();
            std::stringstream iss(num);
            std::string token;
            iss >> token;
            mapita.dest = toU64(token);
            token.clear();
            iss >> token;
            mapita.source = toU64(token);
            token.clear();
            iss >> token;
            mapita.length = toU64(token);
            sections.back().push_back(mapita);
        }
    }
}

int one() {
    u64 minimal = UINT64_MAX;
    for ( auto& seed : seeds ) {
        u64 modifiedSeed = seed;
        for (auto& section : sections) {
            for (auto& map : section) {
                auto diff = modifiedSeed - map.source;
                if (diff >= 0 && diff < map.length) {
                    modifiedSeed = diff + map.dest;
                    break;
                }
            }
        }
        minimal = std::min(minimal, modifiedSeed);
    }
    return minimal;
}

int two(std::vector<std::string> input) {
    return 0;
}

int main()
{
    auto v = read(path);
    parse(v);
    std::cout << one() << "\n";
    std::cout << two(v) << "\n";
}
