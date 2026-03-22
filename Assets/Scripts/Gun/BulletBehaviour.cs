using UnityEngine;

namespace Assets.Scripts.Gun
{
    public class BulletBehaviour : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, 4f);
        }

        private void OnTriggerEnter(Collider _)
        {
            Destroy(gameObject, 0.5f);
        }
    }
}