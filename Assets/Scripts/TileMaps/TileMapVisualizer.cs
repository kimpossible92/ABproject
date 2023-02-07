using System;
using System.Collections.Generic;
using SnakeMaze.Enums;
using Unity.Mathematics;
using SnakeMaze.SO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SnakeMaze.TileMaps
{
    public class TileMapVisualizer : MonoBehaviour
    {
        [SerializeField] private Tilemap wallTilemap;
        [SerializeField] private Tilemap foodTilemap;
        [SerializeField] private SkinContainerSO skinContainer;


        public void PaintWallTiles(IEnumerable<WallTile> wallTiles)
        {
            foreach (var wallTile in wallTiles)
            {
                var tile = skinContainer.CurrentMazeSkin.TileDic[wallTile.SpriteType];
                PaintSingleTile(wallTilemap, tile, wallTile.Position);
            }
        }

        public void PaintCorridorTiles(Vector2Int position, Directions direction, int amount, bool horizontalCorridor)
        {
            for (int i = 0; i < amount; i++)
            {
                var tile = horizontalCorridor ? skinContainer.CurrentMazeSkin.HorizontalCorridor : skinContainer.CurrentMazeSkin.VerticalCorridor;
                var dir = DirectionsActions.DirectionsToVector2(direction);
                var actualDir = new Vector2Int((int) dir.x, (int) dir.y);
                PaintSingleTile(wallTilemap, tile, position + actualDir * i);
            }
        }


        public void PaintExitTile(Vector2Int position)
        {
            PaintSingleTile(wallTilemap, skinContainer.CurrentMazeSkin.Exit, position);
        }

        public void PaintFoodTile(Vector2Int position)
        {
            PaintSingleTile(foodTilemap, skinContainer.CurrentMazeSkin.Food, position);
        }

        public void EraseFoodTile(Vector2Int position)
        {
            PaintSingleTile(foodTilemap, null, position);
        }

        private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
        {
            var tilePosition = tilemap.WorldToCell((UnityEngine.Vector3Int)position);
            tilemap.SetTile(tilePosition, tile);
        }
    }
}