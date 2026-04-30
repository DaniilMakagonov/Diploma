using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class AudioPlayerComponent : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _music;
        void Start()
        {
            _music.Play();
        }
    }
}