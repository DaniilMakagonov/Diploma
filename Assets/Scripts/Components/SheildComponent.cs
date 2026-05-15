using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(DeffenseComponent))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class SheildComponent : MonoBehaviour
    {
        private DeffenseComponent _deffence;

        private void Start()
        {
            _deffence = GetComponent<DeffenseComponent>();
            _deffence.SubscribeOnDeath(() => _deffence.GetHealth(_deffence.MaxHealth));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<BulletComponent>()  != null)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}