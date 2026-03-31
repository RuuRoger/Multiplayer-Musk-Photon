using System;
using UnityEngine;
using Assets.Scripts.Gun;
using Photon.Pun;

namespace Assets.Scripts.Ammo
{
    public class GetAmmo : MonoBehaviour
    {
        /* ================================================================================================================
        ---------------------------------------------------- MEMBERS -----------------------------------------------------
        ================================================================================================================= */
        [SerializeField] private Shoot _gun;
        private int playerActorNumber = -1;

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
            var photonViewComponent = GetComponentInParent<PhotonView>();
            if (photonViewComponent != null)
            {
                if (photonViewComponent.InstantiationData != null && photonViewComponent.InstantiationData.Length > 0)
                {
                    try
                    { 
                        playerActorNumber = (int)photonViewComponent.InstantiationData[0]; 
                    } 
                    catch 
                    { 
                        playerActorNumber = photonViewComponent.Owner != null ? photonViewComponent.Owner.ActorNumber : -1;
                    }
                }
                else
                {
                    playerActorNumber = photonViewComponent.Owner != null ? photonViewComponent.Owner.ActorNumber : -1;
                }
            }
        }

        /* ================================================================================================================
        ---------------------------------------------------- COLLISION -----------------------------------------------------
        ================================================================================================================= */
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ammo1") && playerActorNumber == 1)
            {
                _gun.SetBulleetNumber();
                OnDisableAmmo?.Invoke("Player");
                OnRestartBulelts?.Invoke(6);
            }
            
            if (other.CompareTag("Ammo2") && playerActorNumber == 2)
            {
                _gun.SetBulleetNumber();
                OnDisableAmmo?.Invoke("Player2");
                OnRestartBulelts?.Invoke(6);
            }

            if (other.CompareTag("Ammo3") && playerActorNumber == 3)
            {
                _gun.SetBulleetNumber();
                OnDisableAmmo?.Invoke("Player3");
                OnRestartBulelts?.Invoke(6);
            }
        }
    }
}