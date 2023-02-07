using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using SnakeMaze.SO;
using SnakeMaze.SO.FoodSO;
using UnityEngine;

namespace SnakeMaze.CameraShake
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float dieDuration=1;
        [SerializeField] private float dieAmplitud=1.2f;
        [SerializeField] private float dieFrecuency=2f;
        [SerializeField] private float eatDuration=1;
        [SerializeField] private float eatAmplitud=1.2f;
        [SerializeField] private float eatFrecuency=2f;
        [SerializeField] private CinemachineVirtualCamera virtaulCamera;
        [SerializeField] private BusGameManagerSO gameManagerSo;
        [SerializeField] private BusFoodSO busFoodSo;
        private CinemachineBasicMultiChannelPerlin _noise;
        private IEnumerator _dieShake;
        private IEnumerator _eatShake;
        private void Awake()
        {
            _noise = virtaulCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void StartDieShacke()
        {
            _dieShake = DieShake();
            StartCoroutine(_dieShake);
        }
        private void StartEatShake()
        {
            _eatShake = EatShake();
            StartCoroutine(_eatShake);
        }
        private IEnumerator DieShake()
        {
            _noise.m_AmplitudeGain = dieAmplitud;
            _noise.m_FrequencyGain = dieFrecuency;
            yield return new WaitForSeconds(dieDuration);
            _noise.m_AmplitudeGain = 0;
            _noise.m_FrequencyGain = 0;
        }
        private IEnumerator EatShake()
        {
            _noise.m_AmplitudeGain = eatAmplitud;
            _noise.m_FrequencyGain = eatFrecuency;
            yield return new WaitForSeconds(eatDuration);
            _noise.m_AmplitudeGain = 0;
            _noise.m_FrequencyGain = 0;
        }
        private void OnEnable()
        {
            gameManagerSo.PlayerDeath += StartDieShacke;
            busFoodSo.OnEatFoodNoArg += StartEatShake;
        }

        private void OnDisable()
        {
            gameManagerSo.PlayerDeath -= StartDieShacke;
            busFoodSo.OnEatFoodNoArg -= StartEatShake;
        }
    }
}
