using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TrapComponent : MonoBehaviour
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