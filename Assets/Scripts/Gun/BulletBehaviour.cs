using UnityEngine;

namespace Assets.Scripts.Gun
{
    public class BulletBehaviour : MonoBehaviour
    {
        private void Start()
        {
            Destroy(this.gameObject, 7f);
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(this.gameObject);
        }
    }
}