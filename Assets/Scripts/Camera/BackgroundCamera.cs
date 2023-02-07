using System;
using SnakeMaze.SO;
using UnityEngine;

namespace SnakeMaze.CameraUtil
{
    [RequireComponent(typeof(Camera))]
    public class BackgroundCamera : MonoBehaviour
    {
        [SerializeField] private SkinContainerSO skinContainerSo;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Start()
        {
            _camera.backgroundColor = skinContainerSo.CurrentMazeSkin.BackgroundColor;
        }
    }
}
