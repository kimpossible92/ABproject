using SnakeMaze.Structs;
using UnityEngine;

namespace SnakeMaze.SO
{
    [CreateAssetMenu(fileName = "SnakeSkin", menuName = "Scriptables/SnakeSkin")]
    public class SnakeSkinSO : AbstractSkinSO
    {
        [SerializeField] private Sprite bodyUp;
        [SerializeField] private Sprite bodyDown;
        [SerializeField] private Sprite bodyRight;
        [SerializeField] private Sprite bodyLeft;
        [SerializeField] private Sprite bodyCornerTopRight;
        [SerializeField] private Sprite bodyCornerTopLeft;
        [SerializeField] private Sprite bodyCornerBottomRight;
        [SerializeField] private Sprite bodyCornerBottomLeft;

        [SerializeField] private Sprite headUp;
        [SerializeField] private Sprite headDown;
        [SerializeField] private Sprite headRight;
        [SerializeField] private Sprite headLeft;

        [SerializeField] private Sprite tailUp;
        [SerializeField] private Sprite tailDown;
        [SerializeField] private Sprite tailRight;
        [SerializeField] private Sprite tailLeft;

        private SnakeSkin _snakeSkin = new SnakeSkin();

        public SnakeSkin SnakeSkin => _snakeSkin;

        public void InitSnakeSkin()
        {
            _snakeSkin.SetAllSprites(headUp, headDown, headRight, headLeft,
                bodyUp, bodyDown, bodyRight, bodyLeft, bodyCornerTopRight, bodyCornerTopLeft,
                bodyCornerBottomRight, bodyCornerBottomLeft, tailUp, tailDown, tailRight, tailLeft);
        }

        public override void InitScriptable()
        {
            InitSnakeSkin();
        }
    }
}