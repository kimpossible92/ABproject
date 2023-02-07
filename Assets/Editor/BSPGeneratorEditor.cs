using UnityEngine;
using UnityEditor;
using SnakeMaze.BSP;

namespace SnakeMaze.Editors
{
    [CustomEditor(typeof(BSPGenerator))]
    public class BSPGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            _ = DrawDefaultInspector();

            BSPGenerator targetBSPGenerator = (BSPGenerator)target;

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Delete & Generate Dungeon"))
                {
                    targetBSPGenerator.DeleteDungeon();
                    targetBSPGenerator.GenerateDungeon();
                }
            }
        }
    }
}