use std::fs;

fn main() {
    let path = "../AOCinput";
    part1(path);
    part2(path);
}

fn part1(path: &str) {
    let input = fs::read_to_string(path).expect("Catn read file");

    let mut n: u32 = 0;
    loop {
        let actStr = format!("{input}{n}");
        let digest = md5::compute(actStr);
        let res = format!("{:x}", digest);

        if res.starts_with("00000") {break;} else {n+=1;}
    }
    println!("Res1: {n}");
}

fn part2(path: &str) {
    let input = fs::read_to_string(path).expect("Catn read file");

    let mut n: u32 = 0;
    loop {
        let actStr = format!("{input}{n}");
        let digest = md5::compute(actStr);
        let res = format!("{:x}", digest);

        if res.starts_with("000000") {break;} else {n+=1;}
    }
    println!("Res2: {n}");
}