using UnityEngine;
using Assets.Scripts.Gun;

namespace Assets.Scripts.Ammo
{
    public class GetAmmo : MonoBehaviour
    {
        [SerializeField] private Shoot _gun;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _gun.SetBulleetNumber();
                this.gameObject.SetActive(false);
            }
        }
    }
}