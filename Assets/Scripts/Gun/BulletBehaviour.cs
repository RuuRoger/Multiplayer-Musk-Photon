using UnityEngine;

namespace Assets.Scripts.Gun
{
    public class BulletBehaviour : MonoBehaviour
    {
        private void Start()
        {
            Destroy(this.gameObject, 4f);
        }

        private void OnTriggerEnter(Collider _)
        {
            Destroy(this.gameObject);
        }
    }
}