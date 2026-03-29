using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public event Action<string> OnPlayerAwaked;

        private void Awake()
        {
            string tagPlayer = gameObject.tag;
            OnPlayerAwaked?.Invoke(tagPlayer);
        }
    }
}