use std::{ops::RangeInclusive, fs};

use nom::{
    IResult,
    bytes::complete::tag, 
    branch::alt, 
    combinator::{value, map_res}, 
    sequence::separated_pair, 
    character::complete::digit1
};

enum Instruction {
    On(RangeInclusive<u16>, RangeInclusive<u16>),
    Off(RangeInclusive<u16>, RangeInclusive<u16>),
    Toggle(RangeInclusive<u16>, RangeInclusive<u16>),
}

fn main() {
    let path = "../AOCinput";
    part1(path);
    part2(path);
}

fn parse_input(input: &str) -> Vec<Instruction> {
    fs::read_to_string(input).unwrap()
    .lines().map(parse_line).collect()
}

fn parse_line(line: &str) -> Instruction {
    let (mut rem, is_toggle) = parse_toogle(line).unwrap();

    let turn_on = if !is_toggle {
        let (re2, turn) = alt((
            value(true, tag::<&str, &str, nom::error::Error<_>>("on ")),
            value(false, tag("off ")),
          ))(rem).unwrap();
          rem = re2;
          turn
    } else {
        false
    };

    let (rem, r1) = parse_range(rem);

    let (rem, _) = tag::<&str, &str, nom::error::Error<_>>(" through ")(rem).unwrap();

    let (rem, r2) = parse_range(rem); 

    assert!(rem.len() == 0);

    let (r1, r2) = ((r1.0..=r2.0), (r1.1..=r2.1));

    if is_toggle {
        Instruction::Toggle(r1, r2)
    } else if turn_on {
        Instruction::On(r1, r2)
    } else {
        Instruction::Off(r1, r2)
    }

}

fn parse_range(input: &str) -> (&str, (u16, u16)) {
    let (rem, range) = 
        separated_pair(
            map_res(digit1::<&str, nom::error::Error<_>>, str::parse::<u16>),
            tag(","),
            map_res(digit1::<&str, nom::error::Error<_>>, str::parse::<u16>)
        )(input).unwrap();
    (rem, (range.0,range.1))
}

fn parse_toogle(input: &str) -> IResult<&str, bool> {
    alt((
      value(true, tag("toggle ")),
      value(false, tag("turn ")),
    ))(input)
}

fn part1(path: &str) {
    let input = parse_input(path);

    let mut state = [[0u16; 1_000]; 1_000];

    input.into_iter().for_each(|instruction| {
        match instruction {
            Instruction::On(r1, r2) => {
                for i in r1 {
                    for j in r2.clone() {
                        state[i as usize][j as usize] = 1;
                    }
                }
            },
            Instruction::Off(r1, r2) => {
                for i in r1 {
                    for j in r2.clone() {
                        state[i as usize][j as usize] = 0;
                    }
                }
            },
            Instruction::Toggle(r1, r2) => {
                for i in r1 {
                    for j in r2.clone() {
                        if state[i as usize][j as usize] == 0 {
                            state[i as usize][j as usize] = 1;
                        } else {
                            state[i as usize][j as usize] = 0;
                        }
                    }
                }
            },
        }        
    });

    let total: u32 = state.into_iter().map(|arr| arr.into_iter().sum::<u16>() as u32).sum();

    println!("Res1: {total}");
}


fn part2(path: &str) {
    let input = parse_input(path);

    let mut state = [[0u32; 1_000]; 1_000];

    input.into_iter().for_each(|instruction| {
        match instruction {
            Instruction::On(r1, r2) => {
                for i in r1 {
                    for j in r2.clone() {
                        state[i as usize][j as usize] += 1;
                    }
                }
            },
            Instruction::Off(r1, r2) => {
                for i in r1 {
                    for j in r2.clone() {
                        let var = &mut state[i as usize][j as usize];
                        *var = var.saturating_sub(1);
                    }
                }
            },
            Instruction::Toggle(r1, r2) => {
                for i in r1 {
                    for j in r2.clone() {
                        state[i as usize][j as usize] += 2;
                    }
                }
            },
        }        
    });

    let total: u64 = state.into_iter().map(|arr| arr.into_iter().sum::<u32>() as u64).sum();

    println!("Res1: {total}");
}