using System;
using System.Collections;
using System.Collections.Generic;
using SnakeMaze.Audio;
using SnakeMaze.Enums;
using SnakeMaze.SO;
using SnakeMaze.SO.FoodSO;
using UnityEngine;

namespace SnakeMaze.Player
{
    public class BodyController : MonoBehaviour
    {
        [SerializeField] private GameObject snakePrefab;
        [SerializeField] private SkinContainerSO skinContainer;
        [SerializeField] private Transform headPosition;
        [SerializeField] private PlayerVariableSO player;
        [SerializeField] private AudioRequest eatRequest;
        [SerializeField] private BusFoodSO busFoodSo;
        [SerializeField] private BusGameManagerSO gameManagerSo;
        [SerializeField] private EventSO playerSpawnEvent;
        [SerializeField] private bool infiniteGrow;
        [SerializeField] private float growTime=2f;
        private List<Snake> snakeParts = new List<Snake>();
        private bool _growSnake;
        private IEnumerator _grow;

        private void Start()
        {
            _growSnake = false;
        }

        IEnumerator StartInfiniteGrowing()
        {
            while (true)
            {
                yield return new WaitForSeconds(growTime);
                GrowSnakeNextMove();
            }
        }

        private void StartGrow()
        {
            _grow = StartInfiniteGrowing();
            StartCoroutine(_grow);
        }

        private void StopInfinteGrow()
        {
            if (_grow == null) return;
                
            StopCoroutine(_grow);
        }


        private void InitBody()
        {
            var position = headPosition.position;
            InstantiateInitialBody(position - Vector3.right * player.PlayerPixels / player.PixelsPerTile,
                Directions.Right, Directions.Right);
            snakeParts[0].IsTail = false;
            snakeParts[0].CurrentSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Body.Right;
            snakeParts[0].LastSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Body.Right;
            snakeParts[0].UpdateSprite(snakeParts[0].CurrentSprite);
            InstantiateInitialBody(position - Vector3.right * 2 * player.PlayerPixels / player.PixelsPerTile,
                Directions.Right, Directions.Right);
            snakeParts[1].IsTail = false;
            snakeParts[1].CurrentSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Body.Right;
            snakeParts[1].LastSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Body.Right;
            snakeParts[1].UpdateSprite(snakeParts[1].CurrentSprite);
            InstantiateInitialBody(position - Vector3.right * 3 * player.PlayerPixels / player.PixelsPerTile,
                Directions.Right, Directions.Right);
            snakeParts[2].IsTail = true;
            snakeParts[2].CurrentSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Tail.Right;
            snakeParts[2].LastSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Tail.Right;
            snakeParts[2].UpdateSprite(snakeParts[2].CurrentSprite);
        }

        private void InstantiateInitialBody(Vector2 position, Directions currentDirections, Directions lastDirections)
        {
            var body = Instantiate(snakePrefab, position, Quaternion.identity, transform);
            var snake = body.GetComponent<Snake>();
            snake.LastDirection = lastDirections;
            snake.CurrentDirection = currentDirections;
            snakeParts.Add(snake);
        }

        [ContextMenu("Grow Snake")]
        public void GrowSnakeNextMove()
        {
            _growSnake = true;
        }

        private void InstantiateTail()
        {
            var lastTail = snakeParts[snakeParts.Count - 1];
            var pos =
                lastTail.transform.position - (Vector3) DirectionsActions.DirectionsToVector2(lastTail.LastDirection) *
                (player.PlayerPixels * 1f) / player.PixelsPerTile;
            var body = Instantiate(snakePrefab, pos, Quaternion.identity, transform);
            var snake = body.GetComponent<Snake>();
            snake.IsTail = true;
            snake.LastDirection = lastTail.LastDirection;
            snake.CurrentDirection = lastTail.LastDirection;
            snake.CurrentSprite = lastTail.LastSprite;
            snake.LastSprite = lastTail.LastSprite;
            snakeParts.Add(snake);
            TailToBody(lastTail, snakeParts.Count - 2);
            eatRequest.PlayAudio();
        }

        private void TailToBody(Snake newBody, int index)
        {
            newBody.IsTail = false;
            newBody.CurrentSprite = GetActualSprite(index);
            newBody.UpdateSprite(newBody.CurrentSprite);
        }


        public void MoveSnakeBody()
        {
            snakeParts[snakeParts.Count - 1].Move(snakeParts[snakeParts.Count - 2].LastDirection);


            for (int i = snakeParts.Count - 2; i >= 1; i--)
            {
                snakeParts[i].Move(snakeParts[i - 1].LastDirection);
            }

            snakeParts[0].Move(player.LastDirection);

            snakeParts[0].LastDirection = snakeParts[0].CurrentDirection;
            snakeParts[0].CurrentDirection = player.CurrentDirection;
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

            if (_growSnake)
            {
                InstantiateTail();
                _growSnake = false;
            }
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
                return GetActualBodySprite(snakeParts[1].CurrentDirection, player.CurrentDirection);

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

        private void OnEnable()
        {
            if(infiniteGrow)
            {
                gameManagerSo.StartGame += StartGrow;
                gameManagerSo.PlayerDeath += StopInfinteGrow;
            }
            busFoodSo.OnEatFoodNoArg += GrowSnakeNextMove;
            playerSpawnEvent.CurrentAction += InitBody;

        }

        private void OnDisable()
        {
            if(infiniteGrow)
            {
                gameManagerSo.StartGame -= StartGrow;
                gameManagerSo.PlayerDeath -= StopInfinteGrow;
            }
            busFoodSo.OnEatFoodNoArg -= GrowSnakeNextMove;
            playerSpawnEvent.CurrentAction -= InitBody;
        }
    }
}