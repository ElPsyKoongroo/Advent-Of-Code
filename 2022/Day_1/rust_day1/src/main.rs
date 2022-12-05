use std::io::Read;

fn read_data() -> String {
    let mut reader = std::io::BufReader::new(std::fs::File::open("src/data.txt").unwrap());
    let mut data = String::new();
    reader.read_to_string(&mut data);
    data

}

fn main() {
    let data = read_data();

    let mut actual_cal = 0;
    let mut groups = Vec::new();

    for group in data.split("\n") {
        if group.trim().is_empty() {
            groups.push(actual_cal);
            actual_cal = 0;
        } else {
            match group.trim().parse::<i32>() {
                Ok(e) => actual_cal += e,
                Err(_) => {}
            }
        }
    }
    groups.sort();
    let mut total = 0;
    for group in groups.into_iter().rev().take(3){
        total += group;
    }
    println!("{}", total)
}
