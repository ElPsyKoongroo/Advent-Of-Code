const std = @import("std");

pub fn main() !void {
    var gpa = std.heap.GeneralPurposeAllocator(.{}){};
    defer _ = gpa.deinit();
    const allocator = gpa.allocator();

    // Open the file
    const file = try std.fs.cwd().openFile("../AOCinput", .{});
    defer file.close();

    const file_buffer = try file.readToEndAlloc(allocator, std.math.maxInt(usize));
    defer allocator.free(file_buffer);

    Answer1(file_buffer);
    Answer2(file_buffer);
}

// byr (Birth Year)
// iyr (Issue Year)
// eyr (Expiration Year)
// hgt (Height)
// hcl (Hair Color)
// ecl (Eye Color)
// pid (Passport ID)
// cid (Country ID)

const Passport = struct {
    byr: bool = false,
    iyr: bool = false,
    eyr: bool = false,
    hgt: bool = false,
    hcl: bool = false,
    ecl: bool = false,
    pid: bool = false,
    cid: bool = false,

    pub fn isValid(self: Passport) bool {
        return 
            self.byr and
            self.iyr and
            self.eyr and
            self.hgt and
            self.hcl and
            self.ecl and
            self.pid;
    }
};

fn Answer1(file: []const u8) void {

    const map = std.ComptimeStringMap(u8, .{
        .{"byr", 0},
        .{"iyr", 1},
        .{"eyr", 2},
        .{"hgt", 3},
        .{"hcl", 4},
        .{"ecl", 5},
        .{"pid", 6},
        .{"cid", 7},
    });

    var it = std.mem.tokenizeSequence(u8, file, "\n\n");
    var counter: u32 = 0;
    while (it.next()) |passport| {
        var lines = std.mem.tokenizeSequence(u8, passport, "\n");
        var actualPass = Passport{};

        while (lines.next()) |line| {
            var infos = std.mem.tokenizeSequence(u8, line, " ");

            while(infos.next()) |info| {
                const value = map.get(info[0..3]).?;

                switch (value) {
                    0 => actualPass.byr=true,
                    1 => actualPass.iyr=true,
                    2 => actualPass.eyr=true,
                    3 => actualPass.hgt=true,
                    4 => actualPass.hcl=true,
                    5 => actualPass.ecl=true,
                    6 => actualPass.pid=true,
                    7 => actualPass.cid=true,
                    else => unreachable
                }
            }
        }
        if(actualPass.isValid()) counter+=1;
    }
    std.log.info("{d}", .{counter});
}

fn Answer2(file: []const u8) void {

    const map = std.ComptimeStringMap(u32, .{
        .{"byr", 0},
        .{"iyr", 1},
        .{"eyr", 2},
        .{"hgt", 3},
        .{"hcl", 4},
        .{"ecl", 5},
        .{"pid", 6},
        .{"cid", 7},
    });

    var passports = std.mem.tokenizeSequence(u8, file, "\n\n");
    var counter: u32 = 0;
    while (passports.next()) |passport| {
        var lines = std.mem.tokenizeSequence(u8, passport, "\n");
        var actualPass = Passport{};

        while (lines.next()) |line| {
            var infos = std.mem.tokenizeSequence(u8, line, " ");

            while(infos.next()) |info| {
                const value: u32 = map.get(info[0..3]).?;
                
                switch (value) {
                    0 => {
                        const byr = std.fmt.parseInt(u32, info[4..8], 10) catch unreachable;
                        actualPass.byr = byr >= 1920 and byr <= 2002;
                    },
                    1 => {
                        const iyr = std.fmt.parseInt(u32, info[4..8], 10) catch unreachable;
                        actualPass.iyr = iyr >= 2010 and iyr <= 2020;
                    },
                    2 => {
                        const eyr = std.fmt.parseInt(u32, info[4..8], 10) catch unreachable;
                        actualPass.eyr = eyr >= 2020 and eyr <= 2030;
                    },
                    3 => {
                        const unitCM: ?usize = std.mem.indexOf(u8, info[4..], "c");

                        const heightOrNull: ?u32 = if(unitCM) |index| std.fmt.parseInt(u32, info[4..4+index], 10) catch unreachable
                            else blk: {
                                const indexOrNull = std.mem.indexOf(u8, info[4..], "i");
                                if(indexOrNull) |index| {
                                    break :blk std.fmt.parseInt(u32, info[4..4+index], 10) catch unreachable;
                                }
                                break :blk null;
                            };
                        if(heightOrNull == null) break;
                        const height = heightOrNull.?;
                        
                        if(unitCM != null) {
                            if(height >= 150 and height <= 193)
                                actualPass.hgt=true;
                        }
                        else if(height >= 59 and height <= 76) {
                            actualPass.hgt=true;
                        }
                    },
                    4 => {
                        if(info[4] != '#') break;
                        const values = info[5..];
                        if(values.len != 6) break;

                        actualPass.hcl = blk: for (values) |v| {
                            if(v >= '0' and v <= '9') continue;
                            if(v >= 'a' and v <= 'f') continue;
                            break :blk false;
                        }
                        else true;
                    },
                    5 => {
                        //amb blu brn gry grn hzl oth
                        const mapEyes = std.ComptimeStringMap(void, .{
                            .{"amb"},
                            .{"blu"},
                            .{"brn"},
                            .{"gry"},
                            .{"grn"},
                            .{"hzl"},
                            .{"oth"},
                        });
                        actualPass.ecl= mapEyes.has(info[4..]);
                    },
                    6 => {
                        const values = info[4..];
                        if(values.len != 9) break;

                        actualPass.pid = blk: for (values) |v| {
                            if(v < '0' or v > '9') break :blk false;
                        } else true;
                    },
                    7 => {actualPass.cid=true;},
                    else => unreachable
                }
            }
        }
        if(actualPass.isValid()) counter+=1;
    }
    std.log.info("{d}", .{counter});
}