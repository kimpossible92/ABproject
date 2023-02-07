using System.Collections.Generic;
using SnakeMaze.BSP;
using SnakeMaze.Enums;
using SnakeMaze.TileMaps;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnakeMaze.Maze
{
    public class MazeBuilder : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Vector2 cellSize = new Vector2(1f, 1f);
        [SerializeField] private TileMapVisualizer tileMapVisualizer;
        private Vector2Int _currentGidSize;
        private Vector2 _currentBottomLeft = new Vector2(0, 0);

        private MazeGrid _currentGrid;
        private List<Vector2> _currentFrontier;
        private int _rows, _columns;

        public Vector2Int GridSize
        {
            get => _currentGidSize;
            set => _currentGidSize = value;
        }

        public Vector2 CellSize
        {
            get => cellSize;
            set => cellSize = value;
        }

        public Vector2 BottomLeft
        {
            get => _currentBottomLeft;
            set => _currentBottomLeft = value;
        }

        private void Awake()
        {
            cellPrefab.transform.localScale = new Vector3(cellSize.x, cellSize.y, 1);
        }

        public void GenerateMazes(List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                InitMazeValues(room);
                room.Grid=new MazeGrid(_currentGidSize, cellSize);
                _currentGrid = room.Grid;
                CreateMaze(room);
            }
        }

        private void InitMazeValues(Room room)
        {
            _currentBottomLeft = room.BottomLeftCorner;
            _currentGidSize = room.Size;
            cellSize = Vector2.one;
        }


        private void CreateMaze()
        {
            CreateTheGrid();
            RunPrimm();
            // StartCoroutine(Primm());
        }
        private void CreateMaze(Room room)
        {
            CreateTheGrid(room);
            RunPrimm();
            // StartCoroutine(Primm());
        }

        public void PaintTheMaze(MazeGrid mazeGrid)
        {
            List<WallTile> wallList=new List<WallTile>();
            foreach (var mazeCell in mazeGrid.Grid)
            {
                if(!mazeCell.IsExit)
                {
                    mazeCell.SetWallTile();
                    wallList.Add(mazeCell.Tile);
                }
                else
                {
                    var pos = new Vector2Int((int)mazeCell.Position.x, (int)mazeCell.Position.y);
                    tileMapVisualizer.PaintExitTile(pos);
                }
            }
            tileMapVisualizer.PaintWallTiles(wallList);
        }
        private void CreateTheGrid()
        {
            for (int i = 0; i < _currentGrid.Rows; i++)
            for (int j = 0; j < _currentGrid.Columns; j++)
            {
                Vector3 cellPosition = _currentBottomLeft
                                       + Vector2.right * (i * cellSize.x + cellSize.x / 2)
                                       + Vector2.up * (j * cellSize.y + cellSize.y / 2);
                _currentGrid.Grid[i, j] = new MazeCell(cellPosition, i, j);
            }
        }
        private void CreateTheGrid(Room room)
        {
            for (int i = 0; i < _currentGrid.Rows; i++)
            for (int j = 0; j < _currentGrid.Columns; j++)
            {
                Vector3 cellPosition = _currentBottomLeft
                                       + Vector2.right * (i * cellSize.x + cellSize.x / 2)
                                       + Vector2.up * (j * cellSize.y + cellSize.y / 2);
                _currentGrid.Grid[i, j] = new MazeCell(cellPosition, i, j, room);
            }
        }
        private void RunPrimm()
        {
            _currentFrontier = new List<Vector2>();
            SetCellInMaze(Random.Range(0, _currentGrid.Rows), Random.Range(0, _currentGrid.Columns));
            var neighbors = new List<Vector2>();
            var cellPos = new Vector2();
            var neighborSelected = new Vector2();
            var removePosition = 0;
            var direction = Directions.Right;
            while (_currentFrontier.Count != 0)
            {
                removePosition = Random.Range(0, _currentFrontier.Count);
                cellPos = _currentFrontier[removePosition];
                _currentFrontier.RemoveAt(removePosition);
        
                neighbors = GetNeighbors((int) cellPos.x, (int) cellPos.y);
                neighborSelected = neighbors[Random.Range(0, neighbors.Count)];
        
                direction = GetDirection((int) cellPos.x, (int) cellPos.y, (int) neighborSelected.x,
                    (int) neighborSelected.y);
                _currentGrid.Grid[(int) cellPos.x, (int) cellPos.y].GetWall(direction);
                direction = (Directions)((int)direction * -1);
                _currentGrid.Grid[(int) neighborSelected.x,
                    (int) neighborSelected.y].GetWall(direction);
                SetCellInMaze((int) cellPos.x, (int) cellPos.y);
            }
        
            
        }

        private List<Vector2> GetNeighbors(int i, int j)
        {
            var neighbors = new List<Vector2>();

            void AddNeighbor(int x, int y)
            {
                neighbors.Add(new Vector2(x, y));
            }

            if (i > 0 && _currentGrid.Grid[i - 1, j].InMaze)
                AddNeighbor(i - 1, j);

            if (i + 1 < _currentGrid.Rows && _currentGrid.Grid[i + 1, j].InMaze)
                AddNeighbor(i + 1, j);

            if (j > 0 && _currentGrid.Grid[i, j - 1].InMaze)
                AddNeighbor(i, j - 1);

            if (j + 1 < _currentGrid.Columns && _currentGrid.Grid[i, j + 1].InMaze)
                AddNeighbor(i, j + 1);
            return neighbors;
        }

        private void AddFrontier(int i, int j)
        {
            if (i < 0 || j < 0 || i >= _currentGrid.Rows || j >= _currentGrid.Columns) return;
            if (_currentGrid.Grid[i, j].IsFrontier || _currentGrid.Grid[i, j].InMaze) return;

            _currentFrontier.Add(new Vector2(i, j));
            _currentGrid.Grid[i, j].IsFrontier = true;
        }

        private void SetCellInMaze(int i, int j)
        {
            _currentGrid.Grid[i, j].InMaze = true;
            _currentGrid.Grid[i, j].IsFrontier = false;

            AddFrontier(i - 1, j);
            AddFrontier(i + 1, j);
            AddFrontier(i, j + 1);
            AddFrontier(i, j - 1);
        }

        private Directions GetDirection(int i, int j, int x, int y)
        {
            Directions dir = new Directions();
            if (i > x)
                dir = Directions.Left;
            if (i < x)
                dir = Directions.Right;
            if (j > y)
                dir = Directions.Down;
            if (j < y)
                dir = Directions.Up;
            return dir;
        }
    }
}