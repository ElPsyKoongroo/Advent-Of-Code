#include <iostream>
#include <algorithm>
#include <vector>
#include <utility>
#include <fstream>
#include <numeric>



/*

A/X -> Rock     -> 1
B/Y -> Paper    -> 2
C/Z -> Scissors -> 3

0 -> Lose
3 -> Draw
6 -> Win

*/


std::vector<std::pair<char, char>> ParseInput(){
    std::fstream file("../AOCinput.txt", std::ios::in);
    std::vector<std::pair<char,char>> vec;
    
    file.seekg(std::ios::beg);

    while(true)
    {
        char x, y;
        file >> x >> y;
        if(file.eof()) break;
        //std::cout << x << ":" << y << std::endl;
        vec.push_back(std::make_pair(x,y));
    }
    file.close();
    return vec;
}

void parte1(std::vector<std::pair<char,char>> vec){
    uint64_t total = 0;
    
    auto next = [](const int& i) {return (i+1)%3;};

    auto lambda = [&](const std::pair<char,char>& pair)
    {
        int val0 = pair.first - 'A';
        int val1 = pair.second - 'X';

        total += val1 + 1;
        if(val0 == val1)
            total += 3;
        else if(val1 == next(val0))
            total += 6;
        return;
    };

    std::for_each(vec.cbegin(), vec.cend(),lambda);

    std::cout << total << std::endl;
}

void parte2(std::vector<std::pair<char,char>> vec){
    uint64_t total = 0;
    
    auto next = [](const int& i) {return (i+1)%3;};

    auto lambda = [&](const std::pair<char,char>& pair)
    {
        int val0 = pair.first - 'A';
        int result = pair.second - 'X';

        total += (result * 3) + 1;

        if(result == 1)
            total += val0;
        else if(result == 2)
            total += next(val0);
        else
            total += next(next(val0));
        return;
    };

    std::for_each(vec.cbegin(), vec.cend(),lambda);

    std::cout << total << std::endl;
}

int main(){
    auto input = ParseInput();
    parte1(input);
    parte2(input);
}

