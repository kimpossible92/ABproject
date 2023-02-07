using SnakeMaze.SO.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeMaze.UI
{
    [RequireComponent( typeof(Button))]
    public class MuteSfxButton : MonoBehaviour
    {
        [SerializeField] private BusAudioSO sfxBusAudioSo;
        [SerializeField] private GameObject muteBar;

        private bool _isMutted;

        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            SetInitialSprite();
        }

        private void SetInitialSprite()
        {
            _isMutted = PlayerPrefs.GetInt("MuteSfx") == 1;
            SetSprite(_isMutted);
        }

        private void SetSprite(bool value)
        {
            muteBar.SetActive(value);
        }

        public void SwitchMute()
        {
            _isMutted = !_isMutted;

            SetSprite(_isMutted);
            sfxBusAudioSo.MuteAudio?.Invoke(_isMutted);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(SwitchMute);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(SwitchMute);
        }
    }
}
