import kotlin.io.path.Path
import kotlin.io.path.readText
import kotlin.collections.sortedDescending;
fun firstPart(lines: String) {
    var maxCalories = 0
    var currentCalories = 0
    for (line in lines.split("\n")) {
        if (line.isBlank()) {
            if (currentCalories > maxCalories) {
                maxCalories = currentCalories
            }
            currentCalories = 0
            continue
        }
        currentCalories += line.trim().toInt()
    }
    println(maxCalories)
}
fun firstPartFunctional(lines: String) {
    val maxCals = lines.split("\r\n\r\n")
        .maxOfOrNull { set -> set.split("\r\n").sumOf { e ->
            try {
                e.trim().toInt()
            } catch (e: NumberFormatException) {
                0
            }
        }};

    println(maxCals)
}


fun secondPart(lines: String) {
    val caloriesGroups = ArrayList<Int>()
    var currentCalories = 0
    for (line in lines.split("\n")) {
        if (line.isBlank()) {
            caloriesGroups.add(currentCalories)
            currentCalories = 0
            continue
        }
        currentCalories += line.trim().toInt()
    }
    println(caloriesGroups.sorted().reversed().take(3).sum())
}
fun secondPartFunctional(lines: String) {
    val maxCals = lines.split("\r\n\r\n")
        .asSequence()
        .map { set -> set.split("\r\n").sumOf { e ->
            try {
                e.trim().toInt()
            } catch (e: NumberFormatException) {
                0
            }
        }}.toList()
        .sortedDescending()
        .take(3)
        .sum();



    println(maxCals)
}

fun main(args: Array<String>) {
    val lines = Path("../AOCinput").readText()
    firstPart(lines)
    firstPartFunctional(lines)

    secondPart(lines)
    secondPartFunctional(lines)

}