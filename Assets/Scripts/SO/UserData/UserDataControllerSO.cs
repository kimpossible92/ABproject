using SnakeMaze.Enums;
using SnakeMaze.PlayFab;
using SnakeMaze.SO.PlayFabManager;
using SnakeMaze.User;
using SnakeMaze.Utils;
using UnityEngine;

namespace SnakeMaze.SO.UserDataSO
{
    [CreateAssetMenu(fileName = "UserData", menuName = "Scriptables/User/UserDataControllerSO")]
    public class UserDataControllerSO : ScriptableObject
    {
        [SerializeField] private EventSO playFabServerResponse;
        [SerializeField] private EventSO updateCoins;
        [SerializeField] private BusSelectSkinSO busSnakeSelectSkinSo;
        [SerializeField] private BusSelectSkinSO busMazeSelectSkinSo;
        [SerializeField] private SkinContainerSO skinContainerSo;
        [SerializeField] private PlayFabManagerSO playFabManagerSo;

        private ScoreData _scoreData = new ScoreData();
        private EconomyData _economyData = new EconomyData();
        private CurrentSkinData _skinData = new CurrentSkinData();
        private string _nickName;

        public string NickName
        {
            get => _nickName;
            set => _nickName = value;
        }

        public int HighScore
        {
            get => _scoreData.Score;
            set => _scoreData.Score = value;
        }

        public int SoftCoins
        {
            get => _economyData.SoftCoin;
            set
            {
                _economyData.SoftCoin = value;
                updateCoins.CurrentAction?.Invoke();
            }
        }

        public int HardCoins
        {
            get => _economyData.HardCoin;
            set
            {
                _economyData.HardCoin = value;
                updateCoins.CurrentAction?.Invoke();
            }
        }

        public SnakeSkinEnum CurrentSnakeSkin
        {
            get => SkinEnumUtils.StringToSnakeEnum(_skinData.Snake);
            set => _skinData.Snake = value.ToString();
        }

        public MazeSkinEnum CurrentMazeSkin
        {
            get => SkinEnumUtils.StringToMazeEnum(_skinData.Maze);
            set => _skinData.Maze = value.ToString();
        }

        private void SetCurrentSnakeSkin(SnakeSkinEnum snakeSkin)
        {
            CurrentSnakeSkin = snakeSkin;
            playFabManagerSo.UpdateCurrentSkins(_skinData.Snake, _skinData.Maze);
        }

        private void SetCurrentMazeSkin(MazeSkinEnum mazeSkin)
        {
            CurrentMazeSkin = mazeSkin;
            playFabManagerSo.UpdateCurrentSkins(_skinData.Snake, _skinData.Maze);
        }

        private void SetCurrentSnakeSkin(string snakeSkin)
        {
            _skinData.Snake = snakeSkin;
        }

        private void SetCurrentMazeSkin(string mazeSkin)
        {
            _skinData.Maze = mazeSkin;
        }

        public void LoadData(LoginDataResult loginData)
        {
            JsonUtility.FromJsonOverwrite(loginData.loginData.readOnlyData["HighScore"].Value, _scoreData);
            Debug.Log(loginData.loginData.readOnlyData["Skins"].Value);
            JsonUtility.FromJsonOverwrite(loginData.loginData.readOnlyData["Skins"].Value, _skinData);

            _economyData.SetEconomyData(loginData.loginData.currency);

            Debug.Log("SoftCoins: " + SoftCoins);
            Debug.Log("HardCoins: " + HardCoins);
            Debug.Log("CurrentSnakeSkin: " + _skinData.Snake);
            Debug.Log("CurrentMazeSkin: " + _skinData.Maze);
            skinContainerSo.ChangeSnakeSkin(SkinEnumUtils.StringToSnakeEnum(_skinData.Snake));
            skinContainerSo.ChangeMazeSkin(SkinEnumUtils.StringToMazeEnum(_skinData.Maze));
            playFabServerResponse.CurrentAction.Invoke();
        }

        private void OnEnable()
        {
            busSnakeSelectSkinSo.OnSnakeSkinSelect += SetCurrentSnakeSkin;
            busMazeSelectSkinSo.OnMazeSkinSelect += SetCurrentMazeSkin;
        }

        private void OnDisable()
        {
            busSnakeSelectSkinSo.OnSnakeSkinSelect -= SetCurrentSnakeSkin;
            busMazeSelectSkinSo.OnMazeSkinSelect -= SetCurrentMazeSkin;
        }
    }
}