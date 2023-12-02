EXE = main.bin

FLAGS = -std=c++23
OPT_FLAGS = -march=native -Ofast -funroll-loops
DEBUG_FLAGS = -O1 -ggdb3 -fno-omit-frame-pointer -D DEBUG

DEBUG = g++ $(FLAGS) $(DEBUG_FLAGS)
COMPILE = g++ $(FLAGS) $(OPT_FLAGS)

$(EXE) : main.cpp
	$(COMPILE) main.cpp -o $(EXE) 

all : $(EXE)

run : all
	clear
	@./$(EXE)

debug : COMPILE = $(DEBUG)
debug : all

