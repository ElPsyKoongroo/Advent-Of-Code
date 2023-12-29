use std::fs;

fn main() {
    let path: &str = "../AOCinput";
    part1(path);
    part2(path);
}

fn part1(path: &str) {
    let input = fs::read_to_string(path).expect("Cant read file");
    let up = input.chars().filter(|&x| x == '(').count();
    let down = input.chars().filter(|&x| x == ')').count();
    println!("Res1: {}", up - down);
}

fn part2(path: &str) {
    let input = fs::read_to_string(path).expect("Cant read file");
    let ammount = 
        input.chars().scan(0, |a, x| {*a += if x == '(' {1} else {-1}; Some(*a)}).position(|x| x < 0).unwrap()+1;
    println!("Res2: {ammount}");
}