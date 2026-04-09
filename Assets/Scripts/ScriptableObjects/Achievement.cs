using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Achievement", menuName = "Game/Achievement")]
    public class Achievement : ScriptableObject
    {
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        public DateTime CompleteDate { get; set; }
    }
}