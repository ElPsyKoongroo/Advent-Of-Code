use std::collections::HashMap;
use std::collections::VecDeque;
use std::io::Read;

fn read_data(file_name: &str) -> String {
    let mut reader = std::io::BufReader::new(std::fs::File::open(file_name).unwrap());
    let mut data = String::new();
    reader.read_to_string(&mut data).unwrap();
    data
}

#[derive(Hash, PartialEq, Eq, Clone)]
struct Folder {
    name: String,
    total_size: u32,
}

impl Folder {
    pub fn new(name: &str, size: u32) -> Folder {
        Folder {
            name: name.to_owned(),
            total_size: size,
        }
    }
}

fn part_1(data: &str) -> (u32, u32) {
    let mut visited_folders: HashMap<Folder, bool> = HashMap::new();
    let mut folders: VecDeque<Folder> = VecDeque::new();
    let mut actual_dir = Folder::new("/", 0);
    let mut total_size = 0;
    let mut disk_size = 0;

    for line in data.split("\n") {
        match line {
            line if line == "$ cd .." => {
                if visited_folders.get(&actual_dir).is_none() {
                    if actual_dir.total_size <= 100_000{
                        total_size += actual_dir.total_size
                    }
                }
                let aux = actual_dir.total_size;
                visited_folders.insert(actual_dir, true);
                actual_dir = folders.pop_back().unwrap();
                actual_dir.total_size += aux;
            }
            line if line.contains("$ cd") => {
                folders.push_back(actual_dir);
                let args = line.split(" ").collect::<Vec<&str>>();
                actual_dir = Folder::new(args[2], 0);
            }
            _ => {
                let (arg, _) = line.split_once(" ").unwrap();
                if visited_folders.get(&actual_dir).is_none() {
                    if let Ok(size) = arg.parse::<u32>() {
                        actual_dir.total_size += size;
                        disk_size += size;
                    }
                }
            }
        }
    }
    println!("{total_size}");
    (total_size, disk_size)
}

const MAX_SIZE: u32 = 70000000;
const REQ_SIZE: u32 = 30000000;

fn part_2(data: &str, total_size: u32){
    let mut visited_folders: HashMap<Folder, bool> = HashMap::new();
    let mut folders: VecDeque<Folder> = VecDeque::new();
    let mut actual_dir = Folder::new("/", 0);
    let mut dir_removed = u32::MAX;
    
    let free_space = MAX_SIZE - total_size;

    for line in data.split("\n") {
        match line {
            line if line == "$ cd .." => {
                if free_space + actual_dir.total_size > REQ_SIZE && actual_dir.total_size < dir_removed {
                    dir_removed = actual_dir.total_size;
                }        
                let aux = actual_dir.total_size;
                visited_folders.insert(actual_dir, true);
                actual_dir = folders.pop_back().unwrap();
                actual_dir.total_size += aux;
            }
            line if line.contains("$ cd") => {
                folders.push_back(actual_dir);
                let args = line.split(" ").collect::<Vec<&str>>();
                actual_dir = Folder::new(args[2], 0);
            }
            _ => {
                let (arg, _) = line.split_once(" ").unwrap();
                if visited_folders.get(&actual_dir).is_none() {
                    if let Ok(size) = arg.parse::<u32>() {
                        actual_dir.total_size += size;
                    }
                }
            }
        }
    }
    println!("{dir_removed}");
}

fn main() {
    let data = read_data("src/data.txt");
    let (_, used) = part_1(&data);
    part_2(&data, used);
}
