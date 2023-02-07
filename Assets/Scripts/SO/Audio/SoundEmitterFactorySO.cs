using System;
using SnakeMaze.Audio;
using UnityEngine;

namespace SnakeMaze.SO.Audio
{
    [CreateAssetMenu(fileName = "SoundFactory",menuName = "Scriptables/Factory/SoundEmitter")]
    public class SoundEmitterFactorySO : FactorySO<SoundEmitter>
    {
        public SoundEmitter prefab;

        public override SoundEmitter Create()
        {
            return Instantiate(prefab);
        }
    }
}
