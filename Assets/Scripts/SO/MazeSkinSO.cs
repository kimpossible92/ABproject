using System.Collections.Generic;
using SnakeMaze.Enums;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SnakeMaze.SO
{
    [CreateAssetMenu(fileName = "MazeSkin", menuName = "Scriptables/TileMaps/MazeSkinSO")]
    public class MazeSkinSO : AbstractSkinSO
    {
        [Header("Maze")] [SerializeField] private TileBase topRightLeft;

        [SerializeField] private TileBase
            topRightBot,
            topLeftBot,
            topRight,
            topLeft,
            topBot,
            top,
            rightLeftBot,
            rightLeft,
            rightBot,
            leftBot,
            right,
            left,
            bot,
            empty;

        [Header("Corridors")] [SerializeField] private TileBase horizontalCorridor;
        [SerializeField] private TileBase verticalCorridor;

        private Dictionary<WallSprites, TileBase> _tileDic;

        public TileBase HorizontalCorridor => horizontalCorridor;
        public TileBase VerticalCorridor => verticalCorridor;

        [Header("Exit")] [SerializeField] private TileBase exit;
        [Header("Food")] [SerializeField] private List<TileBase> food;
        [Header("Background Color")] [SerializeField] private Color backgroundColor;
        

        public TileBase Exit
        {
            get => exit;
            set => exit = value;
        }
        public TileBase Food
        {
            get => food[Random.Range(0,food.Count)];
        }

        public Dictionary<WallSprites, TileBase> TileDic
        {
            get => _tileDic;
            set => _tileDic = value;
        }

        public Color BackgroundColor => backgroundColor;

        public void InitMazeSkin()
        {
            _tileDic = new Dictionary<WallSprites, TileBase>()
            {
                {WallSprites.TopRightLeft, topRightLeft},
                {WallSprites.TopRightBot, topRightBot},
                {WallSprites.TopLeftBot, topLeftBot},
                {WallSprites.TopRight, topRight},
                {WallSprites.TopLeft, topLeft},
                {WallSprites.TopBot, topBot},
                {WallSprites.Top, top},
                {WallSprites.RightLeftBot, rightLeftBot},
                {WallSprites.RightLeft, rightLeft},
                {WallSprites.RightBot, rightBot},
                {WallSprites.LeftBot, leftBot},
                {WallSprites.Right, right},
                {WallSprites.Left, left},
                {WallSprites.Bot, bot},
                {WallSprites.Empty, empty}
            };
        }

        public override void InitScriptable()
        {
            InitMazeSkin();
        }
    }
}