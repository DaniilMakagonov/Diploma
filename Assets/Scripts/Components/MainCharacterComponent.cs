using Assets.Scripts.Data;
using Assets.Scripts.Models;
using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(DeffenseComponent))]
    public class MainCharacterComponent : MonoBehaviour
    {
        private DeffenseComponent _deffenseComponent;
        private Rigidbody2D _rigidBody;
        private SpriteRenderer _spriteRenderer;
        private Transform _transform;
        private float _moveInput;
        private bool _isGrounded;
        private float _jumpBufferCounter;
        private bool _facingRight = true;
        private Dictionary<Elements, Sprite> _sprites;

        [Header("Level Settings")]
        [SerializeField]
        private Vector3 _spawn;

        [Header("Movement")]
        [SerializeField]
        private float _moveSpeed = 6f;
        [SerializeField]
        private float _jumpForce = 12f;

        [Header("Ground Check")]
        [SerializeField]
        private Transform _groundCheck;
        [SerializeField]
        private float _groundCheckRadius = 0.2f;
        [SerializeField]
        private LayerMask _groundLayer;

        [Header("Jump Tuning")]
        [SerializeField]
        private float _jumpBufferTime = 0.15f;

        [Header("Attack")]
        [SerializeField]
        private GameObject _bullet;
        [SerializeField]
        private Transform _firePoint;
        [SerializeField]
        private float _bulletSpeed = 10f;
        [SerializeField]
        private List<KeyValue<Elements, Sprite>> _spritesSource;
        [SerializeField]
        private List<Element> _elements;

        [Header("Achievements")]
        [SerializeField]
        private Achievement _firstDeathAchievement;

        public void UpdateSpawn(Vector3 spawn)
        {
            Repository.SetData<PlayerData>(new() { Spawn = spawn });
        }

        private void Start()
        {
            _deffenseComponent = GetComponent<DeffenseComponent>();
            _deffenseComponent.SubscribeOnDeath(CharacterDeath);
            _rigidBody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _sprites = new(_spritesSource.Select(p => new KeyValuePair<Elements, Sprite>(p.Key, p.Value)));

            if (Repository.TryGetData<PlayerData>(out var data))
            {
                Debug.Log(data.Spawn.ToString());
                transform.position = data.Spawn;
            }
        }
        private void Update()
        {
            if (Time.timeScale < 1) return;

            if (transform.position.y < -10) CharacterDeath();

            _moveInput = Input.GetAxisRaw("Horizontal");

            _isGrounded = Physics2D.OverlapCircle(
                _groundCheck.position,
                _groundCheckRadius,
                _groundLayer);

            if (Input.GetButtonDown("Jump"))
            {
                _jumpBufferCounter = _jumpBufferTime;
            }
            else
            {
                _jumpBufferCounter = Mathf.Max(_jumpBufferCounter - Time.deltaTime, 0);
            }

            if (_moveInput > 0 && !_facingRight || _moveInput < 0 && _facingRight)
            {
                Flip();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Shoot();
            }

            CheckElementChange();
        }

        private void FixedUpdate()
        {
            if (Time.timeScale < 1) return;

            _rigidBody.linearVelocityX = _moveInput * _moveSpeed;

            if (_jumpBufferCounter > 0 && _isGrounded)
            {
                _rigidBody.linearVelocityY = _jumpForce;
                _jumpBufferCounter = 0;
            }
        }

        private void Flip()
        {
            _facingRight = !_facingRight;

            _spriteRenderer.flipX = !_facingRight;

            var firePoint = _firePoint.transform.localPosition;
            firePoint.x *= -1;
            _firePoint.transform.localPosition = firePoint;
        }

        private void Shoot()
        {
            var bullet = Instantiate(_bullet, _firePoint.position, Quaternion.identity);

            var bulletComponent = bullet.GetComponent<BulletComponent>();
            bulletComponent.Attack.Element = _deffenseComponent.DeffenseElement;

            var spriteRenderer = bullet.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = _sprites[_deffenseComponent.DeffenseElement.Type];

            var bulletTransform = bullet.GetComponent<Transform>();
            var scale = bulletTransform.localScale;
            scale *= 0.2f;
            bulletTransform.localScale = scale;

            var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            var direction = _facingRight ? 1f : -1f;
            bulletRigidbody.linearVelocityX = direction * _bulletSpeed;
        }

        private void CheckElementChange()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _deffenseComponent.DeffenseElement = _elements[0];
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _deffenseComponent.DeffenseElement = _elements[1];
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _deffenseComponent.DeffenseElement = _elements[2];
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _deffenseComponent.DeffenseElement = _elements[3];
            }
        }

        private void CharacterDeath()
        {
            if (!AchievementsStorage.Check(_firstDeathAchievement))
            {
                AchievementsStorage.Add(_firstDeathAchievement);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}