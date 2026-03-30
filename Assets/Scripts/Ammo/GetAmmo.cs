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
            // Determine this player's actor number from the nearest PhotonView (if any)
            var pv = GetComponentInParent<Photon.Pun.PhotonView>();
            if (pv != null)
            {
                if (pv.InstantiationData != null && pv.InstantiationData.Length > 0)
                {
                    try { playerActorNumber = (int)pv.InstantiationData[0]; } catch { playerActorNumber = pv.Owner != null ? pv.Owner.ActorNumber : -1; }
                }
                else
                {
                    playerActorNumber = pv.Owner != null ? pv.Owner.ActorNumber : -1;
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