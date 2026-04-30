using Assets.Scripts.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public static class Repository
    {
        private const string STATE_KEY = "Game State Info";
        private const string SETTINGS_KEY = "Game Settings Info";

        private static Dictionary<string, string> _data = new();

        public static void SaveSettings(GameSettingsData data)
        {
            PlayerPrefs.SetString(SETTINGS_KEY, JsonUtility.ToJson(data));
        }

        public static bool TryGetSettings(out GameSettingsData data)
        {
            if (PlayerPrefs.HasKey(SETTINGS_KEY))
            {
                data = JsonUtility.FromJson<GameSettingsData>(PlayerPrefs.GetString(SETTINGS_KEY));
                return true;
            }

            data = default;
            return false;
        }

        public static void ClearData()
        {
            _data.Clear();
            SaveState();
        }

        public static void SaveState()
        {
            Debug.Log("Save state" +_data.Count.ToString() + JsonUtility.ToJson(
                _data.Select(p => new KeyValue<string, string>(p.Key, p.Value)).ToList()));
            PlayerPrefs.SetString(STATE_KEY, JsonUtility.ToJson(new RepositoryData()
            {
                Data = _data.Select(p => new KeyValue<string, string>(p.Key, p.Value)).ToList()
            }));
        }

        public static void LoadState()
        {
            if (!PlayerPrefs.HasKey(STATE_KEY))
            {
                return;
            }
            Debug.Log("Load state" + PlayerPrefs.GetString(STATE_KEY));
            _data = new(JsonUtility.FromJson<RepositoryData>(PlayerPrefs.GetString(STATE_KEY)).Data
                .Select(p => new KeyValuePair<string, string>(p.Key, p.Value)));
        }

        public static void SetData<T>(T value)
        {
            _data[typeof(T).Name] = JsonUtility.ToJson(value);
            Debug.Log("set data" + _data[typeof(T).Name]);
        }

        public static bool TryGetData<T>(out T value)
        {
            Debug.Log("try get data" + typeof(T).Name);
            if (_data.TryGetValue(typeof(T).Name, out var data))
            {
                value = JsonUtility.FromJson<T>(data);
                Debug.Log("get data" + data);
                return true;
            }

            value = default;
            return false;
        }
    }
}