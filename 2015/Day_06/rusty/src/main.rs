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
}

fn parse_input(input: &str) -> Vec<Instruction> {
    fs::read_to_string(input).unwrap()
    .lines().map(parse_line).collect()
}

fn parse_line(line: &str) -> Instruction {
    let (rem, isToggle) = parse_toogle(line).unwrap();

    let turn_on = if !isToggle {
        let (rem, turn) = alt((
            value(true, tag::<&str, &str, nom::error::Error<_>>("on ")),
            value(false, tag("off ")),
          ))(rem).unwrap();
          turn
    } else {
        false
    };

    let (rem, r1) = parse_range(rem);

    let (rem, _) = tag::<&str, &str, nom::error::Error<_>>(" through ")(rem).unwrap();

    let (rem, r2) = parse_range(rem); 

    assert!(rem.len() == 0);

    if isToggle {
        Instruction::Toggle(r1, r2)
    } else if turn_on {
        Instruction::On(r1, r2)
    } else {
        Instruction::Off(r1, r2)
    }

}

fn parse_range(input: &str) -> (&str, RangeInclusive<u16>) {
    let (rem, range) = 
        separated_pair(
            map_res(digit1::<&str, nom::error::Error<_>>, str::parse::<u16>),
            tag(","),
            map_res(digit1::<&str, nom::error::Error<_>>, str::parse::<u16>)
        )(input).unwrap();
    (rem, range.0..=range.1)
}

fn parse_toogle(input: &str) -> IResult<&str, bool> {
    alt((
      value(true, tag("toggle ")),
      value(false, tag("turn ")),
    ))(input)
}

fn part1(path: &str) {

}


fn part2(path: &str) {
    
}