#include <iostream>
#include <algorithm>
#include <vector>
#include <utility>
#include <fstream>
#include <numeric>
#include <string>
#include <set>

std::vector<std::vector<int>> ParseInput(){
    std::fstream file("../AOCinput.txt", std::ios::in);
    std::vector<std::vector<int>> vec;
    
    file.seekg(std::ios::beg);

    while(!file.eof())
    {
        std::string s;
        file >> s;
        std::vector<int> st;
        auto lambda = [&](const char& x){
            st.push_back(isupper(x) ? x-'A'+27 : x-'a'+1);
        };
        std::for_each(s.cbegin(),s.cend(),lambda);

        vec.push_back(st);
    }
    file.close();
    return vec;
}



void parte1(std::vector<std::vector<int>> vec){
    uint16_t total = 0;


    auto lambda = [&](std::vector<int> st){
        uint64_t size = st.size()/2;

        int res;
        std::sort(st.begin(), st.begin() + size);
        std::sort(st.begin() + size, st.end());
        std::set_intersection(st.cbegin(), st.cbegin() + size, st.cbegin() + size,st.cend(), &res);
        total += res;
    };
    
    std::for_each(vec.begin(), vec.end(),lambda);

    std::cout << total << std::endl;

}


void parte2(std::vector<std::vector<int>> vec){
    uint16_t total = 0;

    for(int i = 0; i < vec.size(); ++i)
    {
        std::sort( vec[i].begin(), vec[i].end() );
        vec[i].erase( std::unique( vec[i].begin(), vec[i].end() ), vec[i].end() );
    }

    // 1 2 2 3 3 4 5    ->   1 2 3 4 5 2 3 

    for(int i = 0; i < vec.size(); i+=3)
    {
        std::vector<int> first_inter;
        int finalInter = 0;

        std::set_intersection(vec[i].begin(), vec[i].end(), vec[i+1].begin(),
                               vec[i+1].end(), std::back_inserter(first_inter));


        std::sort(first_inter.begin(), first_inter.end());

        std::set_intersection(first_inter.begin(), first_inter.end(), vec[i+2].begin(), 
                               vec[i+2].end(), &finalInter);

        total += finalInter;
    }

    std::cout << total << std::endl;

}

int main(){
    auto vec = ParseInput();

    //parte1(vec);
    parte2(vec);
}