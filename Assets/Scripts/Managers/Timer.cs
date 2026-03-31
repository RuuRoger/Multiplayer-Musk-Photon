using UnityEngine;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System;

namespace Assets.Scripts.Managers
{
    // Network-synced timer using room custom properties and PhotonNetwork.ServerTimestamp.
    public class Timer : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text _time;
        [SerializeField] private float _duration = 90f;

        private const string TimerStartKey = "TimerStart";
        private const string TimerEndedKey = "TimerEnded";

        private bool _isRunning = false;
        private int _startTimestampMs;

        public static event Action OnTimerExpired;

        private void Awake()
        {
            if (_time == null) Debug.LogWarning("Timer: _time TMP_Text not assigned.");
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
            Initialize();
        }

        public override void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            base.OnDisable();
        }

        private void Update()
        {
            if (!_isRunning) return;

            float remaining = TimeRemaining();

            if (remaining > 0f)
            {
                UpdateDisplay(remaining);
            }
            else
            {
                // Expired
                UpdateDisplay(0f);
                _isRunning = false;
                HandleTimerExpired();
            }
        }

        private void UpdateDisplay(float secondsLeft)
        {
            int s = Mathf.CeilToInt(secondsLeft);
            if (_time != null)
            {
                _time.text = s.ToString();
                if (s <= 10) _time.color = Color.red;
                else if (s <= 30) _time.color = Color.yellow;
                else _time.color = Color.white;
            }
        }

        private float TimeRemaining()
        {
            int now = PhotonNetwork.ServerTimestamp; // milliseconds
            float elapsed = (now - _startTimestampMs) / 1000f;
            return _duration - elapsed;
        }

        private void Initialize()
        {
            // If the room already has a start timestamp, use it. If not, the MasterClient initializes it once.
            int propStart;
            if (TryGetStartTime(out propStart))
            {
                _startTimestampMs = propStart;
                _isRunning = TimeRemaining() > 0f && !TryGetTimerEnded();
            }
            else
            {
                if (PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom != null)
                {
                    SetStartTime();
                }
                else
                {
                    _isRunning = false;
                }
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            // Re-evaluate when room props change (start or end)
            Initialize();
        }

        public override void OnJoinedRoom()
        {
            Initialize();
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            // When any player joins, the MasterClient restarts the timer (per user request).
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom != null)
            {
                SetStartTime();
            }
        }

        private static bool TryGetStartTime(out int startTimestamp)
        {
            startTimestamp = PhotonNetwork.ServerTimestamp;
            if (PhotonNetwork.CurrentRoom == null) return false;

            object val;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(TimerStartKey, out val))
            {
                startTimestamp = (int)val;
                return true;
            }

            return false;
        }

        private static bool TryGetTimerEnded()
        {
            if (PhotonNetwork.CurrentRoom == null) return false;
            object val;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(TimerEndedKey, out val))
            {
                try { return Convert.ToBoolean(val); } catch { return false; }
            }
            return false;
        }

        private void SetStartTime()
        {
            if (PhotonNetwork.CurrentRoom == null) return;
            int start = PhotonNetwork.ServerTimestamp;
            // Set start timestamp and clear the ended flag so the timer restarts for everyone.
            var props = new ExitGames.Client.Photon.Hashtable {{ TimerStartKey, start }, { TimerEndedKey, false }};
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);

            // Update local state immediately when MasterClient to reduce perceived latency.
            _startTimestampMs = start;
            _isRunning = true;
        }

        private void SetTimerEnded()
        {
            if (PhotonNetwork.CurrentRoom == null) return;
            var props = new ExitGames.Client.Photon.Hashtable {{ TimerEndedKey, true }};
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }

        private void HandleTimerExpired()
        {
            // Persist the ended state
            SetTimerEnded();

            // Notify listeners
            OnTimerExpired?.Invoke();

            // Stop local gameplay: disable input on local player(s)
            DisableLocalPlayerControls();
        }

        private void DisableLocalPlayerControls()
        {
            var allPVs = FindObjectsOfType<Photon.Pun.PhotonView>();
            foreach (var pv in allPVs)
            {
                if (!pv.IsMine) continue;

                var cp = pv.GetComponent<Assets.Scripts.Player.CustomPlayerController>();
                if (cp != null) cp.enabled = false;

                var shoot = pv.GetComponentInChildren<Assets.Scripts.Gun.Shoot>(true);
                if (shoot != null) shoot.enabled = false;

                // Disable camera rotation controller if present on the local player
                var camCtrl = pv.GetComponentInChildren<Assets.Scripts.CameraFPS.CameraRotationController>(true);
                if (camCtrl != null) camCtrl.enabled = false;
            }
        }
    }
}