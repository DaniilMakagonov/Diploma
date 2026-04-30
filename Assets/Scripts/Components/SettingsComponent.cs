using Assets.Scripts.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class SettingsComponent : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _music;
        [SerializeField]
        private GameObject _previousScreen;

        private void Start()
        {
            if (_music == null)
            {
                _music = FindFirstObjectByType<AudioSource>();
            }
        }

        public void SoundChange(float value)
        {
            _music.volume = value;
            Repository.SaveSettings(new() { MusicValue = value });
        }

        public void Back()
        {
            _previousScreen.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}