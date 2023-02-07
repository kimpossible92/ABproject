using System;
using SnakeMaze.Enums;
using SnakeMaze.Exceptions;
using SnakeMaze.SO;
using SnakeMaze.SO.PlayFabManager;
using SnakeMaze.SO.UserDataSO;
using SnakeMaze.User;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeMaze.UI
{
    [RequireComponent(typeof(Button))]
    public class SelectButton : MonoBehaviour
    {
        [SerializeField] private AbstractSkinItemSO item;
        [SerializeField] private UserInventorySO inventorySo;
        [SerializeField] private UserDataControllerSO userDataControllerSo;
        /// <summary>
        /// If the item is a snake skin, put the snake bus, otherwise put the maze bus.
        /// </summary>
        [Tooltip("If the item is a snake skin, put the snake bus, otherwise put the maze bus.")]
        [SerializeField] private BusSelectSkinSO busSelectSkinSo;
        [SerializeField] private TextMeshProUGUI price;
        
        private Button _selectButton;

        public Button MyButton
        {
            get => _selectButton;
            set => _selectButton = value;
        }

        public AbstractSkinItemSO Item
        {
            get => item;
            set => item = value;
        }

        private void Awake()
        {
            _selectButton = GetComponent<Button>();
            
        }
        private void HidePrice()
        {
            price.gameObject.SetActive(false);
        }

        public void SelectItem()
        {
            if (inventorySo.SnakeDictionary.TryGetValue(item.ItemId, out var snakeSkin))
            {
                busSelectSkinSo.OnSnakeSkinSelect?.Invoke(snakeSkin);
                busSelectSkinSo.OnButtonSelect?.Invoke(item.ItemId);
            }
            else
            {
                if(inventorySo.MazeDictionary.TryGetValue(item.ItemId, out var mazeSkin))
                {
                    busSelectSkinSo.OnMazeSkinSelect?.Invoke(mazeSkin);
                    busSelectSkinSo.OnButtonSelect?.Invoke(item.ItemId);
                }
            }
            
        }

        public void CheckButtonState(string itemID)
        {
            _selectButton.interactable = itemID != item.ItemId;
        }
        public void CheckButtonState(string snakeId, string mazeId)
        {
            _selectButton.interactable = snakeId != item.ItemId && mazeId != item.ItemId;
        }

        private void OnEnable()
        {
            _selectButton.onClick.AddListener(SelectItem);
            busSelectSkinSo.OnButtonSelect += CheckButtonState;
            CheckButtonState(SkinEnumUtils.SnakeEnumToId(userDataControllerSo.CurrentSnakeSkin),SkinEnumUtils.MazeEnumToId(userDataControllerSo.CurrentMazeSkin));
            HidePrice();
        }

        private void OnDisable()
        {
            _selectButton.onClick.RemoveListener(SelectItem);
            busSelectSkinSo.OnButtonSelect -= CheckButtonState;
        }
        
    }
}