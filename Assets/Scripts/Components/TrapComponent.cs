using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TrapComponent : MonoBehaviour
    {
        [SerializeField]
        private Attack _attack;

        private enum SpikeState
        {
            Hidden,
            Rising,
            Active,
            Lowering
        }

        [Header("Timing")]
        [SerializeField] private float hiddenTime = 2f;
        [SerializeField] private float activeTime = 2f;
        [SerializeField] private float moveSpeed = 5f;

        [Header("Damage")]
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float damageCooldown = 1f;

        [Header("Positions")]
        [SerializeField] private Transform hiddenPoint;
        [SerializeField] private Transform activePoint;

        private SpikeState currentState = SpikeState.Hidden;
        private float timer;
        private float damageTimer;

        private void Start()
        {
            transform.position = hiddenPoint.position;
            timer = hiddenTime;
        }

        private void Update()
        {
            switch (currentState)
            {
                case SpikeState.Hidden:
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        currentState = SpikeState.Rising;
                    }
                    break;

                case SpikeState.Rising:
                    MoveTowards(activePoint.position);

                    if (Vector2.Distance(transform.position, activePoint.position) < 0.05f)
                    {
                        currentState = SpikeState.Active;
                        timer = activeTime;
                    }
                    break;

                case SpikeState.Active:
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        currentState = SpikeState.Lowering;
                    }
                    break;

                case SpikeState.Lowering:
                    MoveTowards(hiddenPoint.position);

                    if (Vector2.Distance(transform.position, hiddenPoint.position) < 0.05f)
                    {
                        currentState = SpikeState.Hidden;
                        timer = hiddenTime;
                    }
                    break;
            }
        }

        private void MoveTowards(Vector2 target)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (currentState != SpikeState.Active) return;

            if (((1 << other.gameObject.layer) & playerLayer) != 0)
            {
                damageTimer -= Time.deltaTime;

                if (damageTimer <= 0f)
                {
                    var health = other.GetComponent<DeffenseComponent>();

                    if (health != null)
                    {
                        health.GetDamage(_attack);
                        Debug.Log($"attacked from trap with damage {_attack.Damage}");
                    }
                    damageTimer = damageCooldown;
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