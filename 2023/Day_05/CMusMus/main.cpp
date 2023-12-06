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

using u64 = int64_t; // bromita
struct Map {
    u64 dest, source, length;
};
std::string path = "input.txt";
std::vector<u64> seeds;
std::vector<std::pair<u64,u64>> rSeeds;
std::vector<std::vector<Map>> sections;
auto toU64 = [](std::string num) { return std::stoull(num); };

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

void parse(std::vector<std::string> input) {
    auto listaSeeds = split(input[0].substr(input[0].find(":")+2), " ");
    std::ranges::copy(listaSeeds | rv::transform(toU64), std::back_inserter(seeds));
    
    for(auto par : seeds | rv::chunk(2)){
        rSeeds.push_back(std::make_pair(par[0], par[1]));
    }

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

// Rango1 |_| Rango2
std::pair<u64, u64> interseccion(std::pair<u64, u64> rango1, std::pair<u64, u64> rango2) {
    u64 maxInicio = std::max(rango1.first, rango2.first);
    u64 minFin = std::min(rango1.second, rango2.second);

    if (maxInicio <= minFin) {
        return std::make_pair(maxInicio, minFin);
    } else {
        // No hay intersección
        return std::make_pair(-1, -1);
    }
}

bool hayInterseccion(const std::pair<u64,u64>& a, const std::pair<u64,u64>& b) {
    return a.first <= b.second && b.first <= a.second;
}

std::vector<std::pair<u64,u64>> obtenerRangosSeedNoIntersectados(const std::vector<std::pair<u64,u64>>& lista, const std::pair<u64,u64>& seed) {
    std::vector<std::pair<u64,u64>> rangosNoIntersectadosSeed;

    // Agregar el rango completo de la Seed inicialmente
    rangosNoIntersectadosSeed.push_back(seed);

    for (const auto& rango : lista) {
        std::vector<std::pair<u64,u64>> nuevosRangos;

        for (const auto& seedRango : rangosNoIntersectadosSeed) {
            // Verificar la intersección con el rango de la lista
            if (hayInterseccion(seedRango, rango)) {
                // Dividir el rango de la Seed en dos si hay intersección
                if (rango.first > seedRango.first) {
                    nuevosRangos.push_back({seedRango.first, rango.first - 1});
                }
                if (rango.second < seedRango.second) {
                    nuevosRangos.push_back({rango.second + 1, seedRango.second});
                }
            } else {
                // Si no hay intersección, mantener el rango de la Seed sin cambios
                nuevosRangos.push_back(seedRango);
            }
        }

        // Actualizar la lista de rangos de la Seed
        rangosNoIntersectadosSeed = nuevosRangos;
    }

    return rangosNoIntersectadosSeed;
}

std::vector<std::pair<u64,u64>> getRange(std::vector<Map>& maps, std::pair<u64,u64>& seed ){
    std::vector<std::pair<u64,u64>> ranges;
    std::vector<std::pair<u64,u64>> mapRanges;

    for( auto& map : maps){
        uint64_t mapStart = map.source;
        uint64_t mapEnd = map.source + map.length-1;
        mapRanges.push_back(std::make_pair(mapStart,mapEnd));
        auto inter = interseccion(std::make_pair(mapStart,mapEnd) , seed);
        if(inter.first != -1){
            ranges.push_back(std::make_pair(inter.first-map.source + map.dest , inter.second - map.source + map.dest));
        }
    }

    if( ranges.empty() ) {
        ranges.push_back(seed);
        return ranges;
    }
    auto noIntersectados = obtenerRangosSeedNoIntersectados(mapRanges, seed);
    if(!noIntersectados.empty()){
        for(auto& nI : noIntersectados){
            ranges.push_back(nI);
        }
    }

    return ranges;
}



int two() {
    u64 minimal = INT64_MAX;
    for ( auto& seed : rSeeds ) {
        std::vector<std::pair<u64,u64>> subRanges;
        subRanges.push_back(std::make_pair(seed.first,seed.first + seed.second - 1 ));
        for (auto& section : sections) {
            std::vector<std::pair<u64,u64>> tempRanges;
            for(auto& rangeSeeds : subRanges){
                auto aux = getRange(section, rangeSeeds);
                tempRanges.insert( tempRanges.end(), aux.begin(), aux.end() );
            }
            subRanges.clear();
            subRanges = tempRanges;
        }
        auto minor = *std::min_element(
            subRanges.begin(), 
            subRanges.end(), 
            [](std::pair<u64,u64> p1 , std::pair<u64,u64> p2){
                return p1.first < p2.first;
            });
        minimal = std::min(minimal, minor.first);
        std::cout << minimal << "\n";
    }
    return minimal;
}


int main()
{
    auto v = read(path);
    parse(v);
    //std::cout << one() << "\n";
    std::cout << two() << "\n";
}
