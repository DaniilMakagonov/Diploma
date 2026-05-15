using Assets.Scripts.Data;
using Assets.Scripts.Models;
using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(DeffenseComponent))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class BossComponent : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField]
        private RectTransform _ui;
        [SerializeField]
        private float _abilityCoolDown = 3f;
        [SerializeField]
        private List<GameObject> _blockDoors;

        [Header("Patrol")]
        [SerializeField] 
        private Transform _leftPoint;
        [SerializeField] 
        private Transform _rightPoint;
        [SerializeField] 
        private float _moveSpeed = 2f;

        [Header("Vision")]
        [SerializeField] 
        private Transform _visionPoint;
        [SerializeField] 
        private float _visionDistance = 8f;
        [SerializeField] 
        private LayerMask _playerLayer;

        [Header("Shooting")]
        [SerializeField] 
        private GameObject _projectilePrefab;
        [SerializeField] 
        private Transform _firePoint;
        [SerializeField] 
        private float _projectileSpeed = 7f;
        [SerializeField] 
        private float _shootCooldown = 1f;
        [SerializeField]
        private float _shootingCoolDown = 7f;
        [SerializeField] 
        private float _firePointRadious = 2f;

        [Header("Laser")]
        [SerializeField]
        private GameObject _lazer;
        [SerializeField]
        private float _laserCooldown = 5f;

        [Header("Shield")]
        [SerializeField]
        private GameObject _shield;
        [SerializeField]
        private float _shieldCooldown = 7f;

        [Header("Achievements")]
        [SerializeField]
        private Achievement _bossDefeatAchievement;


        private DeffenseComponent _deffenceComponent;
        private Rigidbody2D _rb;
        private Transform _player;
        private bool _isPlayerVisible;
        private bool _movingRight = true;
        private float _shootTimer;
        private float _abilityTimer;
        private BossAbility _currentAbility = BossAbility.None;
        private void Start()
        {
            _deffenceComponent = GetComponent<DeffenseComponent>();
            _deffenceComponent.SubscribeOnDeath(OnDeath);

            _rb = GetComponent<Rigidbody2D>();

            UnityEngine.Random.InitState(179);
        }

        private void Update()
        {
            if (Time.timeScale < 1) return;

            if (!_isPlayerVisible) CheckPlayerVisibility();

            Patrol();

            if (!_isPlayerVisible) return;
            
            LookAtPlayer();

            UseAbility();
        }

        private void UseAbility()
        {
            _abilityTimer -= Time.deltaTime;

            if (_currentAbility == BossAbility.Shooting)
            {
                _shootTimer -= Time.deltaTime;

                if (_shootTimer <= 0f)
                {
                    Shoot();
                    Debug.Log("shoot");
                    _shootTimer = _shootCooldown;
                }

                if (_abilityTimer <= 0f)
                {
                    _currentAbility = BossAbility.None;
                    _abilityTimer = _abilityCoolDown;
                }
            }

            if (_currentAbility == BossAbility.Laser)
            {
                _lazer.SetActive(true);
                Debug.Log(_lazer.activeSelf);
                if (_abilityTimer <= 0f)
                {
                    _currentAbility = BossAbility.None;
                    _abilityTimer = _abilityCoolDown;
                    _lazer.SetActive(false);
                }
            }

            if (_currentAbility == BossAbility.Shield)
            {
                _shield.SetActive(true);
                Debug.Log(_shield.activeSelf);
                if (_abilityTimer <= 0f)
                {
                    _currentAbility = BossAbility.None;
                    _abilityTimer = _abilityCoolDown;
                    _shield.SetActive(false);
                }
            }

            if (_currentAbility == BossAbility.None && _abilityTimer <= 0f)
            {
                _currentAbility = (BossAbility)UnityEngine.Random.Range(1, 4);
                _abilityTimer = _currentAbility switch
                {
                    BossAbility.Shooting => _shootingCoolDown,
                    BossAbility.Laser => _laserCooldown,
                    BossAbility.Shield => _shieldCooldown,
                    _ => throw new InvalidEnumArgumentException(nameof(BossAbility)),
                };
                Debug.Log(_currentAbility);
            }
        }

        private void CheckPlayerVisibility()
        {
            var hit = Physics2D.OverlapCircle(
                _visionPoint.position,
                _visionDistance,
                _playerLayer
            );

            if (hit != null)
            {
                _isPlayerVisible = true;
                _player = hit.transform;

                foreach (var block in _blockDoors)
                {
                    block.SetActive(true);
                }
            }
        }

        private void Patrol()
        {
            if (_movingRight)
            {
                _rb.linearVelocity = new Vector2(_moveSpeed, _rb.linearVelocity.y);

                if (transform.position.x >= _rightPoint.position.x)
                {
                    _movingRight = false;
                    Flip(false);
                }
            }
            else
            {
                _rb.linearVelocity = new Vector2(-_moveSpeed, _rb.linearVelocity.y);

                if (transform.position.x <= _leftPoint.position.x)
                {
                    _movingRight = true;
                    Flip(true);
                }
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

        private void LookAtPlayer()
        {
            if (_player == null) return;

            if (_player.position.x > transform.position.x)
            {
                _movingRight = true;
                Flip(true);
            }
            else
            {
                _movingRight = false;
                Flip(false);
            }
        }

        private void Shoot()
        {
            if (_projectilePrefab == null || _firePoint == null) return;

            var direction = (_player.position - _firePoint.position).normalized;

            GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position + direction * _firePointRadious, Quaternion.identity);

            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                projectileRb.linearVelocity = new Vector2(direction.x, direction.y).normalized * _projectileSpeed;
            }
        }

        private void OnDeath()
        {
            if (!AchievementsStorage.Check(_bossDefeatAchievement))
            {
                AchievementsStorage.Add(_bossDefeatAchievement);
            }

            foreach (var block in _blockDoors)
            {
                block.SetActive(false);
            }

            Repository.SaveState();

            Destroy(gameObject);
        }
    }
}