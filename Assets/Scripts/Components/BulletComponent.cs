using Assets.Scripts.Models;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class BulletComponent : MonoBehaviour
    {
        [SerializeField]
        private Attack _attack;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var health = collision.gameObject.GetComponent<DeffenseComponent>();

            if (health != null)
            {
                health.GetDamage(_attack);
            }

            Destroy(gameObject);
        }
    }
}