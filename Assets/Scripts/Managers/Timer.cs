using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using System.Collections;

namespace Assets.Scripts.Managers
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] public TMP_Text _time;

        private void Start()
        {
           StartCoroutine(CountDown());
        }

        private IEnumerator CountDown()
        {
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