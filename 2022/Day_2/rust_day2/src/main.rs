use std::io::Read;

const ROCK_POINTS: u32 = 1;
const PAPER_POINTS: u32 = 2;
const SCISSORS_POINTS: u32 = 3;

const LOST: u32 = 0;
const DRAW: u32 = 3;
const WIN: u32 = 6;

fn read_data() -> String {
    let mut reader = std::io::BufReader::new(std::fs::File::open("src/data.txt").unwrap());
    let mut data = String::new();
    reader.read_to_string(&mut data);
    data
}

fn main() {
    let games = read_data();
    let mut points = 0;
    for game in games.split("\n") {
        points += match game.split_once(" ").unwrap() {
            ("A", "X") => DRAW + ROCK_POINTS,       
            ("A", "Y") => WIN + PAPER_POINTS,  
            ("A", "Z") => LOST + SCISSORS_POINTS,  

            ("B", "X") => LOST + ROCK_POINTS,
            ("B", "Y") => DRAW + PAPER_POINTS,
            ("B", "Z") => WIN + SCISSORS_POINTS,

            ("C", "X") => WIN + ROCK_POINTS,
            ("C", "Y") => LOST + PAPER_POINTS,
            ("C", "Z") => DRAW + SCISSORS_POINTS,
            _ => {
                println!("WTF");
                0
            }
        }
    }

    println!("{points}")
}
