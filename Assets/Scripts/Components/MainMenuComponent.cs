using Assets.Scripts.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class MainMenuComponent : MonoBehaviour
    {
        [SerializeField]
        private Button _continueButton;
        [SerializeField]
        private GameObject _settingsUI;
        [SerializeField]
        private GameObject _achievementUI;
        [SerializeField]
        private AudioSource _music;
        [SerializeField]
        private Slider _musicVolume;
        private bool _hasSavedGame;
        private string _continueSceneName;

        private void Start()
        {
            Repository.LoadState();
            AchievementsStorage.Load();
            _hasSavedGame = Repository.TryGetData<SaverData>(out var savedData);

            _continueSceneName = savedData?.SceneName ?? string.Empty;
            _continueButton.interactable = _hasSavedGame;

            if (Repository.TryGetSettings(out var settings))
            {
                _music.volume = settings.MusicValue;
                _musicVolume.value = settings.MusicValue;
            }
        }

        public void StartNewGame()
        {
            Repository.ClearData();
            SceneManager.LoadScene("Level 1");
        }

        public void ContinueGame()
        {
            SceneManager.LoadScene(_continueSceneName);
        }

        public void Settings()
        {
            _settingsUI.SetActive(true);
            gameObject.SetActive(false);
        }

        public void Achievments()
        {
            _achievementUI.SetActive(true);
            gameObject.SetActive(false);
        }

        public void ExitGame()
        {
            Debug.Log("exit");
            Application.Quit();
        }
        
    }
}