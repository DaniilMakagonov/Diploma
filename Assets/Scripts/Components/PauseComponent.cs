using Assets.Scripts.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class PauseComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject _settingsUI;
        [SerializeField]
        private GameObject _pauseButton;
        [SerializeField]
        private Slider _musicVolume;

        private void Start()
        {
            if (Repository.TryGetSettings(out var settings))
            {
                _musicVolume.value = settings.MusicValue;
            }
        }

        public void Continue()
        {
            _pauseButton.SetActive(true);
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
        }

        public void Exit()
        {
            SceneManager.LoadScene("Main Menu");
            Time.timeScale = 1.0f;
        }

        public void Settings()
        {
            _settingsUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}