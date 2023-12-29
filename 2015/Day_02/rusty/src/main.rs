use std::{fs, cmp::min};
use regex::Regex;

fn main() {
    let path = "../AOCinput";
    part1(path);
    part2(path);
}

fn part1(path: &str) {
    let input = fs::read_to_string(path).unwrap();
    
    let re = Regex::new(r"(?m)^(\d+)x(\d+)x(\d+)$").unwrap();

    let total: u64 = re.captures_iter(&input).map(|l| {
        let [l, w, h] = l.extract().1.map(|n| n.parse::<u64>().unwrap());

        let lw = l*w;
        let wh = h*w;
        let hl = l*h;
        let min = min(min(lw, hl), wh);
        2*(lw+wh+hl) + min
    }).sum();

    println!("Res1: {total}");
}

fn part2(path: &str) {
    let input = fs::read_to_string(path).unwrap();
    
    let re = Regex::new(r"(?m)^(\d+)x(\d+)x(\d+)$").unwrap();

    let total: u64 = re.captures_iter(&input).map(|l| {
        let mut sizes: [u64; 3] = l.extract().1.map(|n| n.parse::<u64>().unwrap());
        sizes.sort_unstable();

        let min_face = sizes.iter().take(2).sum::<u64>()*2;
        let volume = sizes.into_iter().fold::<u64,_>(1, |a, x| a*x);

        min_face + volume
    }).sum();

    println!("Res2: {total}");
}
