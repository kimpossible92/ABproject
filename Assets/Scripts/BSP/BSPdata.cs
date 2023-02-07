using UnityEngine;

namespace SnakeMaze.BSP
{
    public class BSPData
    {
        public Bounds PartitionBounds { get; set; }
        public Room StoredRoom { get; set; }
        public Vector2 LeftCenterPosition => new Vector2(Center.x - PartitionBounds.size.x / 2f, Center.y);
        public Vector2 RightCenterPosition => new Vector2(Center.x + PartitionBounds.size.x / 2f, Center.y);
        public Vector2 TopCenterPosition => new Vector2(Center.x, Center.y + PartitionBounds.size.y / 2f);
        public Vector2 BottomCenterPosition => new Vector2(Center.x, Center.y - PartitionBounds.size.y / 2f);
        public Vector2 BottomLeftCorner => new Vector2(Center.x - PartitionBounds.size.x / 2f, Center.y - PartitionBounds.size.y / 2f);
        public Vector2 TopLeftCorner => new Vector2(Center.x - PartitionBounds.size.x / 2f, Center.y + PartitionBounds.size.y / 2f);
        public Vector2 BottomRightCorner => new Vector2(Center.x + PartitionBounds.size.x / 2f, Center.y - PartitionBounds.size.y / 2f);
        public Vector2 TopRightCorner => new Vector2(Center.x + PartitionBounds.size.x / 2f, Center.y + PartitionBounds.size.y / 2f);

        public Vector2 Center
        {
            get
            {
                return PartitionBounds.center + PartitionBounds.size / 2f;
            }
        }

        public BSPData() { }

        public BSPData(Bounds bounds)
        {
            this.PartitionBounds = bounds;
        }

        public override string ToString()
        {
            string dataString = "";

            if (this != null)
            {
                dataString += $"pos<{PartitionBounds.center.x},{PartitionBounds.center.y}>:size<{PartitionBounds.size.x},{PartitionBounds.size.y}>";
            }

            return dataString;
        }
    }
}