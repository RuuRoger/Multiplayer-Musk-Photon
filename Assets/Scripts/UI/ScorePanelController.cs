using System;
using UnityEngine;
using TMPro;
using Assets.Scripts.Managers;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
    public class ScorePanelController : MonoBehaviour
    {
        [SerializeField] private GameObject _scorePanel;
        [SerializeField] private TMP_Text _blueText;
        [SerializeField] private TMP_Text _redText;
        [SerializeField] private TMP_Text _whiteText;
        [SerializeField] private string _scoreTag = "Score";
        [Header("Material mapping (assign the exact Material used by the Score objects)")]
        [SerializeField] private Material _blueMaterial;
        [SerializeField] private Material _redMaterial;
        [SerializeField] private Material _whiteMaterial;

        [Header("Score Objects")]
        [SerializeField] private GameObject[] _scores;
        [SerializeField] private bool _autoFindScores = true;

        [Header("Actor Mapping (color -> actor number)")]
        [SerializeField] private int _blueActorNumber = 1;
        [SerializeField] private int _redActorNumber = 2;
        [SerializeField] private int _whiteActorNumber = 3;

        private void Awake()
        {
            if (_scorePanel != null) _scorePanel.SetActive(false);
            if (_autoFindScores) RefreshScores();
        }

        private void OnEnable()
        {
            Timer.OnTimerExpired += OnTimerExpired;
        }

        private void OnDisable()
        {
            Timer.OnTimerExpired -= OnTimerExpired;
        }

        private void OnTimerExpired()
        {
            ShowScores();
        }

        public void ShowScores()
        {
            int blue = 0, red = 0, white = 0;

            if (_scores == null || _scores.Length == 0) RefreshScores();

            if (_scores != null && _scores.Length > 0)
            {
                foreach (var go in _scores) CountScoreObjectColor(go, ref blue, ref red, ref white);
            }

            // Show panel and labels (label + value)
            if (_scorePanel != null) _scorePanel.SetActive(true);
            if (_blueText != null)
            {
                blue --;
                if (blue <= 0) blue = 0;
                _blueText.text = $"{blue}";
            } 
            if (_redText != null)
            {
                red --;
                if (red <= 0) red = 0;
                _redText.text = $"{red}";
            } 
            if (_whiteText != null)
            {
                white --;
                if (white <= 0) white = 0;
                _whiteText.text = $"{white}";
            } 

            // Persist scores: MasterClient writes authoritative values for all players; other clients set their own player score locally
            if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom != null)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    foreach (var p in PhotonNetwork.PlayerList)
                    {
                        int val = 0;
                        if (p.ActorNumber == _blueActorNumber) val = blue;
                        else if (p.ActorNumber == _redActorNumber) val = red;
                        else if (p.ActorNumber == _whiteActorNumber) val = white;
                        try { p.SetScore(val); } catch { }
                    }

                    var props = new ExitGames.Client.Photon.Hashtable { { $"Score_{_blueActorNumber}", blue }, { $"Score_{_redActorNumber}", red }, { $"Score_{_whiteActorNumber}", white } };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                }
                else
                {
                    // Non-master: set only local player's score according to mapping so UI/Player.GetScore() reflects it locally
                    int myActor = PhotonNetwork.LocalPlayer.ActorNumber;
                    int myVal = 0;
                    if (myActor == _blueActorNumber) myVal = blue;
                    else if (myActor == _redActorNumber) myVal = red;
                    else if (myActor == _whiteActorNumber) myVal = white;
                    try { PhotonNetwork.LocalPlayer.SetScore(myVal); } catch { }
                }
            }
        }

        private void RefreshScores()
        {
            GameObject[] found = null;
            if (!string.IsNullOrWhiteSpace(_scoreTag) && TagExists(_scoreTag))
            {
                try { found = GameObject.FindGameObjectsWithTag(_scoreTag); } catch { found = null; }
            }

            if (found == null || found.Length == 0)
            {
                var list = new List<GameObject>();
                var renderers = FindObjectsOfType<Renderer>();
                foreach (var r in renderers)
                {
                    if (r == null) continue;
                    if (r.gameObject.name.ToLowerInvariant().Contains("score")) list.Add(r.gameObject);
                }
                found = list.ToArray();
            }

            _scores = found ?? new GameObject[0];
            Debug.Log($"ScorePanelController: RefreshScores found {_scores.Length} score objects");
        }

        private enum ScoreType { Blue, Red, White, Unknown }

        private void CountScoreObjectColor(GameObject go, ref int blue, ref int red, ref int white)
        {
            if (go == null) return;

            var renderers = go.GetComponentsInChildren<Renderer>(true);
            bool counted = false;
            foreach (var r in renderers)
            {
                if (r == null) continue;
                var mats = r.sharedMaterials;
                if (mats == null) continue;
                foreach (var m in mats)
                {
                    if (m == null) continue;
                    var t = GetScoreTypeFromMaterial(m);
                    if (t == ScoreType.Blue) { blue++; counted = true; break; }
                    if (t == ScoreType.Red) { red++; counted = true; break; }
                    if (t == ScoreType.White) { white++; counted = true; break; }
                }
                if (counted) break;
            }
        }

        private ScoreType GetScoreTypeFromMaterial(Material m)
        {
            if (m == null) return ScoreType.Unknown;

            // Manual override (optional): compare to inspector-assigned materials
            if (_blueMaterial != null && MaterialMatches(m, _blueMaterial)) return ScoreType.Blue;
            if (_redMaterial != null && MaterialMatches(m, _redMaterial)) return ScoreType.Red;
            if (_whiteMaterial != null && MaterialMatches(m, _whiteMaterial)) return ScoreType.White;

            // Try matching by material name (supports English/Spanish keywords)
            string name = m.name.ToLowerInvariant();
            if (name.Contains("blue") || name.Contains("azul") || name.Contains("blu")) return ScoreType.Blue;
            if (name.Contains("red") || name.Contains("rojo")) return ScoreType.Red;
            if (name.Contains("white") || name.Contains("blanco")) return ScoreType.White;
            return ScoreType.Unknown;
        }

        private bool MaterialMatches(Material a, Material b)
        {
            if (a == null || b == null) return false;
            if (ReferenceEquals(a, b)) return true;
            // Compare names ignoring Unity's " (Instance)" suffix
            string an = a.name.Replace(" (Instance)", string.Empty);
            string bn = b.name.Replace(" (Instance)", string.Empty);
            if (string.Equals(an, bn, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        // Color fallback removed — detection is by material reference or material name only.

        private bool TagExists(string tag)
        {
            try { GameObject.FindWithTag(tag); return true; } catch { return false; }
        }
    }
}
