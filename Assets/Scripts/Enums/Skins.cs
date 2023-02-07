using System;
using System.Collections.Generic;
using SnakeMaze.Exceptions;
using SnakeMaze.Utils;
using UnityEngine;

namespace SnakeMaze.Enums
{
    public enum MazeSkinEnum 
    {
        Default, Space, Skin3, Error
    }

    public enum SnakeSkinEnum
    {
        Default, Astronaut,  Skin3, Error
    }

    public enum AudioSkinEnum
    {
        Default
    }

    public static class SkinEnumUtils
    {
        public static SnakeSkinEnum StringToSnakeEnumById(string value)
        {
            SnakeSkinEnum skin;
            switch (value)
            {
                case Constants.AstronautSnakeSkin:
                    skin = SnakeSkinEnum.Astronaut;
                    break;
                case Constants.Skin3SnakeSkin:
                    skin = SnakeSkinEnum.Skin3;
                    break;
                case Constants.DefaultSnakeSkin:
                    skin = SnakeSkinEnum.Default;
                    break;
                default:
                    Debug.Log("Id does not correspond to a snake skin");
                    skin = SnakeSkinEnum.Error;
                    break;
            }

            return skin;
        }
        public static SnakeSkinEnum StringToSnakeEnum(string value)
        {
            SnakeSkinEnum skin;
            switch (value)
            {
                case "Astronaut":
                    skin = SnakeSkinEnum.Astronaut;
                    break;
                case "Skin3":
                    skin = SnakeSkinEnum.Skin3;
                    break;
                case "Default":
                    skin = SnakeSkinEnum.Default;
                    break;
                default:
                    Debug.Log("Id does not correspond to a snake skin");
                    skin = SnakeSkinEnum.Error;
                    break;
            }

            return skin;
        }
        public static MazeSkinEnum StringToMazeEnumById(string value)
        {
            MazeSkinEnum skin;
            switch (value)
            {
                case Constants.SpaceMazeSkin:
                    skin = MazeSkinEnum.Space;
                    break;
                case Constants.Skin3MazeSkin:
                    skin = MazeSkinEnum.Skin3;
                    break;
                case Constants.DefaultSnakeSkin:
                    skin = MazeSkinEnum.Default;
                    break;
                default:
                    Debug.Log("Id does not correspond to a snake skin");
                    skin = MazeSkinEnum.Error;
                    break;
            }

            return skin;
        }
        public static MazeSkinEnum StringToMazeEnum(string value)
        {
            MazeSkinEnum skin;
            switch (value)
            {
                case "Space":
                    skin = MazeSkinEnum.Space;
                    break;
                case "Skin3":
                    skin = MazeSkinEnum.Skin3;
                    break;
                case "Default":
                    skin = MazeSkinEnum.Default;
                    break;
                default:
                    Debug.Log("Id does not correspond to a snake skin");
                    skin = MazeSkinEnum.Error;
                    break;
            }

            return skin;
        }

        public static string SnakeEnumToId(SnakeSkinEnum value)
        {
            string id = String.Empty;
            switch (value)
            {
                case SnakeSkinEnum.Default:
                    id = Constants.DefaultSnakeSkin;
                    break;
                case SnakeSkinEnum.Astronaut:
                    id = Constants.AstronautSnakeSkin;
                    break;
                case SnakeSkinEnum.Skin3:
                    id = Constants.Skin3SnakeSkin;
                    break;
                case SnakeSkinEnum.Error:
                    break;
                default:
                    throw new NotEnumTypeSupportedException();
            }
            
            return id;
        }
        public static string MazeEnumToId(MazeSkinEnum value)
        {
            string id = String.Empty;
            switch (value)
            {
                case MazeSkinEnum.Default:
                    id = Constants.DefaultMazeSkin;
                    break;
                case MazeSkinEnum.Space:
                    id = Constants.SpaceMazeSkin;
                    break;
                case MazeSkinEnum.Skin3:
                    id = Constants.Skin3MazeSkin;
                    break;
                case MazeSkinEnum.Error:
                    break;
                default:
                    throw new NotEnumTypeSupportedException();
            }

            return id;
        }
    }
}
