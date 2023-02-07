using UnityEngine;

namespace SnakeMaze.BSP
{
    public class Room
    {
        public MazeGrid Grid { get; set; }
        public Vector2 Center { get; set; }
        public Vector2Int Size { get; set; }

        public Vector2 BottomLeftCorner => new Vector2(Center.x - Size.x / 2f, Center.y - Size.y / 2f);
        public Vector2 TopLeftCorner => new Vector2(Center.x - Size.x / 2f, Center.y + Size.y / 2f);
        public Vector2 BottomRightCorner => new Vector2(Center.x + Size.x / 2f, Center.y - Size.y / 2f);
        public Vector2 TopRightCorner => new Vector2(Center.x + Size.x / 2f, Center.y + Size.y / 2f);
        public Vector2 LeftCenterPosition => new Vector2(Center.x - Size.x / 2f, Center.y);
        public Vector2 RightCenterPosition => new Vector2(Center.x + Size.x / 2f, Center.y);
        public Vector2 TopCenterPosition => new Vector2(Center.x, Center.y + Size.y / 2f);
        public Vector2 BottomCenterPosition => new Vector2(Center.x, Center.y - Size.y / 2f);

        public int NumberOfCorridors { get; set; }
        public bool IsExitRoom { get; set; }
        public int NumberOfFood { get; set; }

        public int NumberOfCells
        {
            get
            {
                var dist=BottomLeftCorner - TopRightCorner;
                return (int) (dist.x / Grid.CellSize.x * dist.y / Grid.CellSize.y);
            }
        }

        public Room(Vector2 center, Vector2Int size)
        {
            Center = center;
            Size = size;
            NumberOfCorridors = 0;
            NumberOfFood = 0;
        }
        public Room(Vector2 center, Vector2Int size, MazeGrid grid)
        {
            Center = center;
            Size = size;
            Grid = grid;
            NumberOfCorridors = 0;
            NumberOfFood = 0;
        }

        public override string ToString()
        {
            var dataString = "";

            dataString += $"pos<{Center.x},{Center.y}>:size<{Size.x},{Size.y}>";

            return dataString;
        }
    }
}