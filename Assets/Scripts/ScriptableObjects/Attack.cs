using System;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Game/Attack")]
    public sealed class Attack : ScriptableObject
    {
        public event Action<Elements> OnElementChanged;
        [SerializeField]
        private Element _element;
        [SerializeField]
        private int _damage;
        public Element Element { 
            get => _element;
            set 
            {
                _element = value;
                OnElementChanged?.Invoke(_element.Type);
            }
        }
        public int Damage { 
            get => _damage; 
            set => _damage = value;
        }
    }
}