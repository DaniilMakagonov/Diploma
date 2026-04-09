using Assets.Scripts.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public static class Repository
    {
        private const string KEY = "Game State Info";

        private static Dictionary<string, string> _data = new();

        public static void SaveState()
        {
            PlayerPrefs.SetString(KEY, JsonUtility.ToJson(
                _data.Select(p => new KeyValue<string, string>(p.Key, p.Value)).ToList()));
        }

        public static void LoadState()
        {
            if (!PlayerPrefs.HasKey(KEY))
            {
                return;
            } 

            _data = new(JsonUtility.FromJson<List<KeyValue<string, string>>>(PlayerPrefs.GetString(KEY))
                .Select(p => new KeyValuePair<string, string>(p.Key, p.Value)));
        }

        public static void SetData<T>(T value)
        {
            _data[typeof(T).Name] = JsonUtility.ToJson(value);
        }

        public static bool TryGetData<T>(out T value)
        {
            if (_data.TryGetValue(typeof(T).Name, out var data))
            {
                value = JsonUtility.FromJson<T>(data);
                return true;
            }

            value = default;
            return false;
        }
    }
}