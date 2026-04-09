using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [System.Serializable]
    public class KeyInventoryData
    {
        public HashSet<string> Keys = new();
    }
}