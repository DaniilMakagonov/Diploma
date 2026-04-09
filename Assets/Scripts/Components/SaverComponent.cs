using Assets.Scripts.Data;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SaverComponent : MonoBehaviour
    {
        [SerializeField] private int _id;
        private bool _isUsed;

        private void Start()
        {
            _isUsed = Repository.TryGetData<SaverData>(out var data) && data.Id <= _id;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<MainCharacterComponent>(out _) && !_isUsed)
            {
                _isUsed = true;
                Repository.SetData<SaverData>(new() { Id = _id });
                Repository.SaveState();
            }
        }


    }
}