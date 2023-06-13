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

    Answer1(file_buffer, allocator);
    Answer2(file_buffer, allocator);
}

fn Answer1(file: []const u8, allocator: std.mem.Allocator) void {
    var array = std.ArrayList([]const u8).init(allocator);
    defer _ = array.deinit();

    var it = std.mem.splitSequence(u8, file, "\n");

    while(it.next()) |line| {
        if(line.len == 0) continue;
        array.append(line) catch unreachable;
    }

    const rowSize = array.items[0].len;

    var posX: u32 = 3;
    var posY: u32 = 1;

    var counter: u32 = 0;
    while (true) : ({posX+=3; posY+=1;}){
        if(posY >= array.items.len) break; 
        posX = @intCast(u32, posX % rowSize);

        if(array.items[posY][posX] == '#') counter+=1;
    }
    std.log.info("{}", .{counter});
}

fn Answer2(file: []const u8, allocator: std.mem.Allocator) void {
        var array = std.ArrayList([]const u8).init(allocator);
    defer _ = array.deinit();

    var it = std.mem.splitSequence(u8, file, "\n");

    while(it.next()) |line| {
        if(line.len == 0) continue;
        array.append(line) catch unreachable;
    }

    const rowSize = array.items[0].len;

    const xIncrements = [_]u32{1,3,5,7,1};
    const yIncrements = [_]u32{1,1,1,1,2};

    var total: u64 = 1;
    for(xIncrements, yIncrements) |xIncrement, yIncrement| {
        var counter: u64 = 0;
        var posX: u32 = xIncrement;
        var posY: u32 = yIncrement;

        while (true) : ({posX+=xIncrement; posY+=yIncrement;}){
            if(posY >= array.items.len) break; 
            posX = @intCast(u32, posX % rowSize);

            if(array.items[posY][posX] == '#') counter+=1;
        }
        total *= counter;
    }

    
    std.log.info("{}", .{total});
}
