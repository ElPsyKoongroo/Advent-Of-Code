use std::io::Read;

fn read_data() -> String {
    let mut reader = std::io::BufReader::new(std::fs::File::open("src/data.txt").unwrap());
    let mut data = String::new();
    reader.read_to_string(&mut data).unwrap();
    data
}

fn main() {
    let data = read_data();
    let mut profit = 0;
    for items in data.split("\n") {
        let (first_half, second_half) = items.split_at(items.len()/2);
        let common_element = &first_half
        .chars()
        .filter(|c| second_half.contains(*c))
        .collect::<Vec<char>>()
        .first()
        .unwrap()
        .clone();
        
        profit += match common_element.is_ascii_lowercase() {
            true  => (*common_element as u32) - ('a' as u32) + 1,
            false => (*common_element as u32) - ('A' as u32) + 27,
        }
    }

    println!("{profit}");
}
