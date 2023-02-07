using UnityEngine;

namespace SnakeMaze.SO
{
    public abstract class ResseteableSO : ScriptableObject
    {
        public abstract void ResetValues();
    }
    public abstract class InitiableSO : ScriptableObject
    {
            public abstract void InitScriptable();
    }

    public abstract class InitAndResetSO : ScriptableObject
    {
        public abstract void ResetValues();
        public abstract void InitScriptable();
    }
}
