using SnakeMaze.BSP;
using SnakeMaze.SO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnakeMaze.Player
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private BusMazeManagerSO mazeManager;
        [SerializeField] private EventSO playerSpawnEvent;
        private BSPGenerator _bspGenerator;
        private Transform _player;


        private void Awake()
        {
            _bspGenerator = FindObjectOfType<BSPGenerator>();
            _player = FindObjectOfType<PlayerController>().gameObject.transform;
        }

        private void SpawnPlayer()
        {
            Room room = null;
            do
            {
                room = _bspGenerator.OneCorridorRooms[Random.Range(0, _bspGenerator.OneCorridorRooms.Count)];
            } while (room.IsExitRoom);
            var pos = room.Grid.GetCellAtPosition(room.BottomLeftCorner,room.Center).Position;
            _player.position = pos;
            playerSpawnEvent.CurrentAction?.Invoke();
        }

        private void OnEnable()
        {
            mazeManager.FinishMaze += SpawnPlayer;
        }

        private void OnDisable()
        {
            mazeManager.FinishMaze -= SpawnPlayer;
        }
    }
}
