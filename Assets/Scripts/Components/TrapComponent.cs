using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TrapComponent : MonoBehaviour
    {
        private enum SpikeState
        {
            Hidden,
            Rising,
            Active,
            Lowering
        }

        [Header("Timing")]
        [SerializeField] 
        private float _hiddenTime = 2f;
        [SerializeField] 
        private float _activeTime = 2f;
        [SerializeField] 
        private float _moveSpeed = 5f;

        [Header("Damage")]
        [SerializeField] 
        private LayerMask _playerLayer;
        [SerializeField] 
        private float _damageCooldown = 1f;
        [SerializeField]
        private Attack _attack;

        [Header("Positions")]
        [SerializeField] 
        private Transform _hiddenPoint;
        [SerializeField] 
        private Transform _activePoint;

        private SpikeState _currentState = SpikeState.Hidden;
        private float _timer;
        private float _damageTimer;

        private void Start()
        {
            transform.position = _hiddenPoint.position;
            _timer = _hiddenTime;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case SpikeState.Hidden:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0f)
                    {
                        _currentState = SpikeState.Rising;
                    }
                    break;

                case SpikeState.Rising:
                    MoveTowards(_activePoint.position);

                    if (Vector2.Distance(transform.position, _activePoint.position) < 0.05f)
                    {
                        _currentState = SpikeState.Active;
                        _timer = _activeTime;
                    }
                    break;

                case SpikeState.Active:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0f)
                    {
                        _currentState = SpikeState.Lowering;
                    }
                    break;

                case SpikeState.Lowering:
                    MoveTowards(_hiddenPoint.position);

                    if (Vector2.Distance(transform.position, _hiddenPoint.position) < 0.05f)
                    {
                        _currentState = SpikeState.Hidden;
                        _timer = _hiddenTime;
                    }
                    break;
            }
        }

        private void MoveTowards(Vector2 target)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                _moveSpeed * Time.deltaTime
            );
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (_currentState != SpikeState.Active) return;

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