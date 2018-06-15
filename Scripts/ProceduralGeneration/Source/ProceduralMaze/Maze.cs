using UnityEngine;
using System.Collections.Generic;
using FK.Utility.ArraysAndLists;

/// <summary>
/// <para>A randomly generated maze using the backtracing algorithm described here in an interative way: https://en.wikipedia.org/wiki/Maze_generation_algorithm#Recursive_backtracker
/// The maze generated is a doubly linked list of connected cells </para>
/// 
/// v1.0 05/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
public class Maze
{
    // ######################## STRUCTS AND CLASSES ######################## //
    /// <summary>
    /// One cell of a maze. It can have up to four connected neighbours
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// The Left Neighbour, null if there is a wall
        /// </summary>
        public Cell Left
        {
            get { return Neighbours[0]; }
        }
        /// <summary>
        /// The Upper Neighbour, null if there is a wall
        /// </summary>
        public Cell Up
        {
            get { return Neighbours[1]; }
        }
        /// <summary>
        /// The Right Neighbour, null if there is a wall
        /// </summary>
        public Cell Right
        {
            get { return Neighbours[2]; }
        }
        /// <summary>
        /// The Lower Neighbour, null if there is a wall
        /// </summary>
        public Cell Down
        {
            get { return Neighbours[3]; }
        }

        /// <summary>
        /// 0 LEFT, 1 UP, 2 RIGHT, 3 DOWN
        /// </summary>
        public Cell[] Neighbours = new Cell[4];

        /// <summary>
        /// Was this cell already visited by the generation algorithm?
        /// </summary>
        public bool Visited;

