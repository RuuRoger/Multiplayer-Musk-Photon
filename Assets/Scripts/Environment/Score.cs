using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] private Material _blue;
        [SerializeField] private Material _orange;
        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "BulletBlue")
            {
                _renderer.material = _blue;
            }
        }
    }
}