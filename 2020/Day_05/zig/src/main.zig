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

    std.log.info("{d}", .{Answer1(file_buffer)});
    std.log.info("{d}", .{try Answer2(file_buffer, allocator)});
}

fn Answer1(file: []const u8) u32 {
    var lines = std.mem.tokenizeSequence(u8, file, "\n");

    var maxID: u32 = 0;

    while(lines.next()) |line| {
        var rangeMin: u32 = 0;
        var rangeMax: u32 = 127;

        const posRow = for(0..7) |i| {
            const nextRange = (rangeMax - rangeMin + 1) / 2;
            if(line[i] == 'B') {
                rangeMin += nextRange;
            }
            else {
                rangeMax = rangeMin + nextRange - 1;
            }
        }
        else rangeMin;
        
        rangeMin = 0;
        rangeMax = 7;

        const posColumn = for(0..3) |i| {
            const nextRange = (rangeMax - rangeMin + 1) / 2;

            if(line[7+i] == 'R') {
                rangeMin += nextRange;
            }
            else {
                rangeMax = rangeMin + nextRange - 1;
            }
        }
        else rangeMin;

        const actID = posRow * 8 + posColumn;
        if(actID > maxID) maxID = actID;
    }
    return maxID;
}

fn Answer2(file: []const u8, allocator: std.mem.Allocator) !u32 {
    var lines = std.mem.tokenizeSequence(u8, file, "\n");

    var maxID: u32 = 0;
    var minID: u32 = std.math.maxInt(u32);

    var ids = std.ArrayList(u32).init(allocator);
    defer _ = ids.deinit();

    while(lines.next()) |line| {
        var rangeMin: u32 = 0;
        var rangeMax: u32 = 127;

        const posRow = for(0..7) |i| {
            const nextRange = (rangeMax - rangeMin + 1) / 2;
            if(line[i] == 'B') {
                rangeMin += nextRange;
            }
            else {
                rangeMax = rangeMin + nextRange - 1;
            }
        } else rangeMin;
        
        rangeMin = 0;
        rangeMax = 7;

        const posColumn = for(0..3) |i| {
            const nextRange = (rangeMax - rangeMin + 1) / 2;

            if(line[7+i] == 'R') {
                rangeMin += nextRange;
            }
            else {
                rangeMax = rangeMin + nextRange - 1;
            }
        } else rangeMin;

        const actID = posRow * 8 + posColumn;
        if(actID > maxID) maxID = actID;
        if(actID < minID) minID = actID;
        try ids.append(actID);
    }
    
    const notFound: u32 =  blkFor: for(minID+1..maxID) |actID| {
        const found: bool = blk: for(ids.items) |item| {
            if(item == actID) break :blk true;
        } else false;
        if(!found) break :blkFor @intCast(u32,  actID);
    } else unreachable;

    return notFound;
}