        /// <summary>
        /// X position of the Cell in the maze
        /// </summary>
        public int x;
        /// <summary>
        /// Y position of the Cell in the maze
        /// </summary>
        public int y;
    }

    // ######################## PUBLIC VARS ######################## //
    /// <summary>
    /// All the Cells in the maze
    /// </summary>
    public Cell[,] Cells { get; private set; }

    /// <summary>
    /// The width of the maze in numbers of cells
    /// </summary>
    public int Width { get { return Cells.GetLength(0); } }
    /// <summary>
    /// The Height of the maze in numbers of cells
    /// </summary>
    public int Height { get { return Cells.GetLength(1); } }

    /// <summary>
    /// Is the maze completely generated? False if still generating or not yet generated
    /// </summary>
    public bool Generated { get; private set; }


    // ######################## INITS ######################## //
    /// <summary>
    /// Generates the maze unsing an iterative version of the recursive backtracing algorithm.
    /// Depending on the size you chose this might take some time!
    /// </summary>
    /// <param name="width">The width of the maze in numbers of cells</param>
    /// <param name="height">The Height of the maze in numbers of cells</param>
    public void Generate(int width, int height)
    {
        // create the cell array and populate it
        Cells = new Cell[width, height];

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                Cell cell = new Cell();

                cell.x = x;
                cell.y = y;

                Cells[x, y] = cell;
            }
        }

        // create the variables needed for the maze generation
        List<Cell> _stack = new List<Cell>();
        int _totalCells = width * height;
        int _visitedCells = 0;

        // start with a random cell
        Cell current = Cells[Random.Range(0, width), Random.Range(0, height)];
        current.Visited = true;
        ++_visitedCells;

        // this is the main part of the algorithm. As long as there are unvisited cells,
        // try getting a random unvisited neighbour and connect it to the current cell.
        // Then push the current cell on a stack and make the random neighbour we chose the current cell.
        // If the current cell has no unvisited neighbours, pop a cell to from the stack and make it the current.
        while (_visitedCells < _totalCells)
        {
            // get random neighbour
            Cell randomNeighbour = GetUnvisitedNeighbour(current.x, current.y);

            // if there where unvisited neighbours, do what we need to do
            if (randomNeighbour != null)
            {
                _stack.Add(current);

                int xDiff = randomNeighbour.x - current.x;
                int yDiff = randomNeighbour.y - current.y;

                if (xDiff < 0)
                {
                    current.Neighbours[0] = randomNeighbour;
                    randomNeighbour.Neighbours[2] = current;
                }
                else if (xDiff > 0)
                {
                    current.Neighbours[2] = randomNeighbour;
                    randomNeighbour.Neighbours[0] = current;
                }
                else if (yDiff < 0)
                {
                    current.Neighbours[1] = randomNeighbour;
                    randomNeighbour.Neighbours[3] = current;
                }
                else if (yDiff > 0)
                {
                    current.Neighbours[3] = randomNeighbour;
                    randomNeighbour.Neighbours[1] = current;
                }

                randomNeighbour.Visited = true;
                ++_visitedCells;
                current = randomNeighbour;
            }
            else // if there where no unvisited neighbours, go back one cell
            {
                current = _stack[_stack.Count - 1];
                _stack.RemoveAt(_stack.Count - 1);
            }
        }

        // finished generating
        Generated = true;
    }


    // ######################## FUNCTIONALITY ######################## //
    /// <summary>
    /// Returns position of the cell if not null, else returns (-1, -1)
    /// </summary>
    /// <param name="cell">The cell to go to</param>
    /// <returns></returns>
    private Vector2Int GoTo(Cell cell)
    {
        return cell != null ? new Vector2Int(cell.x, cell.y) : new Vector2Int(-1, -1);
    }

    /// <summary>
    /// Try to go to the cell upwards from the cell at the provided position. 
    /// If there is no wall, this returns the position of that cell, else it returns (-1, -1)
    /// </summary>
    /// <param name="x">Current x position</param>
    /// <param name="y">Current y position</param>
    /// <returns></returns>
    public Vector2Int GoUp(int x, int y)
    {
        Cell up = Cells[x, y].Up;
        return GoTo(up);
    }

    /// <summary>
    /// Try to go to the cell downwards from the cell at the provided position. 
    /// If there is no wall, this returns the position of that cell, else it returns (-1, -1)
    /// </summary>
    /// <param name="x">Current x position</param>
    /// <param name="y">Current y position</param>
    /// <returns></returns>
    public Vector2Int GoDown(int x, int y)
    {
        Cell down = Cells[x, y].Down;
        return GoTo(down);
    }

    /// <summary>
    /// Try to go to the cell left from the cell at the provided position. 
    /// If there is no wall, this returns the position of that cell, else it returns (-1, -1)
    /// </summary>
    /// <param name="x">Current x position</param>
    /// <param name="y">Current y position</param>
    /// <returns></returns>
    public Vector2Int GoLeft(int x, int y)
    {
        Cell left = Cells[x, y].Left;
        return GoTo(left);
    }

    /// <summary>
    /// Try to go to the cell right from the cell at the provided position. 
    /// If there is no wall, this returns the position of that cell, else it returns (-1, -1)
    /// </summary>
    /// <param name="x">Current x position</param>
    /// <param name="y">Current y position</param>
    /// <returns></returns>
    public Vector2Int GoRight(int x, int y)
    {
        Cell right = Cells[x, y].Right;
        return GoTo(right);
    }

    /// <summary>
    /// Try to go to the cell upwards from the cell at the provided position. 
    /// If there is no wall, this returns the position of that cell, else it returns (-1, -1)
    /// </summary>
    /// <param name="currentPos">Current position</param>
    /// <returns></returns>
    public Vector2Int GoUp(Vector2Int currentPos)
    {
        Cell up = Cells[currentPos.x, currentPos.y].Up;
        return GoTo(up);
    }

    /// <summary>
    /// Try to go to the cell downwards from the cell at the provided position. 
    /// If there is no wall, this returns the position of that cell, else it returns (-1, -1)
    /// </summary>
    /// <param name="currentPos">Current position</param>
    /// <returns></returns>
    public Vector2Int GoDown(Vector2Int currentPos)
    {
        Cell down = Cells[currentPos.x, currentPos.y].Down;
        return GoTo(down);
    }

    /// <summary>
    /// Try to go to the cell left from the cell at the provided position. 
    /// If there is no wall, this returns the position of that cell, else it returns (-1, -1)
    /// </summary>
    /// <param name="currentPos">Current position</param>
    /// <returns></returns>
    public Vector2Int GoLeft(Vector2Int currentPos)
    {
        Cell left = Cells[currentPos.x, currentPos.y].Left;
        return GoTo(left);
    }

    /// <summary>
    /// Try to go to the cell right from the cell at the provided position. 
    /// If there is no wall, this returns the position of that cell, else it returns (-1, -1)
    /// </summary>
    /// <param name="currentPos">Current position</param>
    /// <returns></returns>
    public Vector2Int GoRight(Vector2Int currentPos)
    {
        Cell right = Cells[currentPos.x, currentPos.y].Right;
        return GoTo(right);
    }


    // ######################## UTILITIES ######################## //
    /// <summary>
    /// Returns a random unvisited neighbour of the cell at the provided position.
    /// If there are no unvisited neighbours, it returns null
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private Cell GetUnvisitedNeighbour(int x, int y)
    {
        List<Cell> unvisitedNeighbours = new List<Cell>();

        if (x > 0 && !Cells[x - 1, y].Visited)
            unvisitedNeighbours.Add(Cells[x - 1, y]);
        if (y > 0 && !Cells[x, y - 1].Visited)
            unvisitedNeighbours.Add(Cells[x, y - 1]);
        if (x < Cells.GetLength(0) - 1 && !Cells[x + 1, y].Visited)
            unvisitedNeighbours.Add(Cells[x + 1, y]);
        if (y < Cells.GetLength(1) - 1 && !Cells[x, y + 1].Visited)
            unvisitedNeighbours.Add(Cells[x, y + 1]);

        if (unvisitedNeighbours.Count == 0)
            return null;

        return unvisitedNeighbours.GetRandom();
    }

}
