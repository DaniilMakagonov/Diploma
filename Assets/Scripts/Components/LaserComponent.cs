using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LaserComponent : MonoBehaviour
    {
        [Header("Damage")]
        [SerializeField] 
        private LayerMask _playerLayer;
        [SerializeField]
        private float _damageCooldown = 1f;
        [SerializeField]
        private Attack _attack;

        private float _damageTimer;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
            {
                _damageTimer -= Time.deltaTime;

                if (_damageTimer <= 0f)
                {
                    var health = other.GetComponent<DeffenseComponent>();

                    if (health != null)
                    {
                        health.GetDamage(_attack);
                        Debug.Log($"attacked from trap with damage {_attack.Damage}");
                    }
                    _damageTimer = _damageCooldown;
                }
            }
        }
    }
}