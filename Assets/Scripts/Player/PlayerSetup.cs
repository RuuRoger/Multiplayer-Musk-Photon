using System;
using UnityEngine;
using Photon.Pun;

namespace Assets.Scripts.Player
{
    public class PlayerSetup : MonoBehaviour
    {
        private PhotonView _photonViewScript;
        private UnityEngine.Camera _cameraPlayer;
        private AudioListener _audioListenerPlayer;

        private void Start()
        {
            // Fields
            _photonViewScript = GetComponent<PhotonView>();
            _cameraPlayer = GetComponentInChildren<UnityEngine.Camera>();
            _audioListenerPlayer = GetComponentInChildren<AudioListener>();

            // Logical settings
            if (!_photonViewScript.IsMine)
            {
                _cameraPlayer.enabled = false;
                _audioListenerPlayer.enabled = false;
            }       
        }

    }
}