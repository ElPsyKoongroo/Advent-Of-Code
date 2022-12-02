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
    let mut max_cal = 0;
    for group in data.split("\n") {
        if group.trim().is_empty() {
            if actual_cal > max_cal { max_cal = actual_cal }
            actual_cal = 0;
        } else {
            match group.trim().parse::<i32>() {
                Ok(e) => actual_cal += e,
                Err(_) => {}
            }
        }
    }

    println!("{}", max_cal)
}
