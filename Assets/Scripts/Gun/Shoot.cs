using UnityEngine;
using Assets.Scripts.Ammo;

namespace Assets.Scripts.Gun
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _shootForce = 5f;
        [SerializeField] GetAmmo _getAmoObject;
        private InputSystem_Actions _inputSystemActions;

        // Properties
        public int BulletNumber {get; private set; } = 6;

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

        private void ToShoot()
        {
            if (_inputSystemActions.Player.Shoot.WasPressedThisFrame())
            {
                if (_bulletNumber <= 0)
                {
                    return;
                }

                Vector3 shootDirection = GetShootDirection();
                Quaternion shootRotation = Quaternion.LookRotation(shootDirection);
                GameObject bulletInstance = Instantiate(_bullet, _firePoint.position, shootRotation);
                var rigidbodyBullet = bulletInstance.GetComponent<Rigidbody>();

                if (rigidbodyBullet != null)
                {
                    rigidbodyBullet.linearVelocity = shootDirection * _shootForce;
                    _bulletNumber --;
                }
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

    }
}