using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class PersistentComponent : MonoBehaviour
    {
        private void Awake()
        {
            if (GameObject.FindGameObjectsWithTag(gameObject.tag).Length > 1)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}