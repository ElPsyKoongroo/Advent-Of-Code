use std::{fs, collections::HashSet};

fn main() {
    let path = "../AOCinput";
    part1(path);
    part2(path);
}


fn part1(path: &str) {
    let input = fs::read_to_string(path).expect("Cant read file");

    let mut positions = HashSet::new();

    let mut x: i32 = 0;
    let mut y: i32 = 0;

    positions.insert((0,0));

    for chr in input.chars() {
        match chr {
            '^' => y+=1,
            'v' => y-=1,
            '<' => x-=1,
            '>' => x+=1,
            _ => unreachable!()
        }
        positions.insert((x,y));
    }
    println!("Res1: {}", positions.len());
}

fn part2(path: &str) {
    let input = fs::read_to_string(path).expect("Cant read file");

    let mut positions = HashSet::new();

    let mut x: [i32; 2] = [0,0];
    let mut y: [i32; 2] = [0,0];

    positions.insert((0,0));

    let mut santa = true;

    for chr in input.chars() {
        let idx = if santa {0} else {1};
        santa = !santa;
        match chr {
            '^' => y[idx]+=1,
            'v' => y[idx]-=1,
            '<' => x[idx]-=1,
            '>' => x[idx]+=1,
            _ => unreachable!()
        }
        positions.insert((x[idx],y[idx]));
    }
    println!("Res1: {}", positions.len());
}