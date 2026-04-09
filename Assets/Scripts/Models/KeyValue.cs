using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public sealed class KeyValue<T, N>
    {
        public T Key;
        public N Value;

        public KeyValue(T key, N value)
        {
            Key = key;
            Value = value;
        }
    }
}