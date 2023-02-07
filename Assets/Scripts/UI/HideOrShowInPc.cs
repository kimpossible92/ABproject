using System;
using UnityEngine;

namespace SnakeMaze.UI
{
    public class HideOrShowInPc : MonoBehaviour
    {
        [SerializeField] private bool showInPc;
        [SerializeField] private bool showInEditor;

        private void Awake()
        {
            CheckDevice();
        }

        private void CheckDevice()
        {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            gameObject.SetActive(showInPc);
#endif
#if UNITY_EDITOR
            gameObject.SetActive(showInEditor);
#endif
        }
    }
}