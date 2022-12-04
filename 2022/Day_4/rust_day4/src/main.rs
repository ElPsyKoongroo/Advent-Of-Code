use std::io::Read;

fn read_data(file_name: &str) -> String {
    let mut reader = std::io::BufReader::new(std::fs::File::open(file_name).unwrap());
    let mut data = String::new();
    reader.read_to_string(&mut data).unwrap();
    data
}

struct ElfRange {
    lower_limit: i32,
    upper_limit: i32
}

impl ElfRange {
    pub fn contains(&self, other: &ElfRange) -> bool {
        self.lower_limit <= other.lower_limit 
        && self.upper_limit >= other.upper_limit
    }

    pub fn overlaps(&self, other: &ElfRange) -> bool {
        self.upper_limit >= other.lower_limit 
        && self.lower_limit <= other.upper_limit
        
    }
}

impl std::convert::From<&str> for ElfRange {
    fn from(input: &str) -> ElfRange {
        let (low, up) = input.split_once("-").unwrap();
        ElfRange { 
            lower_limit: low.parse().unwrap_or(0), 
            upper_limit: up.parse().unwrap_or(0) 
        }
    }
}

fn part1(data: &str){
    let mut n = 0;
    for line in data.split("\n") {
        let (elf1, elf2) = line.split_once(",").unwrap();

        let range1: ElfRange = elf1.into();
        let range2: ElfRange = elf2.into();

        if range1.contains(&range2) || range2.contains(&range1) {
            n += 1;
        }
    }

    println!("{n}");
}


fn part2(data: &str) {
    let mut n = 0;
    for line in data.split("\n") {
        let (elf1, elf2) = line.split_once(",").unwrap();

        let range1: ElfRange = elf1.into();
        let range2: ElfRange = elf2.into();

        if range1.overlaps(&range2) || range2.overlaps(&range1) {
            n += 1;
        }
    }

    println!("{n}");
}

fn main() {
    let data = read_data("src/test.txt");
    part2(&data);
}
