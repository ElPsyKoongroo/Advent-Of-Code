use std::{io::Read, collections::VecDeque};

fn read_data(file_name: &str) -> String {
    let mut reader = std::io::BufReader::new(std::fs::File::open(file_name).unwrap());
    let mut data = String::new();
    reader.read_to_string(&mut data).unwrap();
    data
}

fn part_1(data: &str){
    let mut readed: VecDeque<char> = VecDeque::new();
    let mut count = 0;
    for c in data.chars() {
        count += 1;
        if !readed.contains(&c){readed.push_back(c);}
        else {
            while readed.pop_front().unwrap() != c {}
            readed.push_back(c);
        }
        if readed.len() == 4 {break}
    }

    println!("{}, {:?}", count, readed);
}

fn part_2(data: &str){
    let mut readed: VecDeque<char> = VecDeque::new();
    let mut count = 0;
    for c in data.chars() {
        count += 1;
        if !readed.contains(&c){readed.push_back(c);}
        else {
            while readed.pop_front().unwrap() != c {}
            readed.push_back(c);
        }
        if readed.len() == 14 {break}
    }

    println!("{}, {:?}", count, readed);
}
fn main() {
    let data = read_data("src/data.txt");
    part_2(&data);
}
