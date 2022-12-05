use std::io::Read;
use std::collections::HashMap;

const ROCK_POINTS: u32 = 1;
const PAPER_POINTS: u32 = 2;
const SCISSORS_POINTS: u32 = 3;

const LOST: u32 = 0;
const DRAW: u32 = 3;
const WIN: u32 = 6;

fn read_data() -> String {
    let mut reader = std::io::BufReader::new(std::fs::File::open("src/data.txt").unwrap());
    let mut data = String::new();
    reader.read_to_string(&mut data).unwrap();
    data
}


fn main() {
    let games = read_data();
    let mut points = 0;
    let games_possibilities: HashMap<(&str, &str), u32> = HashMap::from([
        (("A", "X"), LOST + SCISSORS_POINTS),
        (("A", "Y"), DRAW + ROCK_POINTS),
        (("A", "Z"), WIN + PAPER_POINTS),
        (("B", "X"), LOST + ROCK_POINTS),
        (("B", "Y"), DRAW + PAPER_POINTS),
        (("B", "Z"), WIN + SCISSORS_POINTS),
        (("C", "X"), LOST + PAPER_POINTS),
        (("C", "Y"), DRAW + SCISSORS_POINTS),
        (("C", "Z"), WIN + ROCK_POINTS),
    ]);
    

    for game in games.split("\n") {
        points += games_possibilities.get(&game.split_once(" ").unwrap()).unwrap_or_else(|| {println!("WTF"); &0});
    }

    println!("{points}")
}
