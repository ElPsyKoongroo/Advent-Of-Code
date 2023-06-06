import java.util.Vector
import kotlin.io.path.Path;
import kotlin.io.path.readLines
import kotlin.io.path.readText

fun FirstPart(lines: String) {
    var max_calories = 0;
    var current_calories = 0;
    for (line in lines.split("\n")) {
        if (line.isBlank()) {
            if (current_calories > max_calories) {
                max_calories = current_calories
            }
            current_calories = 0;
            continue;
        }
        current_calories += line.trim().toInt()
    }
    println(max_calories)
}

fun SecondPart(lines: String) {
    var calories_groups = ArrayList<Int>();
    var current_calories = 0;
    for (line in lines.split("\n")) {
        if (line.isBlank()) {
            calories_groups.add(current_calories)
            current_calories = 0
            continue
        }
        current_calories += line.trim().toInt()
    }
    println(calories_groups.sorted().reversed().take(3).sum())
}

fun main(args: Array<String>) {
    val lines = Path("../AOCinput").readText();
    FirstPart(lines);
    SecondPart(lines);
}