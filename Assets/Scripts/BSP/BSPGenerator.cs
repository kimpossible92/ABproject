using System;
using System.Collections.Generic;
using SnakeMaze.Enums;
using SnakeMaze.Exceptions;
using SnakeMaze.Maze;
using SnakeMaze.SO;
using SnakeMaze.Structures;
using SnakeMaze.TileMaps;
using SnakeMaze.Utils;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnakeMaze.BSP
{
    public class BSPGenerator : MonoBehaviour
    {
        [Header("Parameter Configuration")]
        [Range(0.25f, 1.0f)]
        [Tooltip("Variation of size for rooms. The smaller this value the smaller rooms will be generated.")]
        [SerializeField] private float roomSizePerturbation;

        [Tooltip("Width of the corridors.")]
        [SerializeField] private int corridorWidth;

        [Tooltip("The maximum number of Rooms to be created = 2^numberIterations.")]
        [SerializeField] private int numberIterations;

        [Tooltip("Space for the rooms with reference to the walls / partitions.")]
        [SerializeField] private int offset = 1;

        [SerializeField] private Vector2 mapSize;
        [SerializeField] private Vector2 maxRoomSize;

        [Header("Prefabs")]
        [SerializeField] private GameObject corridorPrefab;
        [SerializeField] private Transform corridorParentT;
        [SerializeField] private GameObject exitPrefab;
        [SerializeField] private Transform roomParentT;
        [SerializeField] private TileMapVisualizer tileMapVisualizer;

        private BSPData _rootdata;
        private MazeBuilder _mazeBuilder;

        [Header("BusSO")] [SerializeField] private BusMazeManagerSO mazeManager;

        /// <summary>
        /// Structure that will store the whole information about the partitions.
        /// </summary>
        private BinaryTree<BSPData> _tree;

        // Data generated with the information stored in the Binary Tree.
        /// <summary>
        /// List with Corridor data.
        /// </summary>
        private List<Corridor> _corridorList;

        /// <summary>
        /// A room is defined by a position (center) and a size of the room. There will be a room for each partition.
        /// </summary>
        private List<Room> _roomList;

        private List<Room> _oneCorridorRooms;
        public List<Room> OneCorridorRooms
        {
            get => _oneCorridorRooms;
        }
        public List<Room> RoomList
        {
            get => _roomList;
        }

        private void Awake()
        {
            _mazeBuilder = FindObjectOfType<MazeBuilder>();
        }

        private void Start()
        {
            GenerateDungeon();
        }

        /// <summary>
        /// Destroys all rooms and corridors contained in <see cref="roomParentT"/> and <see cref="corridorParentT"/>.
        /// </summary>
        public void DeleteDungeon()
        {
            foreach (Transform corridor in corridorParentT)
            {
                Destroy(corridor.gameObject);
            }

            foreach (Transform room in roomParentT)
            {
                Destroy(room.gameObject);
            }
        }

        /// <summary>
        /// Generates a new dungeon.
        /// </summary>
        /// <remarks>
        /// You may want to call <see cref="DeleteDungeon"/> before.
        /// </remarks>
        public void GenerateDungeon()
        {
            mazeManager.StartMaze?.Invoke();

            // Putting the information related to the whole map in the tree root.
            _rootdata = new BSPData(new Bounds(Vector2.zero, new Vector3(mapSize.x, mapSize.y, 0)));
            _tree = BSP(new BinaryTree<BSPData>(_rootdata, null, null), 0);
            _corridorList = new List<Corridor>();

            // Data generation.
            // REVIEW: All the Generate methods might be joined to avoid different traversals of the same Binary tree (performance optimization).                                               
            // _corridorList = GenerateCorridorsGood(_tree);
            _roomList = GenerateRooms(_tree);
            _mazeBuilder.GenerateMazes(_roomList);
            GenerateCorridorsGood(_tree, ref _corridorList);
            GenerateExit(_roomList);
            foreach (var room in _roomList)
            {
                _mazeBuilder.PaintTheMaze(room.Grid);
            }
            mazeManager.FinishMaze?.Invoke();
        }

        private BinaryTree<BSPData> BSP(BinaryTree<BSPData> tree, int iterations)
        {
            BinaryTree<BSPData> leftChild, rightChild;
            float cutX, cutY;

            if ((tree == null) || NoSpaceForOneRoom(tree))
            {
                return null;
            }
            else
            {
                var positionVector = tree.Root.PartitionBounds.center;
                var sizeVector = tree.Root.PartitionBounds.size;

                if (ContinueDividing(tree, iterations))
                {
                    if (sizeVector.x == sizeVector.y)
                    {
                        if (Random.value < 0.5) // Divide horizontally.
                        {
                            cutY = SplitHorizontally(tree.Root);
                            leftChild = new BinaryTree<BSPData>(new BSPData(new Bounds(
                                    new Vector2(positionVector.x, (float)(positionVector.y + cutY)),
                                    new Vector2(sizeVector.x, (float)(sizeVector.y - cutY)))),
                                null,
                                null);
                            rightChild = new BinaryTree<BSPData>(new BSPData(new Bounds(positionVector,
                                    new Vector2(sizeVector.x, (float)cutY))),
                                null,
                                null);
                        }
                        else // Divide vertically.
                        {
                            cutX = SplitVertically(tree.Root);
                            leftChild = new BinaryTree<BSPData>(new BSPData(new Bounds(positionVector,
                                new Vector2((float)cutX, sizeVector.y))));
                            rightChild = new BinaryTree<BSPData>(new BSPData(new Bounds(
                                new Vector2((float)(positionVector.x + cutX), positionVector.y),
                                new Vector2((float)(sizeVector.x - cutX), sizeVector.y))));
                        }
                    }
                    else
                    {
                        if (tree.Root.PartitionBounds.size.x > tree.Root.PartitionBounds.size.y) // Divide vertically.
                        {
                            cutX = SplitVertically(tree.Root);
                            leftChild = new BinaryTree<BSPData>(new BSPData(new Bounds(positionVector,
                                new Vector2(cutX, sizeVector.y))));
                            rightChild = new BinaryTree<BSPData>(new BSPData(new Bounds(
                                new Vector2(positionVector.x + cutX, positionVector.y),
                                new Vector2(sizeVector.x - cutX, sizeVector.y))));
                        }
                        else // Divide horizontally.
                        {
                            cutY = SplitHorizontally(tree.Root);
                            leftChild = new BinaryTree<BSPData>(new BSPData(new Bounds(
                                    new Vector2(positionVector.x, positionVector.y + cutY),
                                    new Vector2(sizeVector.x, sizeVector.y - cutY))),
                                null,
                                null);
                            rightChild = new BinaryTree<BSPData>(new BSPData(new Bounds(positionVector,
                                    new Vector2(sizeVector.x, cutY))),
                                null,
                                null);
                        }
                    }

                    return new BinaryTree<BSPData>(tree.Root,
                        // (BinaryTree<BSPdata>)
                        BSP(leftChild, iterations + 1),
                        // (BinaryTree<BSPdata>)
                        BSP(rightChild, iterations + 1)
                    );
                }
                else // No more dividing.
                {
                    return tree;
                }
            }
        }

        /// <summary>
        /// If no single room can be adjusted returns true, false otherwise.
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        private bool NoSpaceForOneRoom(BinaryTree<BSPData> tree)
        {
            BSPData root = tree.Root;
            return (root.PartitionBounds.size.x < maxRoomSize.x) || (root.PartitionBounds.size.y < maxRoomSize.y);
        }

        /// <summary>
        /// If two rooms can still be added, returns true; otherwise returns false.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        private bool ContinueDividing(BinaryTree<BSPData> tree, int iterations)
        {
            BSPData root = tree.Root;

            var canTwoRoomsFitHorizontally = (root.PartitionBounds.size.x >= 2 * (maxRoomSize.x + offset))
                                             && (root.PartitionBounds.size.y >= (maxRoomSize.y + offset));

            var isHSizeBiggerThanARoom = root.PartitionBounds.size.x >= (maxRoomSize.x + offset);

            var isVSizeBiggerThanTwoRooms = root.PartitionBounds.size.y >= 2 * (maxRoomSize.y + offset);

            var doMoreIterations = iterations < numberIterations;

            return (canTwoRoomsFitHorizontally || isHSizeBiggerThanARoom && isVSizeBiggerThanTwoRooms) &&
                   doMoreIterations;
        }

        /// <summary>
        /// It returns a valid value to admit one room in each division of the space. 
        /// It considers an offset (gap with the partition's walls) to avoid exact partitions of the search space.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private float SplitHorizontally(BSPData root)
        {
            return Random.Range(maxRoomSize.y + offset, root.PartitionBounds.size.y - maxRoomSize.y - offset);
        }

        /// <summary>
        /// Returns a valid value to admit one room in each division of the space.
        /// It considers an offset (gap with the partition's walls) to avoid exact partitions of the search space.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private float SplitVertically(BSPData root)
        {
            return Random.Range(maxRoomSize.x + offset, root.PartitionBounds.size.x - maxRoomSize.x - offset);
        }
        // Creo que me daría igual si los centros son una sala o un pasillo.

        private void GenerateCorridorsGood(BinaryTree<BSPData> tree, ref List<Corridor> corridorList)
        {
            // var corridorList = new List<Corridor>();
            var rightNodeList = new List<BSPData>();
            var leftNodeList = new List<BSPData>();
            var rightNode = new BSPData();
            var leftNode = new BSPData();

            if (tree != null)
            {
                if (tree.HasTwoChilds())
                {
                    BinaryTreeUtils<BSPData>.GetAllChildren(tree.Left, ref leftNodeList);
                    BinaryTreeUtils<BSPData>.GetAllChildren(tree.Right, ref rightNodeList);
                    GetNearestNodes(leftNodeList, rightNodeList, out leftNode, out rightNode);
                    var corridorGenerated = GenerateCorridor(leftNode.StoredRoom, rightNode.StoredRoom, ref corridorList);
                    if (!corridorGenerated)
                    {

                    }
                    GenerateCorridorsGood(tree.Left, ref _corridorList);
                    GenerateCorridorsGood(tree.Right, ref _corridorList);
                }

                if (tree.Left == null)
                {
                    GenerateCorridorsGood(tree.Right, ref corridorList);
                }

                if (tree.Right == null)
                {
                    GenerateCorridorsGood(tree.Left, ref corridorList);
                }
            }

            // return corridorList;
        }

        private void GetNearestNodes(List<BSPData> leftList, List<BSPData> rightList, out BSPData finalLeftNode,
            out BSPData finalRightNode)
        {
            var minDistance = Mathf.Infinity;
            var currentRightNode = new BSPData();
            var currentLeftNode = new BSPData();
            foreach (var leftNode in leftList)
            {
                foreach (var rightNode in rightList)
                {
                    var distance = Vector2.Distance(leftNode.Center, rightNode.Center);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        currentLeftNode = leftNode;
                        currentRightNode = rightNode;
                    }
                }
            }

            finalLeftNode = currentLeftNode;
            finalRightNode = currentRightNode;
        }
        private bool GenerateCorridor(Room roomOne, Room roomTwo, ref List<Corridor> corridorList)
        {
            var roomOnePosition = roomOne.Center;
            var roomTwoPosition = roomTwo.Center;
            var minDistanceX = roomOne.Size.x / 2 + roomTwo.Size.x / 2;
            var minDistanceY = roomOne.Size.y / 2 + roomTwo.Size.y / 2;
            var relativeDistanceX = roomTwoPosition.x - roomOnePosition.x;
            var relativeDistanceY = roomTwoPosition.y - roomOnePosition.y;
            var corridorSize = 0f;


            Directions currentDirection;

            if (minDistanceX < Mathf.Abs(relativeDistanceX) && minDistanceY < Mathf.Abs(relativeDistanceY))
            {
                // Rooms don't overlap.
                return false;
            }

            if (minDistanceX > Mathf.Abs(relativeDistanceX))
            {
                // Rooms overlap in X axis.
                currentDirection = relativeDistanceY > 0 ? Directions.Up : Directions.Down;
            }
            else
            {
                // Rooms overlap in Y axis.
                currentDirection = relativeDistanceX > 0 ? Directions.Right : Directions.Left;
            }

            var corridorStart = CoordinateOfCorridorStart();
            var corridorEnd = Vector2.zero;
            // var corridorGO = Instantiate(corridorPrefab, corridorCenter, Quaternion.identity,
            //     corridorParentT);


            switch (currentDirection)
            {
                case Directions.Up:
                    corridorSize = roomTwo.BottomCenterPosition.y - roomOne.TopCenterPosition.y;
                    // corridorGO.transform.localScale = new Vector3(corridorWidth,
                    //     Mathf.Abs(corridorSize), 1);
                    corridorEnd = corridorStart + Vector2.up * (Mathf.Abs(corridorSize) + roomOne.Grid.CellSize.y);

                    tileMapVisualizer.PaintCorridorTiles(new Vector2Int((int)corridorStart.x, (int)(roomOne.TopCenterPosition.y + _mazeBuilder.CellSize.y / 2f)),
                        Directions.Up, (int)Mathf.Abs(corridorSize), false);
                    break;
                case Directions.Down:
                    corridorSize = roomTwo.TopCenterPosition.y - roomOne.BottomCenterPosition.y;
                    // corridorGO.transform.localScale = new Vector3(corridorWidth,
                    //     Mathf.Abs(corridorSize), 1);
                    corridorEnd = corridorStart - Vector2.up * (Mathf.Abs(corridorSize) + roomOne.Grid.CellSize.y);
                    tileMapVisualizer.PaintCorridorTiles(new Vector2Int((int)corridorStart.x, (int)(roomOne.BottomLeftCorner.y - _mazeBuilder.CellSize.y / 2f)),
                        Directions.Down, (int)Mathf.Abs(corridorSize), false);
                    break;
                case Directions.Right:
                    corridorSize = roomTwo.LeftCenterPosition.x - roomOne.RightCenterPosition.x;
                    // corridorGO.transform.localScale =
                    //     new Vector3(Mathf.Abs(corridorSize), corridorWidth, 1);
                    corridorEnd = corridorStart + Vector2.right * (Mathf.Abs(corridorSize) + roomOne.Grid.CellSize.x);
                    tileMapVisualizer.PaintCorridorTiles(new Vector2Int((int)(roomOne.BottomRightCorner.x + _mazeBuilder.CellSize.x / 2f), (int)corridorStart.y),
                        Directions.Right, (int)Mathf.Abs(corridorSize), corridorParentT);
                    break;
                case Directions.Left:
                    corridorSize = roomTwo.RightCenterPosition.x - roomOne.LeftCenterPosition.x;
                    // corridorGO.transform.localScale =
                    //     new Vector3(Mathf.Abs(corridorSize), corridorWidth, 1);
                    corridorEnd = corridorStart - Vector2.right * (Mathf.Abs(corridorSize) + roomOne.Grid.CellSize.x);
                    tileMapVisualizer.PaintCorridorTiles(new Vector2Int((int)(roomOne.BottomLeftCorner.x - _mazeBuilder.CellSize.x / 2f), (int)corridorStart.y),
                        Directions.Left, (int)Mathf.Abs(corridorSize), corridorParentT);
                    break;
            }

            roomOne.Grid.GetWallAtPosition(roomOne.BottomLeftCorner, corridorStart, currentDirection);
            roomTwo.Grid.GetWallAtPosition(roomTwo.BottomLeftCorner, corridorEnd,
                DirectionsActions.GetOppositeDirection(currentDirection));
            roomOne.NumberOfCorridors++;
            roomTwo.NumberOfCorridors++;
            corridorList.Add(new Corridor(roomOne.Center, roomTwo.Center, corridorWidth));

            return true;

            Vector2 CoordinateOfCorridorStart()
            {
                float lower;
                float higher;
                float coordinateX = 0, coordinateY = 0;
                float offset = corridorWidth + 0f;

                switch (currentDirection)
                {
                    case Directions.Left:
                    case Directions.Right:
                        lower = Mathf.Max(roomOne.BottomLeftCorner.y, roomTwo.BottomLeftCorner.y);
                        higher = Mathf.Min(roomOne.TopLeftCorner.y, roomTwo.TopLeftCorner.y);


                        coordinateX = roomOne.Center.x + Mathf.Sign((int)currentDirection) * roomOne.Size.x / 2f -
                                      Mathf.Sign((int)currentDirection) * 0.5f;
                        coordinateY = (int)Random.Range(lower + offset, higher - offset) + 0.5f;
                        break;
                    case Directions.Up:
                    case Directions.Down:
                        lower = Mathf.Max(roomOne.BottomLeftCorner.x, roomTwo.BottomLeftCorner.x);
                        higher = Mathf.Min(roomOne.BottomRightCorner.x, roomTwo.BottomRightCorner.x);


                        coordinateX = (int)Random.Range(lower + offset, higher - offset) + 0.5f;
                        coordinateY = roomOne.Center.y + Mathf.Sign((int)currentDirection) * roomOne.Size.y / 2f -
                                      Mathf.Sign((int)currentDirection) * 0.5f;
                        break;
                }

                return new Vector2(coordinateX, coordinateY);
            }
        }

        /// <summary>
        /// Generates and instantiates the different rooms of the dungeon.
        /// </summary>
        /// <remarks>
        /// The size of the rooms generated will be an integer since we are working with tiles. 
        /// </remarks>
        /// <param name="tree"></param>
        /// <returns></returns>
        private List<Room> GenerateRooms(BinaryTree<BSPData> tree)
        {
            var roomList = new List<Room>();

            if (tree != null)
            {
                if (tree.IsALeaf()) // The node has no childs i.e., it is a final room.
                {
                    var roomSizeXPerturbation = Random.Range(roomSizePerturbation, 1.0f);
                    var roomSizeYPerturbation = Random.Range(roomSizePerturbation, 1.0f);

                    var actualRoomSizeX = Mathf.FloorToInt(maxRoomSize.x * roomSizeXPerturbation);
                    var actualRoomSizeY = Mathf.FloorToInt(maxRoomSize.y * roomSizeYPerturbation);

                    Vector2 actualCenter;

                    if (actualRoomSizeX % 2 == 0)
                    {
                        actualCenter.x = Mathf.RoundToInt(tree.Root.Center.x);
                    }
                    else
                    {
                        // Since the size in x is odd, generate a center position which is unit and a half position to fit the tilemap perfectly.
                        actualCenter.x = UnitAndHalfPosition(tree.Root.Center.x);
                    }

                    if (actualRoomSizeY % 2 == 0)
                    {
                        actualCenter.y = Mathf.RoundToInt(tree.Root.Center.y);
                    }
                    else
                    {
                        // Since the size in y is odd, generate a center position which is unit and a half position to fit the tilemap perfectly.
                        actualCenter.y = UnitAndHalfPosition(tree.Root.Center.y);
                    }

                    var room = new Room(actualCenter, new Vector2Int(actualRoomSizeX, actualRoomSizeY));

                    tree.Root.StoredRoom = room;

                    roomList.Add(room);
                }

                roomList = ListUtils.Concat(roomList, GenerateRooms(tree.Left));
                roomList = ListUtils.Concat(roomList, GenerateRooms(tree.Right));
            }

            return roomList;

            static float UnitAndHalfPosition(float number)
            {
                var rounded = Mathf.RoundToInt(number);
                return rounded + .5f;
            }
        }

        private void InitOneCorridorRoomsList(List<Room> roomList)
        {
            _oneCorridorRooms = new List<Room>();
            foreach (var room in roomList)
            {
                if (room.NumberOfCorridors == 1)
                    _oneCorridorRooms.Add(room);
            }
        }
        private void GenerateExit(List<Room> roomList)
        {
            InitOneCorridorRoomsList(roomList);

            var exitRoom = _oneCorridorRooms[Random.Range(0, _oneCorridorRooms.Count)];
            int randomNumber = 0;
            do
            {
                randomNumber = Random.Range(-2, 2);
            } while (randomNumber == 0);

            var dir = (Directions)randomNumber;
            Vector2 cellPos = Vector2.zero;
            float xPos = 0;
            float yPos = 0;
            var offset = exitRoom.Grid.CellSize / 10f;
            switch (dir)
            {
                case Directions.Up:
                    xPos = Random.Range(exitRoom.LeftCenterPosition.x + offset.x, exitRoom.RightCenterPosition.x - offset.x);
                    cellPos = new Vector2(xPos, exitRoom.TopCenterPosition.y - offset.y);
                    break;
                case Directions.Down:
                    xPos = Random.Range(exitRoom.LeftCenterPosition.x + offset.x, exitRoom.RightCenterPosition.x - offset.x);
                    cellPos = new Vector2(xPos, exitRoom.BottomCenterPosition.y + offset.y);
                    break;
                case Directions.Right:
                    yPos = Random.Range(exitRoom.BottomCenterPosition.y + offset.y, exitRoom.TopCenterPosition.y - offset.y);
                    cellPos = new Vector2(exitRoom.RightCenterPosition.x - offset.x, yPos);
                    break;
                case Directions.Left:
                    yPos = Random.Range(exitRoom.BottomCenterPosition.y + offset.y, exitRoom.TopCenterPosition.y - offset.y);
                    cellPos = new Vector2(exitRoom.LeftCenterPosition.x + offset.x, yPos);
                    break;
                default:
                    throw new NotEnumTypeSupportedException();
            }
            var exitCell = exitRoom.Grid.GetCellAtPosition(exitRoom.BottomLeftCorner, cellPos);
            exitCell.IsExit = true;
            exitRoom.IsExitRoom = true;
            Instantiate(exitPrefab, exitCell.Position, Quaternion.identity);

        }

        public Room GetRandomRoom()
        {
            return _roomList[Random.Range(0, _roomList.Count)];
        }
    }
}