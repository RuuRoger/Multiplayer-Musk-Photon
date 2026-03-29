using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
                new RoomOptions {MaxPlayers = 3},
                null
            );
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.Instantiate("Player", new Vector3(24f, 1f, -24f), Quaternion.identity);
        }
    }
}