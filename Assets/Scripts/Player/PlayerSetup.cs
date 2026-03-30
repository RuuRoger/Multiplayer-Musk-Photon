using System;
using UnityEngine;
using Photon.Pun;
using Assets.Scripts.Ammo;
using Assets.Scripts.CameraFPS;
using Assets.Scripts.Managers;
using Assets.Scripts.Gun;
using System.Collections;

namespace Assets.Scripts.Player
{
    public class PlayerSetup : MonoBehaviour
    {
        [SerializeField] private Material _blue;
        [SerializeField] private Material _red;
        [SerializeField] private Material _white;
        private PhotonView _photonViewScript;
        private CameraRotationController _cameraPlayer;
        private AudioListener _audioListenerPlayer;
        private CustomPlayerController _customPlayerControllerScript;
        private CameraRotationController _cameraRotationController;
        private GetAmmo _getAmmoScript;
        private AmmoHandler _ammoHandlerScirpt;
        private Shoot _shootScript;

        private void Awake()
        {
            // Fields
            _photonViewScript = GetComponent<PhotonView>();
            _customPlayerControllerScript = GetComponent<CustomPlayerController>();
            _cameraRotationController = GetComponentInChildren<CameraRotationController>(true);
            _getAmmoScript = GetComponent<GetAmmo>();
            _ammoHandlerScirpt = GetComponent<AmmoHandler>();
            _cameraPlayer = GetComponentInChildren<CameraRotationController>(true);
            _audioListenerPlayer = GetComponentInChildren<AudioListener>(true);
            _shootScript = GetComponentInChildren<Shoot>(true);

            // If materials not assigned in the prefab, try to load defaults from Bullet prefab
            if (_blue == null || _red == null || _white == null)
            {
                var mats = Assets.Scripts.Gun.BulletBehaviour.LoadDefaultMaterials();
                if (_blue == null) _blue = mats[0];
                if (_red == null) _red = mats[1];
                if (_white == null) _white = mats[2];
            }

            // Asignar material y tag según el actorNumber recibido en InstantiationData (se aplica en todos los clientes)
            int actorNumber = 1;
            if (_photonViewScript != null && _photonViewScript.InstantiationData != null && _photonViewScript.InstantiationData.Length > 0)
            {
                try { actorNumber = (int)_photonViewScript.InstantiationData[0]; } catch { actorNumber = PhotonNetwork.LocalPlayer.ActorNumber; }
            }
            else if (_photonViewScript != null && _photonViewScript.IsMine)
            {
                actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            }

            var meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
            switch (actorNumber)
            {
                case 1:
                    foreach (var r in meshRenderers) if (r != null) r.material = _blue;
                    gameObject.tag = "Player";
                    break;
                case 2:
                    foreach (var r in meshRenderers) if (r != null) r.material = _red;
                    gameObject.tag = "Player2";
                    break;
                case 3:
                    foreach (var r in meshRenderers) if (r != null) r.material = _white;
                    gameObject.tag = "Player3";
                    break;
                default:
                    break;
            }

            // Desactivar inmediatamente los componentes del jugador remoto
            if (_photonViewScript != null && !_photonViewScript.IsMine)
            {
                var cam = GetComponentInChildren<Camera>(true);
                if (cam != null) cam.enabled = false;
                if (_audioListenerPlayer != null) _audioListenerPlayer.enabled = false;
                if (_customPlayerControllerScript != null) _customPlayerControllerScript.enabled = false;
                if (_cameraRotationController != null) _cameraRotationController.enabled = false;
                if (_getAmmoScript != null) _getAmmoScript.enabled = false;
                if (_ammoHandlerScirpt != null) _ammoHandlerScirpt.enabled = false;
                if (_shootScript != null) _shootScript.enabled = false;
            }
        }
    }
}