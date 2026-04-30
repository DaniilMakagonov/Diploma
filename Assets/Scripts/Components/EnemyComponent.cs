using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(DeffenseComponent))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyComponent : MonoBehaviour
    {
        private DeffenseComponent _deffenceComponent;

        [SerializeField]
        private RectTransform _ui;

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
        [SerializeField] private float firePointRadious = 2f;

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
            if (Time.timeScale < 1) return; 

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
            var hit = Physics2D.OverlapCircle(
                visionPoint.position,
                visionDistance,
                playerLayer
            );

            if (hit != null)
            {
                isPlayerVisible = true;
                player = hit.transform;
            }
            else
            {
                isPlayerVisible = false;
                player = null;
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

            var direction = (player.position - firePoint.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position + direction * firePointRadious, Quaternion.identity);

            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                projectileRb.linearVelocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed;
            }
        }

        private void Flip(bool faceRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (faceRight ? 1f : -1f);
            transform.localScale = scale;

            scale = _ui.localScale;
            scale.x = Mathf.Abs(scale.x) * (faceRight ? 1f : -1f);
            _ui.localScale = scale;
        }
    }
}