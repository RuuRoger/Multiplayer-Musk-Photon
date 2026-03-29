using UnityEngine;
using Assets.Scripts.Gun;
using System;

namespace Assets.Scripts.Ammo
{
    public class GetAmmo : MonoBehaviour
    {
        /* ================================================================================================================
        ---------------------------------------------------- MEMBERS -----------------------------------------------------
        ================================================================================================================= */
        [SerializeField] private Shoot _gun;
        // [SerializeField] private GameObject _ammo;
        private string playerTag;


        /* ================================================================================================================
        ---------------------------------------------------- EVENTS -----------------------------------------------------
        ================================================================================================================= */
        public event Action<string> OnDisableAmmo;
        public event Action<int> OnRestartBulelts;
        
        /* ================================================================================================================
        ---------------------------------------------------- UNITY LIVE CYCLE -----------------------------------------------------
        ================================================================================================================= */
        private void Start()
        {
            playerTag = gameObject.tag;
        }

        /* ================================================================================================================
        ---------------------------------------------------- COLLISION -----------------------------------------------------
        ================================================================================================================= */
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ammo1") && playerTag == "Player")
            {
                _gun.SetBulleetNumber();
                OnDisableAmmo?.Invoke(playerTag);
                OnRestartBulelts?.Invoke(6);
            }
            
            if (other.CompareTag("Ammo2") && playerTag == "Player2")
            {
                _gun.SetBulleetNumber();
                OnDisableAmmo?.Invoke(playerTag);
                OnRestartBulelts?.Invoke(6);
            }

            if (other.CompareTag("Ammo3") && playerTag == "Player3")
            {
                _gun.SetBulleetNumber();
                OnDisableAmmo?.Invoke(playerTag);
                OnRestartBulelts?.Invoke(6);
            }
        }
    }
}