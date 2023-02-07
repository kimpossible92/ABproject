using System.Collections.Generic;
using UnityEngine;

namespace SnakeMaze.SO
{
    [CreateAssetMenu(fileName = "SOManager", menuName = "Scriptables/SOManager")]
    public class SOManager : ScriptableObject
    {
        public List<ResseteableSO> scriptableToReset;
        public List<InitiableSO> scriptableToInitiate;

        public void ResetScriptables()
        {
            foreach (var scriptable in scriptableToReset)
            {
                scriptable.ResetValues();
            }
        }

        public void InitScriptables()
        {
            foreach (var scriptable in scriptableToInitiate)
            {
                scriptable.InitScriptable();
            }
        }
    }
}