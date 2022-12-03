use std::io::Read;

fn read_data() -> String {
    let mut reader = std::io::BufReader::new(std::fs::File::open("src/data.txt").unwrap());
    let mut data = String::new();
    reader.read_to_string(&mut data).unwrap();
    data
}

fn main() {
    let data = read_data();
    let mut i = 0;
    let mut organized_data = Vec::new();
    let mut profit = 0;

    for line in data.split("\n") {
        organized_data.push(line);
        i += 1;

        if i % 3 == 0 {
            let index = organized_data[0]
                .chars()
                .position(|c| {
                    organized_data[1].chars().any(|a| a == c)
                        && organized_data[2].chars().any(|a| a == c)
                })
                .unwrap();

            let a = organized_data[0].chars().nth(index).unwrap();

            profit += match a.is_ascii_lowercase() {
                true => (a as u32) - ('a' as u32) + 1,
                false => (a as u32) - ('A' as u32) + 27,
            };
            organized_data.clear();
        }
    }

    println!("{profit}");
}
