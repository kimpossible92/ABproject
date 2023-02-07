using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeMaze.Structs
{
    public struct SnakeSkin
    {
        public SnakeBody Body;
        public SnakeHead Head;
        public SnakeTail Tail;

        public SnakeSkin(SnakeBody body, SnakeHead head, SnakeTail tail)
        {
            Body = body;
            Head = head;
            Tail = tail;
        }

        public void SetSkinProperties(SnakeBody body, SnakeHead head, SnakeTail tail)
        {
            Body = body;
            Head = head;
            Tail = tail;
        }

        public void SetBodySprites(Sprite up, Sprite down, Sprite right, Sprite left,
            Sprite cornerTopRight, Sprite cornerTopLeft, Sprite cornerBottomRight, Sprite cornerBottomLeft)
        {
            Body.SetAllSprites(up, down, right, left, cornerTopRight, cornerTopLeft, cornerBottomRight,
                cornerBottomLeft);
        }

        public void SetHeadSprites(Sprite up, Sprite down, Sprite right, Sprite left)
        {
            Head.SetAllSprites(up, down, right, left);
        }

        public void SetTailSprites(Sprite up, Sprite down, Sprite right, Sprite left)
        {
            Tail.SetAllSprites(up, down, right, left);
        }

        public void SetAllSprites(Sprite headUp, Sprite headDown, Sprite headRight, Sprite headLeft,
            Sprite bodyUp, Sprite bodyDown, Sprite bodyRight, Sprite bodyLeft,
            Sprite bodyCornerTopRight, Sprite bodyCornerTopLeft, Sprite bodyCornerBottomRight,
            Sprite bodyCornerBottomLeft,
            Sprite tailUp, Sprite tailDown, Sprite tailRight, Sprite tailLeft)
        {
            SetHeadSprites(headUp, headDown, headRight, headLeft);
            SetBodySprites(bodyUp, bodyDown, bodyRight, bodyLeft,
                bodyCornerTopRight, bodyCornerTopLeft,
                bodyCornerBottomRight, bodyCornerBottomLeft);
            SetTailSprites(tailUp, tailDown, tailRight, tailLeft);
        }
    }

    public struct SnakeBody
    {
        public Sprite Up;
        public Sprite Down;
        public Sprite Right;
        public Sprite Left;
        public Sprite CornerTopRight;
        public Sprite CornerTopLeft;
        public Sprite CornerBottomRight;
        public Sprite CornerBottomLeft;

        public void SetAllSprites(Sprite up, Sprite down, Sprite right, Sprite left,
            Sprite cornerTopRight, Sprite cornerTopLeft, Sprite cornerBottomRight, Sprite cornerBottomLeft)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
            CornerTopRight = cornerTopRight;
            CornerTopLeft = cornerTopLeft;
            CornerBottomRight = cornerBottomRight;
            CornerBottomLeft = cornerBottomLeft;
        }
    }

    public struct SnakeHead
    {
        public Sprite Up;
        public Sprite Down;
        public Sprite Right;
        public Sprite Left;

        public SnakeHead(Sprite up, Sprite down, Sprite right, Sprite left)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }

        public void SetAllSprites(Sprite up, Sprite down, Sprite right, Sprite left)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }
    }

    public struct SnakeTail
    {
        public Sprite Up;
        public Sprite Down;
        public Sprite Right;
        public Sprite Left;

        public SnakeTail(Sprite up, Sprite down, Sprite right, Sprite left)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }

        public void SetAllSprites(Sprite up, Sprite down, Sprite right, Sprite left)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }
    }
}