use std::{fs, collections::HashMap};

fn main() {
    let path = "../AOCinput";
    part1(path);
    part2(path);
}


fn part1(path: &str) {
    let input_file = fs::read_to_string(path).expect("Cant read file");
    let input = input_file.lines();

    let result: u32 = input.into_iter().map(|line|
        line.chars().filter(|c| c.is_digit(10)).collect::<Vec<char>>()
    ).map(|l| 
        l.first().unwrap().to_digit(10).unwrap()*10 + l.last().unwrap().to_digit(10).unwrap()
    ).sum();

    println!("Res1: {result}");
}

fn part2(path: &str) {
    let input = fs::read_to_string(path).expect("Cant read file");

    let mut total = 0;

    let map: HashMap<&str, u32> = HashMap::from([
        ("1", 1), ("one", 1),
        ("2", 2), ("two", 2),
        ("3", 3), ("three", 3),
        ("4", 4), ("four", 4),
        ("5", 5), ("five", 5),
        ("6", 6), ("six", 6),
        ("7", 7), ("seven", 7),
        ("8", 8), ("eight", 8),
        ("9", 9), ("nine", 9), 
    ]);

    for line in input.lines() {
        let mut first: u32 = 0;
        let mut last: u32 = 0;

        for i in 0..line.len() {
            let str = &line[i..];

            for (key, val) in map.iter() {
                if str.starts_with(key) {
                    if first == 0 { first = *val;}
                    last = *val;
                    break;
                }
            }
        }

        total += first*10 + last;
    }

    println!("Res2: {total}");
}