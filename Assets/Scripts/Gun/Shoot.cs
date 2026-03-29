using UnityEngine;
using Assets.Scripts.Ammo;
using Assets.Scripts.Data.Gun;
using System;

namespace Assets.Scripts.Gun
{
    public class Shoot : MonoBehaviour
    {
        /* ================================================================================================================
        ---------------------------------------------------- MEMBERS -----------------------------------------------------
        ================================================================================================================= */
        [SerializeField] private GunSettings _settings; 
        [SerializeField] private Transform _firePoint;
        [SerializeField] private GameObject _bulletPrefab;
        private InputSystem_Actions _inputSystemActions;

        /* ================================================================================================================
        ---------------------------------------------------- PROPERTIES -----------------------------------------------------
        ================================================================================================================= */
        public int BulletNumber {get; private set; } = 6;

        /* ================================================================================================================
        ---------------------------------------------------- EVENTS -----------------------------------------------------
        ================================================================================================================= */
        public event Action OnShowAmmo;
        public event Action<int> OnBulletNumber;

        /* ================================================================================================================
        ---------------------------------------------------- UNITY LIFE CYCLE METHODS -----------------------------------------------------
        ================================================================================================================= */
        private void Awake()
        {
            _inputSystemActions = new InputSystem_Actions();
        }

        private void OnEnable()
        {
            _inputSystemActions.Player.Shoot.Enable();
        }

        private void OnDisable()
        {
            _inputSystemActions.Player.Shoot.Disable(); 
        }

        private void Update()
        {
            ToShoot();
        }

        /* ================================================================================================================
        ---------------------------------------------------- SHOOT HANDLE -----------------------------------------------------
        ================================================================================================================= */
        private void ToShoot()
        {
            if (_inputSystemActions.Player.Shoot.WasPressedThisFrame())
            {
                if (BulletNumber <= 0)
                {
                    return;
                }

                Vector3 shootDirection = GetShootDirection();
                Quaternion shootRotation = Quaternion.LookRotation(shootDirection);
                GameObject bulletInstance = Instantiate(_bulletPrefab, _firePoint.position, shootRotation);
                var rigidbodyBullet = bulletInstance.GetComponent<Rigidbody>();

                if (rigidbodyBullet != null)
                {
                    rigidbodyBullet.linearVelocity = shootDirection * _settings.ShootForce;
                    BulletNumber --;
                    OnBulletNumber?.Invoke(BulletNumber);

                    if (BulletNumber <= 0)
                    {
                        OnShowAmmo?.Invoke();
                    }
                }

                Debug.Log($"Balas: {BulletNumber}");
            }   
        }

        private Vector3 GetShootDirection()
        {
            if (UnityEngine.Camera.main != null)
            {
                return UnityEngine.Camera.main.transform.forward.normalized;
            }

            return _firePoint.forward.normalized;
        }

        public void SetBulleetNumber()
        {
            BulletNumber = 6;
        }
    }
}