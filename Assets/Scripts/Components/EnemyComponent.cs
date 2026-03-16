using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(DeffenseComponent))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyComponent : MonoBehaviour
    {
        private DeffenseComponent _deffenceComponent;
        private void Start()
        {
            _deffenceComponent = GetComponent<DeffenseComponent>();
            _deffenceComponent.SubscribeOnDeath(() => Destroy(gameObject));
        }
    }
}