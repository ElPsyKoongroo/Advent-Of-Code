use std::io::Read;
use std::collections::VecDeque;

fn read_data(file_name: &str) -> String {
    let mut reader = std::io::BufReader::new(std::fs::File::open(file_name).unwrap());
    let mut data = String::new();
    reader.read_to_string(&mut data).unwrap();
    data
}

fn part_1(data: &str){
    let mut crates: Vec<VecDeque<char>> = Vec::new();
    for _ in 0..9 {
        crates.push(VecDeque::new());
    }


    for line in data.split("\n").take(8){        
        let mut i = 1;
        let chars = line.chars().collect::<Vec<char>>();
        
        for n in 0..9 {
            if !chars[i].is_whitespace() {
                crates[n].push_front(chars[i]);
            }
            i += 4;
        }
    }
    
    for order in data.split("\n").skip(10) {
        let words = order.split(" ").collect::<Vec<&str>>();
        let cuantity = words[1].parse::<i32>().unwrap_or(0);
        let from = words[3].parse::<usize>().unwrap_or(0);
        let to = words[5].parse::<usize>().unwrap_or(0);
    
        //println!("move {} from {} to {}", cuantity, from, to);

        for _ in 0..cuantity {
            let element = crates[from-1].pop_back().unwrap();
            crates[to-1].push_back(element);
        }
    }
    
    for depot in crates.iter_mut() {
        let last_element = depot.pop_back().unwrap();
        print!("{:?}", last_element);
    }

}



fn main() {
    let data = read_data("src/data.txt");

}
