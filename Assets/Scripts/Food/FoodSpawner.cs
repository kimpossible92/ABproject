using System.Collections;
using System.Collections.Generic;
using SnakeMaze.BSP;
using SnakeMaze.Maze;
using SnakeMaze.SO;
using SnakeMaze.SO.FoodSO;
using SnakeMaze.TileMaps;
using UnityEngine;

namespace SnakeMaze.Food
{
    public class FoodSpawner : MonoBehaviour
    {
        [SerializeField] private BusFoodSO busFood;
        [SerializeField] private BusMazeManagerSO busMazeManagerSo;
        [SerializeField] private BusGameManagerSO busGameManagerSo;
        [SerializeField] private FoodPool pool;
        [SerializeField] private TileMapVisualizer tileMapVisualizer;
        [SerializeField] private int initFoodNumber = 30;

        /// <summary>
        /// Time before spawning food after being eaten.
        /// </summary>
        [Tooltip("Time before spawning food after being eaten.")] [Header("Settings")] [SerializeField]
        private int delayTime;

        private BSPGenerator _bspGenerator;
        private IEnumerator _randomSpawn;

        private int _maxFood;

        private void Awake()
        {
            _bspGenerator = FindObjectOfType<BSPGenerator>();
        }

        private void OnEnable()
        {
            busMazeManagerSo.FinishMaze += InitFood;
            busFood.OnEatFood += DespawnFood;
            busGameManagerSo.PlayerDeath += StopAllCoroutines;
            busGameManagerSo.WinGame += StopAllCoroutines;
        }

        private void OnDisable()
        {
            busMazeManagerSo.FinishMaze -= InitFood;
            busFood.OnEatFood -= DespawnFood;
            busGameManagerSo.PlayerDeath -= StopAllCoroutines;
            busGameManagerSo.WinGame -= StopAllCoroutines;
            StopAllCoroutines();
        }

        private void InitFood()
        {
            _maxFood = 0;
            for (int i = 0; i < _bspGenerator.RoomList.Count; i++)
            {
                _maxFood += _bspGenerator.RoomList[i].NumberOfCells;
            }
            pool.Prewarm(_maxFood);
            pool.SetParent(transform);
            var roomFoodAmount = initFoodNumber / _bspGenerator.RoomList.Count;
            var rest = initFoodNumber % _bspGenerator.RoomList.Count;
            MazeCell[] cellList = new MazeCell[roomFoodAmount];
            int actualRoomFoodAmount=0;
            foreach (var room in _bspGenerator.RoomList)
            {
                actualRoomFoodAmount = Mathf.Clamp(roomFoodAmount, 0, room.NumberOfCells);
                for (int i = 0; i < actualRoomFoodAmount; i++)
                {
                    do
                    {
                        cellList[i] = room.Grid.GetRandomCell();
                    } while (cellList[i].HasFood);

                    cellList[i].HasFood = true;
                }

                SpawnFood(cellList);
                room.NumberOfFood += actualRoomFoodAmount;
            }

            if (rest == 0) return;
            cellList = new MazeCell[rest];
            Room currentRoom = null;
            int security=0;
            for (int i = 0; i < rest; i++)
            {
                security = 0;
                do
                { 
                    currentRoom = _bspGenerator.GetRandomRoom();
                    if(security==20)
                        break;
                    security++;

                } while (currentRoom.NumberOfFood>=currentRoom.NumberOfCells);
                if(security==20)
                    break;
                do
                {
                    cellList[i] = currentRoom.Grid.GetRandomCell();
                } while (cellList[i].HasFood);

                cellList[i].HasFood = true;
                currentRoom.NumberOfFood++;
                
            }

            SpawnFood(cellList);
        }

        private void SpawnFood(MazeCell cell)
        {
            var food = pool.Request();
            food.Cell = cell;
            food.transform.position = cell.Position;
            var tilePos = new Vector2Int((int) cell.Position.x, (int) cell.Position.y);
            tileMapVisualizer.PaintFoodTile(tilePos);
        }

        private void SpawnFood(MazeCell[] cells)
        {
            List<FoodController> food = new List<FoodController>();
            food = (List<FoodController>) pool.Request(cells.Length);
            for (int i = 0; i < cells.Length; i++)
            {
                if(cells[i]!=null)
                {
                    food[i].Cell = cells[i];
                    food[i].transform.position = cells[i].Position;
                    var tilePos = new Vector2Int((int) cells[i].Position.x, (int) cells[i].Position.y);
                    tileMapVisualizer.PaintFoodTile(tilePos);
                }
            }
        }

        private void RandomSpawnFood()
        {
            MazeCell cell = null;
            do
            {
                cell = _bspGenerator.GetRandomRoom().Grid.GetRandomCell();
            } while (cell.HasFood);

            SpawnFood(cell);
        }

        private void RandomSpawnFood(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                MazeCell cell = null;
                do
                {
                    cell = _bspGenerator.GetRandomRoom().Grid.GetRandomCell();
                } while (cell.HasFood);

                SpawnFood(cell);
            }
        }

        private void DespawnFood(FoodController food)
        {
            food.Cell.HasFood = false;
            food.Cell.CurrentRoom.NumberOfFood--;
            var tilePos = new Vector2Int((int) food.Cell.Position.x, (int) food.Cell.Position.y);
            tileMapVisualizer.EraseFoodTile(tilePos);
            food.Cell = null;
            pool.Return(food);
            StartRandomRespawn();
        }

        private void DespawnFood(List<FoodController> food)
        {
            foreach (var item in food)
            {
                item.Cell.HasFood = false;
                item.Cell.CurrentRoom.NumberOfFood--;
                var tilePos = new Vector2Int((int) item.Cell.Position.x, (int) item.Cell.Position.y);
                tileMapVisualizer.EraseFoodTile(tilePos);
                item.Cell = null;
            }

            pool.Return(food);
            StartRandomRespawn(food.Count);
        }
        private void StartRandomRespawn()
        {
            _randomSpawn = RespawnFood();
            StartCoroutine(_randomSpawn);
        }

        IEnumerator RespawnFood()
        {
            yield return new WaitForSeconds(delayTime);
            RandomSpawnFood();
        }
        private void StartRandomRespawn(int amount)
        {
            _randomSpawn = RespawnFood(amount);
            StartCoroutine(_randomSpawn);
        }

        IEnumerator RespawnFood(int amount)
        {
            yield return new WaitForSeconds(delayTime);
            RandomSpawnFood(amount);
        }
    }
}