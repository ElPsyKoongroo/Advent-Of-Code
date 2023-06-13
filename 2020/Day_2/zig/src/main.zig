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

    //Answer1(file_buffer);
    Answer2(file_buffer);
}

fn Answer1(file: []const u8) void {
    var iter = std.mem.splitSequence(u8, file, "\n");

    var counter: u32 = 0;
    while (iter.next()) |line| {
        if (line.len == 0) continue;
        const dashIndex = std.mem.indexOf(u8, line, "-").?;
        const spaceIndex = std.mem.indexOf(u8, line, " ").?;

        const minAmmount = std.fmt.parseInt(u8, line[0..dashIndex], 10) catch unreachable;
        const maxAmmount = std.fmt.parseInt(u8, line[dashIndex + 1 .. spaceIndex], 10) catch unreachable;
        const char = line[spaceIndex + 1 .. spaceIndex + 2];
        const text = line[spaceIndex + 4 ..];

        const ammount = std.mem.count(u8, text, char);

        if (ammount <= maxAmmount and ammount >= minAmmount) counter += 1;
    }
    std.log.info("{d}", .{counter});
}

fn Answer2(file: []const u8) void {
    var iter = std.mem.splitSequence(u8, file, "\n");

    var counter: u32 = 0;
    var totalLines: u32 = 0;
    while (iter.next()) |line| {
        totalLines += 1;
        if (line.len == 0) continue;
        const dashIndex = std.mem.indexOf(u8, line, "-").?;
        const spaceIndex = std.mem.indexOf(u8, line, " ").?;

        const firstIndex = std.fmt.parseInt(u8, line[0..dashIndex], 10) catch unreachable;
        const secondIndex = std.fmt.parseInt(u8, line[dashIndex + 1 .. spaceIndex], 10) catch unreachable;
        const char = line[spaceIndex + 1];
        const text = line[spaceIndex + 4 ..];

        const firstNice = firstIndex - 1 < text.len and text[firstIndex - 1] == char;
        const secondNice = secondIndex - 1 < text.len and text[secondIndex - 1] == char;
        if (firstNice != secondNice) counter += 1;
    }
    std.log.info("{d}", .{counter});
}
