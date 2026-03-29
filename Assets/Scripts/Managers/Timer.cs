using UnityEngine;
using TMPro;
using System.Collections;

namespace Assets.Scripts.Managers
{
    public class Timer : MonoBehaviour
    {
        /* ================================================================================================================
        ---------------------------------------------------- MEMBERS -----------------------------------------------------
        ================================================================================================================= */
        [SerializeField] private TMP_Text _time;

        /* ================================================================================================================
        ---------------------------------------------------- UNITY METHODS -----------------------------------------------------
        ================================================================================================================= */

        private void Start()
        {
           StartCoroutine(CountDown());
        }

        /* ================================================================================================================
        ---------------------------------------------------- COUNTDOWN HANDLER -----------------------------------------------------
        ================================================================================================================= */

        private IEnumerator CountDown()
        {
            if (_time == null)
            {
                Debug.LogError("El componente TMP_Text (_time) no está asignado o no es accesible.");
                yield break; // Detenemos la corrutina si _time es null
            }

            float time = 90f;
            
            while(time >= 0)
            {
                // Colors
                if (time <= 10)
                {
                    _time.color = Color.red;
                }
                else if (time <= 30)
                {
                    _time.color = Color.yellow;
                }
                else
                {
                    _time.color = Color.white;
                }

                //Time
                _time.text = time.ToString();
                yield return new WaitForSeconds(1f);
                time --;
            }
        }
    }
}