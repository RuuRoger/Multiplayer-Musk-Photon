using UnityEngine;
using Assets.Scripts.Gun;
using Assets.Scripts.Ammo;

namespace Assets.Scripts.Managers
{
    public class AmmoHandler : MonoBehaviour
    {
        /* ================================================================================================================
        ---------------------------------------------------- MEMBERS -----------------------------------------------------
        ================================================================================================================= */
        [SerializeField] private GameObject _ammo;
        [SerializeField] private Shoot _shootScript;
        [SerializeField] private GetAmmo _getAmmoScript;
        private float _randomX;
        private float _randomZ;

        /* ================================================================================================================
        ---------------------------------------------------- UNITY LIFE CYCLE METHODS -----------------------------------------------------
        ================================================================================================================= */

        private void Start()
        {
            _shootScript.OnShowAmmo += ActiveAmmo;
            _getAmmoScript.OnDisableAmmo += DisableAmmo;
        }

        private void OnDisable()
        {
            _shootScript.OnShowAmmo -= ActiveAmmo;
            _getAmmoScript.OnDisableAmmo -= DisableAmmo;
        }

        /* ================================================================================================================
        ---------------------------------------------------- ENABLE/DISABLE AMMO -----------------------------------------------------
        ================================================================================================================= */

        private void ActiveAmmo()
        {
            _randomX = Random.Range(-24f, 24f);
            _randomZ = Random.Range(-24f, 24f);
            _ammo.transform.position = new Vector3(_randomX, 1f, _randomZ);

            _ammo.gameObject.SetActive(true);
        }

        private void DisableAmmo()
        {
            _ammo.gameObject.SetActive(false);
        }
    }
}