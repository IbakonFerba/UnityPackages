# Procedural Generation
Scripts for proceduraly generating things

# Packages
## ProceduralMaze
 - `Maze`: A randomly generated maze using the backtracing algorithm described here in an interative way: https://en.wikipedia.org/wiki/Maze_generation_algorithm#Recursive_backtracker
The maze generated is a doubly linked list of connected cells.
Use `Generate` to generate the maze, as it is just a grid when an Instance is created due to the generation taking some time if it is a big maze, so you have the control over when this big calculation happens.
