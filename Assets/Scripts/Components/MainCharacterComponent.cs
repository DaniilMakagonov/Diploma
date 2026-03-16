using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(DeffenseComponent))]
    public class MainCharacterComponent : MonoBehaviour
    {
        private DeffenseComponent _deffenseComponent;

        private void Start()
        {
            _deffenseComponent = GetComponent<DeffenseComponent>();
        }

    }
}