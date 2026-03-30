using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System;

namespace Assets.Scripts.Environment
{
    public class Environment : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        /* ================================================================================================================
        ---------------------------------------------------- MEMBERS -----------------------------------------------------
        ================================================================================================================= */
        [SerializeField] private Material _blue;
        [SerializeField] private Material _red;
        [SerializeField] private Material _white;
        [SerializeField] private string _environmentId;
        private Renderer _renderer;

        /* ================================================================================================================
        ---------------------------------------------------- UNITY LIFE CYCLE METHODS -----------------------------------------------------
        ================================================================================================================= */
        private const byte ENVIRONMENT_COLOR_EVENT = 101;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();

            // Ensure a stable id for this environment object (use inspector value or sanitized name)
            if (string.IsNullOrWhiteSpace(_environmentId))
            {
                _environmentId = gameObject.name.Replace("(Clone)", string.Empty).Trim();
            }

            // If materials not assigned in inspector, try to load defaults from Bullet prefab
            if (_blue == null || _red == null || _white == null)
            {
                var mats = Assets.Scripts.Gun.BulletBehaviour.LoadDefaultMaterials();
                if (_blue == null) _blue = mats[0];
                if (_red == null) _red = mats[1];
                if (_white == null) _white = mats[2];
            }
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        /* ================================================================================================================
        ---------------------------------------------------- COLLISION -----------------------------------------------------
        ================================================================================================================= */
        private void OnTriggerEnter(Collider other)
        {
            var pv = other.GetComponentInParent<Photon.Pun.PhotonView>();
            int actorNumber = -1;

            if (pv != null)
            {
                if (pv.InstantiationData != null && pv.InstantiationData.Length > 0)
                {
                    try { actorNumber = Convert.ToInt32(pv.InstantiationData[0]); } catch { actorNumber = pv.Owner != null ? pv.Owner.ActorNumber : -1; }
                }
                else
                {
                    actorNumber = pv.Owner != null ? pv.Owner.ActorNumber : -1;
                }
            }
            else
            {
                // fallback: determine from bullet tag
                if (other.gameObject.tag == "BulletBlue") actorNumber = 1;
                else if (other.gameObject.tag == "BulletRed") actorNumber = 2;
                else if (other.gameObject.tag == "BulletWhite") actorNumber = 3;
            }

                if (actorNumber != -1)
            {
                // Persist color in room properties so late joiners get the current color
                if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom != null)
                {
                        string key = $"EnvColor_{_environmentId}";
                        var props = new ExitGames.Client.Photon.Hashtable { { key, actorNumber } };
                        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                }

                // Also raise an event so all current clients apply it immediately
                object[] content = new object[] { _environmentId, actorNumber };
                var options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(ENVIRONMENT_COLOR_EVENT, content, options, new SendOptions { Reliability = true });
            }
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code != ENVIRONMENT_COLOR_EVENT) return;

            var data = photonEvent.CustomData as object[];
            if (data == null || data.Length < 2) return;

            string targetId = data[0] as string;
            int actorNumber = Convert.ToInt32(data[1]);

            if (targetId != _environmentId) return;

            ApplyColor(actorNumber);
        }

        private void Start()
        {
            // On start, try to apply any cached color from the room (may be null if not in room yet)
            TryApplyCachedColor();
        }

        public override void OnJoinedRoom()
        {
            // When joining a room, ensure we apply any cached environment color
            TryApplyCachedColor();
        }

        private void TryApplyCachedColor()
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom != null)
            {
                string key = $"EnvColor_{_environmentId}";
                var props = PhotonNetwork.CurrentRoom.CustomProperties;
                if (props != null && props.ContainsKey(key))
                {
                    try
                    {
                        int actorNumber = Convert.ToInt32(props[key]);
                        ApplyColor(actorNumber);
                    }
                    catch { }
                }
            }
        }

        private void ApplyColor(int actorNumber)
        {
            if (actorNumber == 1)
            {
                if (_blue != null) _renderer.material = _blue; else _renderer.material.color = Color.blue;
            }
            else if (actorNumber == 2)
            {
                if (_red != null) _renderer.material = _red; else _renderer.material.color = Color.red;
            }
            else if (actorNumber == 3)
            {
                if (_white != null) _renderer.material = _white; else _renderer.material.color = Color.white;
            }
        }
    }
}