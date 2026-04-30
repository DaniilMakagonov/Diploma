using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Data;

namespace Assets.Scripts.Components
{
    public class PlayerKeyInventoryComponent : MonoBehaviour
    {
        private HashSet<string> _collectedKeys;

        private void Awake()
        {
            _collectedKeys = Repository.TryGetData(out KeyInventoryData data)
                ? data.Keys
                : new();
        }
        public void AddKey(string keyName)
        {
            _collectedKeys.Add(keyName);
            Repository.SetData<KeyInventoryData>(new() { Keys = _collectedKeys });
        }

        public bool HasKey(string keyName)
        {
            return _collectedKeys.Contains(keyName);
        }
    }
}