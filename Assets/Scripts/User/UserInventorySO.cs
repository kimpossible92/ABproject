using System.Collections.Generic;
using PlayFab.ClientModels;
using SnakeMaze.Enums;
using SnakeMaze.PlayFab;
using SnakeMaze.SO;
using SnakeMaze.SO.Items;
using SnakeMaze.Utils;
using UnityEngine;

namespace SnakeMaze.User
{
    [CreateAssetMenu(fileName = "UserInventory", menuName = "Scriptables/User/UserInventorySO")]
    public class UserInventorySO : InitiableSO
    {
        [SerializeField] private CatalogSO catalogSo;
        private Dictionary<string, SnakeSkinEnum> _snakeDictionary;
        private Dictionary<string, MazeSkinEnum> _mazeDictionary;
        public Dictionary<string, SnakeSkinEnum> SnakeDictionary => _snakeDictionary;
        public Dictionary<string, MazeSkinEnum> MazeDictionary => _mazeDictionary;

        public void LoadInventory(LoginDataResult loginData)
        {
            AddSkinToDictionary(loginData.loginData.inventory);
            Debug.Log("Inventory successfully loaded");
        }

        public void AddSkinToDictionary(ItemInstance item)
        {
            catalogSo.CatalogList.Find(catalogItem => catalogItem.ItemId == item.ItemId).Available = true;
            switch (item.ItemClass)
            {
                case Constants.SnakeSkin:

                    if (item.CustomData != null && !_snakeDictionary.ContainsKey(item.ItemId))
                    {
                        _snakeDictionary.Add(
                            item.ItemId,
                            SkinEnumUtils.StringToSnakeEnum(item.CustomData[Constants.SkinType]));

                        Debug.Log($"{item.ItemId} successfully loaded");
                    }
                    else
                    {
                        Debug.Log($"Error loading to inventory {item.ItemId} with CustomData {item.CustomData}");
                    }

                    break;
                case Constants.MazeSkin:

                    if (item.CustomData != null && !_mazeDictionary.ContainsKey(item.ItemId))
                    {
                        _mazeDictionary.Add(
                            item.ItemId,
                            SkinEnumUtils.StringToMazeEnum(item.CustomData[Constants.SkinType]));

                        Debug.Log($"{item.ItemId} successfully loaded");
                    }
                    else
                    {
                        Debug.Log($"Error loading to inventory {item.ItemId} with CustomData {item.CustomData}");
                    }

                    break;
            }
        }

        public void AddSkinToDictionary(IEnumerable<ItemInstance> items)
        {
            Debug.Log("Adding: " + items);
            foreach (var itemInstance in items)
            {
                Debug.Log("Adding: " + itemInstance);
                AddSkinToDictionary(itemInstance);
            }
        }

        public override void InitScriptable()
        {
            InitDics();
        }

        private void InitDics()
        {
            _snakeDictionary = new Dictionary<string, SnakeSkinEnum>()
            {
                {"Snake_Skin_Default", SnakeSkinEnum.Default}
            };

            _mazeDictionary = new Dictionary<string, MazeSkinEnum>()
            {
                {"Maze_Skin_Default", MazeSkinEnum.Default}
            };

            Debug.Log("Inventory initiated");
        }
    }
}