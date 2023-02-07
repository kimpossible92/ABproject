using SnakeMaze.Enums;
using SnakeMaze.SO;
using UnityEngine;

namespace SnakeMaze.Utils
{
    public class SkinManager : MonoBehaviour
    {
        [SerializeField] private SkinContainerSO skinContainerSo;

        [ContextMenu("Change To Default Skin ")]
        public void ChangeToDefaultSkin()
        {
            skinContainerSo.ChangeMazeSkin(MazeSkinEnum.Default);
            skinContainerSo.ChangeSnakeSkin(SnakeSkinEnum.Default);
        }
        [ContextMenu("Change To Space Skin ")]
        public void ChangeToMockuptSkin()
        {
            skinContainerSo.ChangeMazeSkin(MazeSkinEnum.Space);
            skinContainerSo.ChangeSnakeSkin(SnakeSkinEnum.Astronaut);
        }


    }
}
