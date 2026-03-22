using UnityEngine;
using TMPro;
using Assets.Scripts.Gun;
using Assets.Scripts.Ammo;

namespace Assets.Scripts.Managers
{
    public class BulletUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _bulletNumberUI;
        [SerializeField] private Shoot _gun;
        [SerializeField] private GetAmmo _getAmmo;


        private void OnEnable()
        {
            _gun.OnBulletNumber += UpdateBulletUI;
            _getAmmo.OnRestartBulelts += UpdateBulletUI;
        }

        private void OnDisable()
        {
            _gun.OnBulletNumber -= UpdateBulletUI;
            _getAmmo.OnRestartBulelts -= UpdateBulletUI;
        }

        private void UpdateBulletUI(int value)
        {
            _bulletNumberUI.text = value.ToString();
        }
    }
}
