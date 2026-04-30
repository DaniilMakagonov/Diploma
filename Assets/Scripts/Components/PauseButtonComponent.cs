using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class PauseButtonComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pauseUI;

        public void Pause()
        {
            _pauseUI.SetActive(true);
            Time.timeScale = 0f;
            gameObject.SetActive(false);
        }
    }
}