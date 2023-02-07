using System.Collections;
using SnakeMaze.Enums;
using SnakeMaze.Exceptions;
using SnakeMaze.Player;
using SnakeMaze.SO;
using UnityEngine;

namespace SnakeMaze.PlayerAnim
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerControllerAnim : PlayerPhysics
    {
        [SerializeField] private SkinContainerSO skinContainer;
        [SerializeField] private float velocity;
        [SerializeField] private int squareNumber;

        public int SquareNumber
        {
            get => squareNumber;
            set => squareNumber = value;
        }

        private BodyControllerAnim _bodyController;
        private SpriteRenderer _spriteRenderer;
        private IEnumerator _moveCoroutine;
        private Directions _currentDirection = Directions.Right;
        private Directions _lastDirection = Directions.Right;
        private Directions _nextDirection = Directions.Right;

        public Directions CurrentDirection
        {
            get => _currentDirection;
            set => _currentDirection = value;
        }
        public Directions LastDirection
        {
            get => _lastDirection;
            set => _lastDirection = value;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _bodyController = FindObjectOfType<BodyControllerAnim>();
            _currentDirection = Directions.Right;
            _lastDirection = Directions.Right;
            _nextDirection = Directions.Right;
        }

        public void StartMoving()
        {
            _moveCoroutine = Move();
            StartCoroutine(_moveCoroutine);
        }

        private void StopMoving()
        {
            StopCoroutine(_moveCoroutine);
        }

        private void SetMoving(bool pause)
        {
            if (pause)
                StopMoving();
            else
                StartMoving();
        }

        private IEnumerator Move()
        {
            var index = 0;
            while (true)
            {
                _lastDirection = _currentDirection;
                _currentDirection = _nextDirection;
                SetHeadSprite(_currentDirection);
                Move(_currentDirection);
                if (index < squareNumber)
                    index++;
                else
                {
                    index = 0;
                    _nextDirection = _currentDirection switch
                    {
                        Directions.Right => Directions.Down,
                        Directions.Down => Directions.Left,
                        Directions.Left => Directions.Up,
                        Directions.Up => Directions.Right,
                        _ => throw new NotEnumTypeSupportedException()
                    };
                }
                _bodyController.MoveSnakeBody();
                yield return new WaitForSeconds(velocity);
            }
        }

        private void SetHeadSprite(Directions direction)
        {
            Sprite currentSprite = null;
            switch (direction)
            {
                case Directions.Up:
                    currentSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Head.Up;
                    break;
                case Directions.Down:
                    currentSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Head.Down;
                    break;
                case Directions.Right:
                    currentSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Head.Right;
                    break;
                case Directions.Left:
                    currentSprite = skinContainer.CurrentSnakeSkin.SnakeSkin.Head.Left;
                    break;
            }

            _spriteRenderer.sprite = currentSprite;
        }
// #if UNITY_EDITOR
//         private void OnDrawGizmos()
//         {
//             
//             Gizmos.color = Color.green;
//             Gizmos.DrawLine(transform.position,
//                 transform.position + (Vector3) DirectionsActions.DirectionsToVector2(_currentDirection)  *
//                 (playerVariable.PlayerPixels / 2f / playerVariable.PixelsPerTile * 1.5f));
//         }
// #endif
    }
}