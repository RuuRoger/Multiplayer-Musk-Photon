using UnityEngine;
using Photon.Pun;

namespace Assets.Scripts.Gun
{
    public class BulletBehaviour : MonoBehaviourPun
    {
        [SerializeField] private Material _blue;
        [SerializeField] private Material _red;
        [SerializeField] private Material _white;

        private void Start()
        {
            // Asignar material según actorNumber recibido
            int actorNumber = 1;
            if (photonView.InstantiationData != null && photonView.InstantiationData.Length > 0)
                actorNumber = (int)photonView.InstantiationData[0];

            var meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
            switch (actorNumber)
            {
                case 1:
                    foreach (var r in meshRenderers) if (r != null) r.material = _blue;
                    gameObject.tag = "BulletBlue";
                    break;
                case 2:
                    foreach (var r in meshRenderers) if (r != null) r.material = _red;
                    gameObject.tag = "BulletRed";
                    break;
                case 3:
                    foreach (var r in meshRenderers) if (r != null) r.material = _white;
                    gameObject.tag = "BulletWhite";
                    break;
            }

            Destroy(gameObject, 4f);
        }

        // Utility: load default bullet materials from the Bullet prefab in Resources
        public static Material[] LoadDefaultMaterials()
        {
            var prefab = Resources.Load<GameObject>("Bullet");
            if (prefab == null) return new Material[] { null, null, null };

            var bb = prefab.GetComponent<BulletBehaviour>();
            if (bb == null) return new Material[] { null, null, null };

            return new Material[] { bb._blue, bb._red, bb._white };
        }

        private void OnTriggerEnter(Collider _)
        {
            Destroy(gameObject, 0.5f);
        }
    }
}