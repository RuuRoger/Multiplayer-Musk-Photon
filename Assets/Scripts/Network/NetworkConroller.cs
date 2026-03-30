using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Assets.Scripts.Network
{
    public class NetworkController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Material _blue;
        [SerializeField] private Material _red; 
        [SerializeField] private Material _white;
        [SerializeField] private GameObject _timerUI;

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinOrCreateRoom(
                "Adventure",
                new RoomOptions {MaxPlayers = 3},
                null
            );
        }

        public override void OnJoinedRoom()
        {
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            string prefabName = "Player";
            Vector3 spawnPos = Vector3.zero;


            switch (actorNumber)
            {
                case 1:
                    spawnPos = new Vector3(24f, 1f, -24f);
                    gameObject.tag = "Player";
                    break;
                case 2:
                    spawnPos = new Vector3(-24f, 1f, -24f);
                    gameObject.tag = "Player2";
                    break;
                case 3:
                    spawnPos = new Vector3(24f, 1f, 24f);
                    gameObject.tag = "Player3";
                    break;
            }

            object[] initData = new object[] { actorNumber };
            PhotonNetwork.Instantiate(prefabName, spawnPos, Quaternion.identity, 0, initData);
            UIHandler();
        }

        private void UIHandler()
        {
            _panel.SetActive(false);
            _timerUI.SetActive(true);
        }
    }
}