const std = @import("std");

pub fn main() !void {
    const data = @embedFile("AOCtest");
    try part1(data);
}

pub fn part1(input: []const u8) !void {
    var data = std.mem.split(u8, input, "\n");
    const desired_value = 2020;

    var gpa = std.heap.GeneralPurposeAllocator(.{}){};
    defer {
        _ = gpa.deinit();
    }
    const allocator = gpa.allocator();
    var allItems = std.AutoHashMap(u32, u32).init(allocator);

    defer allItems.clearAndFree();

    while (data.next()) |line| {
        const actualValue: u32 = try std.fmt.parseInt(u32, line, 10);

        const diff = desired_value - actualValue;

        var val = allItems.get(diff);

        if (val) |v| {
            std.debug.print("Values {} - {}, total -> {}", .{ diff, v, v * diff });
            return;
        }

        try allItems.put(actualValue, diff);
    }
    try std.io.getStdOut().writer().print("No se ha encontrado na", .{});
    return;
}

pub fn part2(input: []const u8) !void {
    var data = std.mem.split(u8, input, "\n");
    const desired_value = 2020;

    var gpa = std.heap.GeneralPurposeAllocator(.{}){};
    defer {
        _ = gpa.deinit();
    }
    const allocator = gpa.allocator();

    var allItems = std.ArrayList(u32).init(allocator);
    defer allItems.deinit();

    while (data.next()) |line| {
        try allItems.append(try std.fmt.parseInt(u32, line, 10));
    }

    for (allItems.items, 0..) |first_item, i| {
        for (allItems.items, i + 1..) |second_item, j| {
            for (allItems.items, j + 1..) |third_item, k| {
                _ = k;
                if (first_item + second_item + third_item == desired_value) {
                    std.debug.print("Values: {} - {} - {} -> {}\n", .{ first_item, second_item, third_item, first_item * second_item * third_item });
                    return;
                }
            }
        }
    }
}
