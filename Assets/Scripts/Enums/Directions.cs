using System;
using System.Collections;
using System.Collections.Generic;
using SnakeMaze.Exceptions;
using UnityEngine;

namespace SnakeMaze.Enums
{
    public enum Directions
    {
        Up = 1,
        Down = -1,
        Right = 2,
        Left = -2,
    }

    public static class DirectionsActions
    {
        public static Vector2 DirectionsToVector2(Directions direction)
        {
            var dir = direction switch
            {
                Directions.Up => Vector2.up,
                Directions.Down => Vector2.down,
                Directions.Right => Vector2.right,
                Directions.Left => Vector2.left,
                _=> throw new NotEnumTypeSupportedException()
            };
            return dir;
        }
        public static Directions GetOppositeDirection(Directions direction)
        {
            var oppsite = direction switch
            {
                Directions.Up => Directions.Down,
                Directions.Down => Directions.Up,
                Directions.Right => Directions.Left,
                Directions.Left => Directions.Right,
                _=> throw new NotEnumTypeSupportedException()
            };
            return oppsite;
        }
    }
}
