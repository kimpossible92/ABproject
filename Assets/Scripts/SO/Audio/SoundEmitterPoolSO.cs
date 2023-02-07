using UnityEngine;
using SnakeMaze.Audio;
using SnakeMaze.Interfaces;

namespace SnakeMaze.SO.Audio
{
    [CreateAssetMenu(fileName = "SoundEmitterPool", menuName = "Scriptables/Audio/SoundEmitterPoolSO")]
    public class SoundEmitterPoolSO : ComponentPoolSO<SoundEmitter>
    {
        [SerializeField] private SoundEmitterFactorySO factory;

        public override IFactory<SoundEmitter> Factory
        {
            get => factory;
            set => factory = (SoundEmitterFactorySO)value;
        }
        public override void ResetValues()
        {
            HasBeenPrewarmed = false;
        }
    }
}