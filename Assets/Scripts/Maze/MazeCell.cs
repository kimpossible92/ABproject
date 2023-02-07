using System;
using System.Collections.Generic;
using SnakeMaze.BSP;
using SnakeMaze.Enums;
using SnakeMaze.Exceptions;
using SnakeMaze.TileMaps;
using UnityEngine;

namespace SnakeMaze.Maze
{
    

    public class MazeCell
    {
        public int GridX { get; }
        public int GridY { get; }
        public Vector3 Position { get; }
        public WallTile Tile { get; private set; }
        public Dictionary<Directions, GameObject> Walls { get; set; }
        public bool InMaze { get; set; }
        public bool IsFrontier { get; set; }
        public bool IsExit { get; set; }
        public bool HasFood { get; set; }
        private char[] spriteBinaryType;
        public Room CurrentRoom{get;set;}
        
        public MazeCell(Vector3 pos, int i, int j)
        {
            Position = pos;
            GridX = i;
            GridY = j;
            spriteBinaryType= new []{'1','1','1','1'};
            IsExit = false;
            HasFood = false;
        }
        public MazeCell(Vector3 pos, int i, int j, Room room)
        {
            Position = pos;
            GridX = i;
            GridY = j;
            spriteBinaryType= new []{'1','1','1','1'};
            IsExit = false;
            HasFood = false;
            CurrentRoom = room;
        }

        public void GetWall(Directions dir)
        {
            var index = dir switch
            {
                Directions.Up => 0,
                Directions.Down => 2,
                Directions.Right => 1,
                Directions.Left => 3,
                _=> throw new NotEnumTypeSupportedException()
            };
            spriteBinaryType[index] = '0';
        }

        public void SetWallTile()
        {
            string binary = "" ;
            for (int i = 0; i < spriteBinaryType.Length; i++)
            {
                binary += spriteBinaryType[i];
            }
            var number = Convert.ToInt32(binary, 2);
            Vector2Int pos = new Vector2Int((int)Position.x, (int)Position.y);
            Tile = new WallTile(pos, (WallSprites) number);
        }
    }
}