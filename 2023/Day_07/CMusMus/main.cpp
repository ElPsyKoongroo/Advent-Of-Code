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

struct Player {
    std::string cards;
    uint64_t bid;
};

auto toInt = [](std::string num) { return std::stoull(num); };

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

std::vector<Player> parse(std::vector<std::string> input) {
    std::vector<Player> datos;
    std::string cards;
    uint64_t bid;
    for (auto row : input) {
        std::stringstream iss(row);
        iss >> cards;
        iss >> bid;
        datos.push_back(Player{ cards, bid });
    }
    return datos;
}
//7  AAAAA - A           - [1] 5
//6  BBBBA - B, A        - [2] 4_1
//5  BBBAA - B, A        - [2] 3_2
//4  BBB32 - B,3,2       - [3] 3_1_1
//3  BBAA2 - B,A,2       - [3] 2_2_1
//2  AA123 - A,1,2,3     - [4] 2_1_1_1
//1  AB123 - A,B,1,2,3   - [5] 1_1_1_1_1

int LadderRank(std::map<char,int>& hand){
    switch(hand.size()){
        case 1 : return 7;
        case 2 : {
            if(std::find_if(hand.cbegin(),hand.cend() , 
            [](std::pair<char,int> key_val) { return key_val.second == 4;}) != hand.cend()){
                return 6;
            }
            else return 5;
        }
        case 3 : {
            if(std::find_if(hand.cbegin(),hand.cend() , 
            [](std::pair<char,int> key_val) { return key_val.second == 3;}) != hand.cend()){
                return 4;
            }
            else return 3;
        }
        case 4 : return 2;
        case 5 : return 1;
    }
    return 0;
}
int LadderRankJoker(std::map<char,int>& hand){
    switch(hand.size()){
        case 1 : return 7; // [AAAAA]
        case 2 : {
            if(hand.find('J') != hand.end()) return 7;
            if(std::find_if(hand.cbegin(),hand.cend() , 
            [](std::pair<char,int> key_val) { return key_val.second == 4;}) != hand.cend()){
                return 6;   // [AAAA][K]
            }
            else return 5;  // [AAA][KK]
        }
        case 3 : {
            if(std::find_if(hand.cbegin(),hand.cend() , 
            [](std::pair<char,int> key_val) { return key_val.second == 3;}) != hand.cend()){
                if(hand.find('J') != hand.end()) return 6;
                return 4;   // [AAA][K][Q]
            }
            else {
                auto Js = std::count_if( hand.begin(), hand.end(), [](std::pair<char,int> key_val){
                    return key_val.first == 'J';
                });
                if( Js == 1 ) return 5;
                else if( Js == 2 ) return 6;
                return 3;
                //return Js == 0 ? 3 : (Js == 1) ? 5 : 6; // [AA][KK][Q]
            }  
        }
        case 4 : {
            if(hand.find('J') != hand.end()) return 4;
            return 2; // [AA][K][Q][J]
        }
        case 5 : {
            if(hand.find('J') != hand.end()) return 2;
            return 1; // [A][K][Q][J][T]
        }

    }
    return 0;
}


// A K Q J T 9 -> 2
bool customCompare(char a, char b) {
    const std::string order = "AKQJT98765432";
    size_t posA = order.find(a);
    size_t posB = order.find(b);
    return posA > posB;
}

bool customCompareJoker(char a, char b) {
    const std::string order = "AKQT98765432J";
    size_t posA = order.find(a);
    size_t posB = order.find(b);
    return posA > posB;
}


int one(std::vector<Player>& datos) {
    uint64_t count = 0;
    std::map<char, int> hand;
    std::map<int, std::vector<Player>> ladderRanks;
    for (auto& player : datos) {
        for (auto& card : player.cards) {
            hand[card]++;
        }
        ladderRanks[LadderRank(hand)].push_back(player); 
        hand.clear();
    }
    uint64_t playerCount = datos.size();
    for ( auto [tempRank, players ] : rv::reverse(ladderRanks)){
        if( players.size() != 1){
            std::sort(players.begin(),players.end() , [](Player& a, Player& b){
                return !std::lexicographical_compare(a.cards.begin(),a.cards.end(),
                b.cards.begin(),b.cards.end() , customCompare);
            });
        }

        for (auto& player : players) { count += (player.bid * playerCount--); } 
    }
    return count;
}

int two(std::vector<Player>& datos) {
    uint64_t count = 0;
    std::map<char, int> hand;
    std::map<int, std::vector<Player>> ladderRanks;
    for (auto& player : datos) {
        for (auto& card : player.cards) {
            hand[card]++;
        }
        ladderRanks[LadderRankJoker(hand)].push_back(player); 
        hand.clear();
    }
    uint64_t playerCount = datos.size();
    for ( auto [tempRank, players ] : rv::reverse(ladderRanks)){
        if( players.size() != 1){
            std::sort(players.begin(),players.end() , [](Player& a, Player& b){
                return !std::lexicographical_compare(a.cards.begin(),a.cards.end(),
                b.cards.begin(),b.cards.end() , customCompareJoker);
            });
        }

        for (auto& player : players) { 
            count += (player.bid * playerCount--); 
        } 
    }
    return count;
}

int main()
{
    auto v = read(path);
    auto datos = parse(v);
    std::cout << one(datos) << "\n";
    std::cout << two(datos) << "\n";
}