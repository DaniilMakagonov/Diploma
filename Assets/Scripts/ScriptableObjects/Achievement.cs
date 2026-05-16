using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Achievement", menuName = "Game/Achievement")]
    public class Achievement : ScriptableObject
    {
        [SerializeField] public string Description;
        [SerializeField] public Sprite Sprite;
        [SerializeField] public string CompleteDate;
    }
}