using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;

namespace Assets.Scripts.Network
{
    public class NetworkController : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinOrCreateRoom(
                "Adventure",
                new RoomOptions {MaxPlayers = 2},
                null
            );
        }

        public override void OnJoinedRoom()
        {
            // PhotonNetwork.Instantiate();
        }
    }
}