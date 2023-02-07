using System.Collections;
using System.Collections.Generic;
using SnakeMaze.Enums;
using SnakeMaze.Maze;
using UnityEngine;

public class MazeGrid
{
   private Vector2Int _gridSize;
   private Vector2 _cellSize;
   private MazeCell[,] _grid;
   private int _rows, _columns;

   public MazeCell[,] Grid
   {
      get => _grid;
      set => _grid = value;
   }

   public Vector2 CellSize
   {
      get => _cellSize;
      set => _cellSize = value;
   }

   public int Rows => _rows;
   public int Columns => _columns;

   public MazeGrid(Vector2Int gridSize, Vector2 cellSize)
   {
      _gridSize = gridSize;
      _cellSize = cellSize;

      _rows = (int) Mathf.Ceil(_gridSize.x / _cellSize.x);
      _columns = (int) Mathf.Ceil(_gridSize.y / _cellSize.y);

      _grid = new MazeCell[_rows, _columns];
   }
   public MazeCell GetCellAtPosition(Vector2 bottomLeft, Vector2 pos)
   {
      var iCell =  Mathf.FloorToInt((pos.x - bottomLeft.x) / _cellSize.x);
      var jCell = Mathf.FloorToInt((pos.y - bottomLeft.y) / _cellSize.y);
      return _grid[iCell, jCell];
   }

   public void GetWallAtPosition(Vector2 bottomLeft, Vector2 pos,Directions direction)
   {
      var number = Mathf.Abs(pos.x - bottomLeft.x);
      var iCell =  Mathf.FloorToInt(number/ _cellSize.x);
      number = Mathf.Abs(pos.y - bottomLeft.y);
      var jCell = Mathf.FloorToInt(number / _cellSize.y);
      iCell = Mathf.Clamp(iCell, 0, _grid.GetLength(0)-1);
      jCell = Mathf.Clamp(jCell, 0, _grid.GetLength(1)-1);
      _grid[iCell, jCell].GetWall(direction);
   }

   public MazeCell GetRandomCell()
   {
      return Grid[Random.Range(0, _rows), Random.Range(0, _columns)];
   }
}
