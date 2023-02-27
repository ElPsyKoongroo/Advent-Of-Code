#include <bits/stdc++.h>


std::vector<std::vector<int>> get_input(){

    std::fstream file("../AOCinput.txt", std::ios::in);
    std::vector<std::vector<int>> vec;
    vec.push_back(std::vector<int>());
    int index = 0;
    std::string str;
    while(std::getline (file , str, '\n')){ 
        if(str.size() > 0){
            int number = stoi(str);
            vec[index].push_back(number);
        }
        else{
            index++;
            vec.push_back(std::vector<int>());
        }
    }
    return vec;
}


int part1(std::vector<std::vector<int>> vec){

    std::vector<int> maxs;

    std::for_each(vec.begin(),vec.end(),[&maxs](std::vector<int> v){
        maxs.push_back(std::accumulate(v.begin(),v.end(),0));
    });
    
    return *std::max_element(maxs.cbegin(),maxs.cend());
}

int part2(std::vector<std::vector<int>> vec){

    std::vector<int> maxs;

    std::for_each(vec.begin(),vec.end(),[&maxs](std::vector<int> v){
        maxs.push_back(std::accumulate(v.begin(),v.end(),0));
    });
    
    std::sort(maxs.rbegin(),maxs.rend());

    return maxs[0] + maxs[1] + maxs[2];
}


int main(){
    std::vector<std::vector<int>> vec = get_input();
    std::cout << part1(vec) << std::endl;
    std::cout << part2(vec) << std::endl;
}