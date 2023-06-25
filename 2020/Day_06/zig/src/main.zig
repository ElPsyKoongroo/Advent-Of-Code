const std = @import("std");
pub fn main() !void {
    var gpa = std.heap.GeneralPurposeAllocator(.{}){};
    defer _ = gpa.deinit();
    const allocator = gpa.allocator();

    const file = try std.fs.cwd().openFile("../AOCinput", .{});
    defer file.close();

    const file_buffer = try file.readToEndAlloc(allocator, std.math.maxInt(usize));
    defer allocator.free(file_buffer);

    std.log.info("{d}", .{Answer1(file_buffer)});
    std.log.info("{d}", .{Answer2(file_buffer)});
}

fn Answer1(file: []const u8) usize {
    var groups = std.mem.tokenizeSequence(u8, file, "\n\n");
    var counter: usize = 0;
    while (groups.next()) |group| {
        var lines = std.mem.tokenizeSequence(u8, group, "\n");
        var buffer = [_]bool{false} ** 26;
        while (lines.next()) |line| {
            for(line) |chr| {
                buffer[chr-'a'] = true;
            }
        }
        counter+= std.mem.count(bool, &buffer, &[_]bool{true});
    }

    return counter;
}

fn Answer2(file: []const u8) usize {
    var groups = std.mem.tokenizeSequence(u8, file, "\n\n");
    var counter: usize = 0;
    while (groups.next()) |group| {
        var lines = std.mem.tokenizeSequence(u8, group, "\n");
        var buffer = [_]u32{0} ** 26;
        var totalLines: u32 = 0;
        while (lines.next()) |line| : (totalLines+=1){
            for(line) |chr| {
                buffer[chr-'a']+=1;
            }
        }
        counter+= std.mem.count(u32, &buffer, &[_]u32{totalLines});
    }

    return counter;
}