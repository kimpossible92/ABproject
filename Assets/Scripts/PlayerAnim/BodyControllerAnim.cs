using System.Collections.Generic;
using SnakeMaze.Enums;
using SnakeMaze.Player;
using SnakeMaze.SO;
using UnityEngine;

namespace SnakeMaze.PlayerAnim
{
    public class BodyControllerAnim : MonoBehaviour
    {
        [SerializeField] private GameObject snakePrefab;
        [SerializeField] private SkinContainerSO skinContainer;
        [SerializeField] private Transform headPosition;
        [SerializeField] private PlayerVariableSO player;
        [SerializeField] private PlayerControllerAnim playerController;
        private List<Snake> snakeParts = new List<Snake>();

        private void Start()
        {
            InitBody();
            playerController.StartMoving();
        }

        public void InitBody()
        {
            var position = headPosition.position;
            for (int i = 0; i < playerController.SquareNumber-1; i++)
            {
                InstantiateInitialBody(position - Vector3.right *(i+1)* player.PlayerPixels / player.PixelsPerTile,
                    Directions.Right, Directions.Right);
                snakeParts[i].IsTail = false;
                snakeParts[i].CurrentSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Body.Right;
                snakeParts[i].LastSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Body.Right;
                snakeParts[i].UpdateSprite(snakeParts[i].CurrentSprite);
            }
            InstantiateInitialBody(position - Vector3.right *playerController.SquareNumber* player.PlayerPixels / player.PixelsPerTile,
                Directions.Right, Directions.Right);
            snakeParts[playerController.SquareNumber-1].IsTail = true;
            snakeParts[playerController.SquareNumber-1].CurrentSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Tail.Right;
            snakeParts[playerController.SquareNumber-1].LastSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Tail.Right;
            snakeParts[playerController.SquareNumber-1].UpdateSprite(snakeParts[playerController.SquareNumber-1].CurrentSprite);
        }

        private void InstantiateInitialBody(Vector2 position, Directions currentDirections, Directions lastDirections)
        {
            var body = Instantiate(snakePrefab, position, Quaternion.identity, transform);
            var snake = body.GetComponent<Snake>();
            snake.LastDirection = lastDirections;
            snake.CurrentDirection = currentDirections;
            snakeParts.Add(snake);
        }

        public void MoveSnakeBody()
        {
            snakeParts[snakeParts.Count - 1].Move(snakeParts[snakeParts.Count - 2].LastDirection);


            for (int i = snakeParts.Count - 2; i >= 1; i--)
            {
                snakeParts[i].Move(snakeParts[i - 1].LastDirection);
            }

            snakeParts[0].Move(playerController.LastDirection);

            snakeParts[0].LastDirection = snakeParts[0].CurrentDirection;
            snakeParts[0].CurrentDirection = playerController.CurrentDirection;
            for (int i = 1; i < snakeParts.Count - 1; i++)
            {
                snakeParts[i].LastDirection = snakeParts[i].CurrentDirection;
                snakeParts[i].CurrentDirection = snakeParts[i - 1].LastDirection;
            }

            snakeParts[snakeParts.Count - 1].LastDirection = snakeParts[snakeParts.Count - 1].CurrentDirection;
            snakeParts[snakeParts.Count - 1].CurrentDirection = snakeParts[snakeParts.Count - 2].LastDirection;

            snakeParts[0].LastSprite = snakeParts[0].CurrentSprite;
            snakeParts[0].CurrentSprite = GetActualSprite(0);
            snakeParts[0].UpdateSprite(snakeParts[0].CurrentSprite);

            if (snakeParts.Count > 2)
            {
                for (int i = 1; i < snakeParts.Count - 1; i++)
                {
                    snakeParts[i].LastSprite = snakeParts[i].CurrentSprite;
                    snakeParts[i].CurrentSprite = snakeParts[i - 1].LastSprite;
                    snakeParts[i].UpdateSprite(snakeParts[i].CurrentSprite);
                }
            }

            snakeParts[snakeParts.Count - 1].LastSprite = snakeParts[snakeParts.Count - 1].CurrentSprite;
            snakeParts[snakeParts.Count - 1].CurrentSprite = GetActualSprite(snakeParts.Count - 1);
            snakeParts[snakeParts.Count - 1].UpdateSprite(snakeParts[snakeParts.Count - 1].CurrentSprite);
        }

        private Sprite GetActualTailSprite(Directions followingDirection)
        {
            var sprite = followingDirection switch
            {
                Directions.Up => skinContainer.CurrentSnakeSkin.SnakeSkin.Tail.Up,
                Directions.Down => skinContainer.CurrentSnakeSkin.SnakeSkin.Tail.Down,
                Directions.Right => skinContainer.CurrentSnakeSkin.SnakeSkin.Tail.Right,
                Directions.Left => skinContainer.CurrentSnakeSkin.SnakeSkin.Tail.Left,
                _ => skinContainer.CurrentSnakeSkin.SnakeSkin.Tail.Up
            };
            return sprite;
        }

        private Sprite GetActualSprite(int index)
        {
            if (index == 0)
                return GetActualBodySprite(snakeParts[1].CurrentDirection, playerController.CurrentDirection);

            if (index == snakeParts.Count - 1)
                return GetActualTailSprite(snakeParts[snakeParts.Count - 2].LastDirection);

            return snakeParts[index - 1].LastSprite;
        }

        private Sprite GetActualBodySprite(Directions previousDir, Directions followingDir)
        {
            Sprite sprite = null;
            //If previous dir is horizontal and following dir is vertical or vice versa.
            if ((Mathf.Abs((int) previousDir) == 1 && Mathf.Abs((int) followingDir) == 2) ||
                (Mathf.Abs((int) previousDir) == 2 && Mathf.Abs((int) followingDir) == 1))
            {
                if (previousDir == Directions.Down)
                {
                    sprite = followingDir == Directions.Left
                        ? skinContainer.CurrentSnakeSkin.SnakeSkin.Body.CornerTopLeft
                        : skinContainer.CurrentSnakeSkin.SnakeSkin.Body.CornerTopRight;
                }

                if (previousDir == Directions.Up)
                {
                    sprite = followingDir == Directions.Left
                        ? skinContainer.CurrentSnakeSkin.SnakeSkin.Body.CornerBottomLeft
                        : skinContainer.CurrentSnakeSkin.SnakeSkin.Body.CornerBottomRight;
                }

                if (previousDir == Directions.Right)
                {
                    sprite = followingDir == Directions.Down
                        ? skinContainer.CurrentSnakeSkin.SnakeSkin.Body.CornerBottomLeft
                        : skinContainer.CurrentSnakeSkin.SnakeSkin.Body.CornerTopLeft;
                }

                if (previousDir == Directions.Left)
                {
                    sprite = followingDir == Directions.Down
                        ? skinContainer.CurrentSnakeSkin.SnakeSkin.Body.CornerBottomRight
                        : skinContainer.CurrentSnakeSkin.SnakeSkin.Body.CornerTopRight;
                }
            }
            //If previous dir is horizontal
            else if (Mathf.Abs((int) previousDir) == 2)
            {
                sprite = followingDir == Directions.Left
                    ? skinContainer.CurrentSnakeSkin.SnakeSkin.Body.Left
                    : skinContainer.CurrentSnakeSkin.SnakeSkin.Body.Right;
            }
            //Following dir is vertical
            else
            {
                sprite = followingDir == Directions.Down
                    ? skinContainer.CurrentSnakeSkin.SnakeSkin.Body.Down
                    : skinContainer.CurrentSnakeSkin.SnakeSkin.Body.Up;
            }

            return sprite;
        }
    }
}