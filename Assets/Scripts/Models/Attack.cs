using Assets.Scripts.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public sealed class Attack
    {
        public Element Element { get; set; }
        public int Damage { get; set; }
    }
}