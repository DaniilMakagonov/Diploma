using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(DeffenseComponent))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyComponent : MonoBehaviour
    {
        private DeffenseComponent _deffenceComponent;
        [Header("Patrol")]
        [SerializeField] private Transform leftPoint;
        [SerializeField] private Transform rightPoint;
        [SerializeField] private float moveSpeed = 2f;

        [Header("Vision")]
        [SerializeField] private Transform visionPoint;
        [SerializeField] private float visionDistance = 8f;
        [SerializeField] private LayerMask playerLayer;

        [Header("Shooting")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float projectileSpeed = 7f;
        [SerializeField] private float shootCooldown = 1f;

        private Rigidbody2D rb;
        private Transform player;
        private bool isPlayerVisible;
        private bool movingRight = true;
        private float shootTimer;
        private void Start()
        {
            _deffenceComponent = GetComponent<DeffenseComponent>();
            _deffenceComponent.SubscribeOnDeath(() => Destroy(gameObject));

            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            CheckPlayerVisibility();

            if (isPlayerVisible)
            {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

                LookAtPlayer();

                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0f)
                {
                    Shoot();
                    shootTimer = shootCooldown;
                }
            }
            else
            {
                Patrol();
            }
        }

        private void CheckPlayerVisibility()
        {
            isPlayerVisible = false;
            player = null;

            Vector2 direction = movingRight ? Vector2.right : Vector2.left;

            RaycastHit2D hit = Physics2D.Raycast(
                visionPoint.position,
                direction,
                visionDistance,
                playerLayer
            );

            if (hit.collider != null)
            {
                isPlayerVisible = true;
                player = hit.transform;
            }
        }

        private void Patrol()
        {
            if (movingRight)
            {
                rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

                if (transform.position.x >= rightPoint.position.x)
                {
                    movingRight = false;
                    Flip(false);
                }
            }
            else
            {
                rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);

                if (transform.position.x <= leftPoint.position.x)
                {
                    movingRight = true;
                    Flip(true);
                }
            }
        }

        private void LookAtPlayer()
        {
            if (player == null) return;

            if (player.position.x > transform.position.x)
            {
                movingRight = true;
                Flip(true);
            }
            else
            {
                movingRight = false;
                Flip(false);
            }
        }

        private void Shoot()
        {
            if (projectilePrefab == null || firePoint == null) return;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                float direction = movingRight ? 1f : -1f;
                projectileRb.linearVelocity = new Vector2(direction * projectileSpeed, 0f);
            }
        }

        private void Flip(bool faceRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (faceRight ? 1f : -1f);
            transform.localScale = scale;
        }
    }
}