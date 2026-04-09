using Assets.Scripts.Models;
using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class BulletComponent : MonoBehaviour
    {
        [SerializeField]
        private Attack _attack;
        [SerializeField]
        private float _timeToLive = 3f;

        private SpriteRenderer _spriteRenderer;

        public Attack Attack
        {
            get => _attack;
            private set => _attack = value;
        }

        private void Start()
        {
            Destroy(gameObject, _timeToLive);

            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var health = collision.gameObject.GetComponent<DeffenseComponent>();

            if (health != null)
            {
                health.GetDamage(_attack);
                Debug.Log($"attacked from bullet with damage {_attack.Damage}");
            }

            Destroy(gameObject);
        }

    }
}