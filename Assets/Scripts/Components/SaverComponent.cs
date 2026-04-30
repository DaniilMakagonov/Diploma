using Assets.Scripts.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SaverComponent : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField]
        private SpriteRenderer _sprite;
        private bool _isUsed = false;

        private void Start()
        {
            _isUsed = Repository.TryGetData<SaverData>(out var data) && data.Id <= _id;
            if (_isUsed)
            {
                _sprite.color = Color.green;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<MainCharacterComponent>(out var character) && !_isUsed)
            {
                _isUsed = true;
                Debug.Log(transform.position.ToString());
                character.UpdateSpawn(transform.position);

                Repository.SetData<SaverData>(new()
                {
                    Id = _id,
                    SceneName = SceneManager.GetActiveScene().name,
                });
                Repository.SaveState();

                _sprite.color = Color.green;
            }
        }
    }
}