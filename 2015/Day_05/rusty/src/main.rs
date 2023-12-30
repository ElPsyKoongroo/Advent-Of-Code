use std::fs;
use regex::Regex;

fn main() {
    let path = "../AOCinput";    
    part1(path);
    part2(path);
}

fn part1(path: &str) {
    let input = fs::read_to_string(path).unwrap();

    let re_vowel = Regex::new(r"[aeiou]").unwrap();
    let re_bad = Regex::new(r"ab|cd|pq|xy").unwrap();

    let mut total: u32 = 0;
    for line in input.split("\n") {
        let vowels = re_vowel.find_iter(line).count() >= 3;
        
        let dup = (1..line.len()).map(|i| {
            line.chars().nth(i-1).unwrap()==line.chars().nth(i).unwrap()
        }).any(|x| x);

        let bad = re_bad.is_match(line);

        if vowels && dup && !bad {
            total+=1;
        }
    }
    println!("Res1: {total}");
}

fn part2(path: &str) {
    let input = fs::read_to_string(path).unwrap();

    let mut total: u32 = 0;
    for line in input.split("\n") {

        //Duplicated pair 
        let pairs: Vec<(char, char, i32)> = (1..line.len()).map(|i| {
            (line.chars().nth(i-1).unwrap(),line.chars().nth(i).unwrap(), i as i32)
        }).collect();

        let dup = 
            pairs.iter()
            .any(|(x,y,i)| 
                pairs.iter().any(|(a,b,i2)| 
                    *a==*x && *b == *y && (*i-*i2).abs() > 1
                )
            );

        //Separated pair
        let pair = (2..line.len()).map(|i| {
            line.chars().nth(i-2).unwrap()==line.chars().nth(i).unwrap()
        }).any(|x| x);

        if dup && pair {
            total+=1;
        }
    }
    println!("Res2: {total}");
}