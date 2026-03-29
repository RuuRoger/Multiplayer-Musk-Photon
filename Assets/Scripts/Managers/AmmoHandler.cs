using UnityEngine;
using Assets.Scripts.Gun;
using Assets.Scripts.Ammo;
using Assets.Scripts.Player;

namespace Assets.Scripts.Managers
{
    public class AmmoHandler : MonoBehaviour
    {
        /* ================================================================================================================
        ---------------------------------------------------- MEMBERS -----------------------------------------------------
        ================================================================================================================= */
        private Shoot _shootScript;
        private GetAmmo _getAmmoScript;
        private GameObject _ammoPlayer1;
        private GameObject _ammoPlayer2;
        private GameObject _ammoPlayer3;
        private float _randomX;
        private float _randomZ;

        /* ================================================================================================================
        ---------------------------------------------------- UNITY LIFE CYCLE METHODS -----------------------------------------------------
        ================================================================================================================= */
        private void Awake()
        {
            _ammoPlayer1 = FindInactiveObjectWithTag("Ammo1");
            _ammoPlayer2 = FindInactiveObjectWithTag("Ammo2");
            _ammoPlayer3 = FindInactiveObjectWithTag("Ammo3");
        }
        private void Start()
        {
            _ammoPlayer1.SetActive(false);
            _ammoPlayer2.SetActive(false);
            _ammoPlayer3.SetActive(false);

            _shootScript = transform.root.GetComponentInChildren<Shoot>();
            _getAmmoScript = transform.root.GetComponent<GetAmmo>();
            // Invoke(nameof(SuscribeEvents), 5f);

            _shootScript.OnShowAmmo += ActiveAmmo;
            _getAmmoScript.OnDisableAmmo += DisableAmmo;
        }

        private void OnDisable()
        {
            _shootScript.OnShowAmmo -= ActiveAmmo;
            _getAmmoScript.OnDisableAmmo -= DisableAmmo;
        }

        // private void SuscribeEvents()
        // {
        //     _shootScript.OnShowAmmo += ActiveAmmo;
        //     _getAmmoScript.OnDisableAmmo += DisableAmmo;
        // }

        /* ================================================================================================================
        ---------------------------------------------------- ASSING AMMO -----------------------------------------------------
        ================================================================================================================= */
        
        private GameObject FindInactiveObjectWithTag(string tag)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.CompareTag(tag) && obj.hideFlags == HideFlags.None)
                {
                    return obj;
                }
            }
            return null;
        }

        /* ================================================================================================================
        ---------------------------------------------------- ENABLE/DISABLE AMMO -----------------------------------------------------
        ================================================================================================================= */

        private void ActiveAmmo(string playerTag)
        {
            if (playerTag == "Player")
            {
                _randomX = Random.Range(-24f, 24f);
                _randomZ = Random.Range(-24f, 24f);
                _ammoPlayer1.transform.position = new Vector3(_randomX, 1f, _randomZ);
                _ammoPlayer1.gameObject.SetActive(true);
            }
            if (playerTag == "Player2")
            {
                _randomX = Random.Range(-24f, 24f);
                _randomZ = Random.Range(-24f, 24f);
                _ammoPlayer2.transform.position = new Vector3(_randomX, 1f, _randomZ);
                _ammoPlayer2.gameObject.SetActive(true);
            }
            if (playerTag == "Player3")
            {
                _randomX = Random.Range(-24f, 24f);
                _randomZ = Random.Range(-24f, 24f);
                _ammoPlayer3.transform.position = new Vector3(_randomX, 1f, _randomZ);
                _ammoPlayer3.gameObject.SetActive(true);
            }
        }

        private void DisableAmmo(string playerTag)
        {
            if (playerTag == "Player")
                _ammoPlayer1.gameObject.SetActive(false);

            if (playerTag == "Player2")
                _ammoPlayer2.gameObject.SetActive(false);

            if (playerTag == "Player3")
                _ammoPlayer3.gameObject.SetActive(false);
        }

    }
}